using System.Text.RegularExpressions;
using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Repositories;
using DeviceLicenseSaleApi.Services.Interfaces;

namespace DeviceLicenseSaleApi.Services
{
    public class DeviceRecommendationService : IDeviceRecommendationService
    {
        private static readonly Regex NumberedRequirementRegex = new(@"(?<count>\d+)\s+(?<label>ip phones?|analog phones?|concurrent calls?|assistants?|employees?|agents?|users?|staff members?|staff|people)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private readonly IDeviceTypesRepository _deviceTypesRepository;
        private readonly ILicensableFeaturesRepository _licensableFeaturesRepository;

        public DeviceRecommendationService(
            IDeviceTypesRepository deviceTypesRepository,
            ILicensableFeaturesRepository licensableFeaturesRepository)
        {
            _deviceTypesRepository = deviceTypesRepository;
            _licensableFeaturesRepository = licensableFeaturesRepository;
        }

        public DeviceRecommendationChatResponseDto Recommend(DeviceRecommendationChatRequestDto request)
        {
            var message = request.Message?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(message))
            {
                return new DeviceRecommendationChatResponseDto
                {
                    Summary = "No requirements were provided.",
                    AssistantMessage = "Describe the number of phones, concurrent calls, or team size you need, and I will suggest the best matching device type.",
                    Notes = new[]
                    {
                        "Example: I have a call center with 10 assistants and I need 10 IP phones and 2 concurrent calls."
                    }
                };
            }

            var requirements = ExtractRequirements(message);
            var notes = new List<string>();

            if (!requirements.Any())
            {
                notes.Add("I could not detect capacity requirements from the message.");
                return new DeviceRecommendationChatResponseDto
                {
                    Summary = "More detail is needed.",
                    AssistantMessage = "Please include counts such as IP phones, analog phones, concurrent calls, or team size so I can recommend the right device type.",
                    Notes = notes
                };
            }

            if (requirements.Any(x => x.Inferred))
            {
                notes.Add("Some requirements were inferred from company size because exact phone counts were not provided.");
            }

            var deviceTypes = _deviceTypesRepository.GetAll().ToList();
            var licensableById = _licensableFeaturesRepository.GetAll().ToDictionary(item => item.Id);
            var options = BuildRecommendations(deviceTypes, licensableById, requirements)
                .Take(3)
                .ToList();

            if (!options.Any())
            {
                notes.Add("No stored device type can currently satisfy the requested capacities.");
                return new DeviceRecommendationChatResponseDto
                {
                    Summary = "No matching device type found.",
                    AssistantMessage = "I could not find a device type in the catalog that satisfies the requested capacities.",
                    Requirements = requirements,
                    Notes = notes
                };
            }

            var best = options[0];
            var summary = $"{best.Quantity} x {best.DeviceTypeName}";
            var assistantMessage = best.UsesLicensableIpPhones
                ? $"I recommend {summary}. Its base capacity gives you {best.BaseIpPhones} IP phones, and adding {best.LicensableIpPhones} licensable IP phones brings the total to {best.ProvidedIpPhones}. It also covers {best.ProvidedConcurrentCalls} concurrent calls and {best.ProvidedTotalPhones} total phone capacity."
                : $"I recommend {summary}. This covers {best.ProvidedIpPhones} IP phones, {best.ProvidedConcurrentCalls} concurrent calls, and {best.ProvidedTotalPhones} total phone capacity based on your request.";

            if (options.Any(option => option.UsesLicensableIpPhones))
            {
                notes.Add("Recommendations may prefer a smaller base device when licensable IP phone expansion can cover the remaining required phones.");
            }

            return new DeviceRecommendationChatResponseDto
            {
                Summary = summary,
                AssistantMessage = assistantMessage,
                Requirements = requirements,
                Recommendations = options,
                Notes = notes
            };
        }

        private static List<DeviceRecommendationRequirementDto> ExtractRequirements(string message)
        {
            var requirements = new List<DeviceRecommendationRequirementDto>();
            var lowered = message.ToLowerInvariant();

            foreach (Match match in NumberedRequirementRegex.Matches(message))
            {
                var count = int.Parse(match.Groups["count"].Value);
                var label = match.Groups["label"].Value.ToLowerInvariant();

                if (label.Contains("ip phone"))
                {
                    AddOrUpdate(requirements, "IP Phones", count, false, match.Value);
                }
                else if (label.Contains("analog phone"))
                {
                    AddOrUpdate(requirements, "Analog Phones", count, false, match.Value);
                }
                else if (label.Contains("concurrent call"))
                {
                    AddOrUpdate(requirements, "Concurrent Calls", count, false, match.Value);
                }
                else if (label.Contains("assistant") || label.Contains("employee") || label.Contains("agent") || label.Contains("user") || label.Contains("staff") || label.Contains("people"))
                {
                    AddOrUpdate(requirements, "Company Size", count, false, match.Value);
                }
            }

            var companySize = requirements.FirstOrDefault(x => x.Name == "Company Size")?.Requested;
            var ipPhones = requirements.FirstOrDefault(x => x.Name == "IP Phones");

            if (companySize.HasValue && ipPhones == null)
            {
                AddOrUpdate(requirements, "IP Phones", companySize.Value, true, "inferred from company size");
            }

            if (lowered.Contains("call center") && requirements.All(x => x.Name != "Concurrent Calls"))
            {
                var baseCount = requirements.FirstOrDefault(x => x.Name == "IP Phones")?.Requested ?? companySize ?? 0;
                if (baseCount > 0)
                {
                    AddOrUpdate(requirements, "Concurrent Calls", Math.Max(2, (int)Math.Ceiling(baseCount * 0.5m)), true, "inferred from call center usage");
                }
            }

            return requirements
                .Where(x => x.Name != "Company Size")
                .ToList();
        }

        private static IEnumerable<DeviceRecommendationOptionDto> BuildRecommendations(
            IEnumerable<Models.DeviceTypes> deviceTypes,
            IReadOnlyDictionary<int, Models.LicensableFeatures> licensableById,
            IReadOnlyCollection<DeviceRecommendationRequirementDto> requirements)
        {
            var requiredIpPhones = requirements.FirstOrDefault(x => x.Name == "IP Phones")?.Requested ?? 0;
            var requiredAnalogPhones = requirements.FirstOrDefault(x => x.Name == "Analog Phones")?.Requested ?? 0;
            var requiredConcurrentCalls = requirements.FirstOrDefault(x => x.Name == "Concurrent Calls")?.Requested ?? 0;
            var requiredTotalPhones = requirements.FirstOrDefault(x => x.Name == "Total Phones")?.Requested ?? 0;

            return deviceTypes
                .Select(type => BuildOption(type, licensableById, requiredIpPhones, requiredAnalogPhones, requiredConcurrentCalls, requiredTotalPhones))
                .Where(option => option != null)
                .OrderBy(option => option!.Quantity)
                .ThenBy(option => option!.UsesLicensableIpPhones ? 0 : 1)
                .ThenBy(option => CalculateOversupplyScore(option!, requiredIpPhones, requiredAnalogPhones, requiredConcurrentCalls, requiredTotalPhones))
                .ThenBy(option => option!.DeviceTypeName)
                .Select(option => option!);
        }

        private static DeviceRecommendationOptionDto? BuildOption(
            Models.DeviceTypes type,
            IReadOnlyDictionary<int, Models.LicensableFeatures> licensableById,
            int requiredIpPhones,
            int requiredAnalogPhones,
            int requiredConcurrentCalls,
            int requiredTotalPhones)
        {
            var quantity = 1;

            if (requiredAnalogPhones > 0)
            {
                if (type.AnalogPhones <= 0)
                {
                    return null;
                }

                quantity = Math.Max(quantity, DivideAndRoundUp(requiredAnalogPhones, type.AnalogPhones));
            }

            if (requiredConcurrentCalls > 0)
            {
                if (type.ConcurrentCalls <= 0)
                {
                    return null;
                }

                quantity = Math.Max(quantity, DivideAndRoundUp(requiredConcurrentCalls, type.ConcurrentCalls));
            }

            if (requiredTotalPhones > 0)
            {
                if (type.TotalPhones <= 0)
                {
                    return null;
                }

                quantity = Math.Max(quantity, DivideAndRoundUp(requiredTotalPhones, type.TotalPhones));
            }

            if (requiredIpPhones > 0 && type.IPPhones <= 0)
            {
                return null;
            }

            var baseIpPhones = quantity * type.IPPhones;
            var licensable = licensableById.TryGetValue(type.LicensableFeatureId, out var licensableFeatures)
                ? licensableFeatures
                : null;
            var ipPhoneExpansionPerUnit = Math.Max(0, licensable?.IPPhoneExpansionKey ?? 0);
            var maxLicensableIpPhones = quantity * ipPhoneExpansionPerUnit;
            var remainingIpPhoneNeed = Math.Max(0, requiredIpPhones - baseIpPhones);
            var licensableIpPhones = Math.Min(remainingIpPhoneNeed, maxLicensableIpPhones);
            var providedIpPhones = baseIpPhones + licensableIpPhones;

            if (requiredIpPhones > 0 && providedIpPhones < requiredIpPhones)
            {
                quantity = Math.Max(quantity, DivideAndRoundUp(requiredIpPhones, type.IPPhones));
                baseIpPhones = quantity * type.IPPhones;
                maxLicensableIpPhones = quantity * ipPhoneExpansionPerUnit;
                remainingIpPhoneNeed = Math.Max(0, requiredIpPhones - baseIpPhones);
                licensableIpPhones = Math.Min(remainingIpPhoneNeed, maxLicensableIpPhones);
                providedIpPhones = baseIpPhones + licensableIpPhones;

                if (providedIpPhones < requiredIpPhones)
                {
                    return null;
                }
            }

            var providedAnalogPhones = quantity * type.AnalogPhones;
            var providedConcurrentCalls = quantity * type.ConcurrentCalls;
            var providedTotalPhones = quantity * type.TotalPhones;

            return new DeviceRecommendationOptionDto
            {
                DeviceTypeId = type.Id,
                DeviceTypeName = type.Name,
                Quantity = quantity,
                ProvidedIpPhones = providedIpPhones,
                ProvidedAnalogPhones = providedAnalogPhones,
                ProvidedConcurrentCalls = providedConcurrentCalls,
                ProvidedTotalPhones = providedTotalPhones,
                BaseIpPhones = baseIpPhones,
                LicensableIpPhones = licensableIpPhones,
                UsesLicensableIpPhones = licensableIpPhones > 0,
                Reason = BuildReason(type.Name, quantity, baseIpPhones, licensableIpPhones, providedAnalogPhones, providedConcurrentCalls, providedTotalPhones)
            };
        }

        private static string BuildReason(
            string deviceTypeName,
            int quantity,
            int baseIpPhones,
            int licensableIpPhones,
            int providedAnalogPhones,
            int providedConcurrentCalls,
            int providedTotalPhones)
        {
            if (licensableIpPhones > 0)
            {
                return $"{quantity} unit(s) of {deviceTypeName} provide {baseIpPhones} base IP phones, and {licensableIpPhones} more can be added with licensable IP phone keys. This also provides {providedAnalogPhones} analog phones, {providedConcurrentCalls} concurrent calls, and {providedTotalPhones} total phones.";
            }

            return $"{quantity} unit(s) of {deviceTypeName} provide {baseIpPhones} IP phones, {providedAnalogPhones} analog phones, {providedConcurrentCalls} concurrent calls, and {providedTotalPhones} total phones.";
        }

        private static int CalculateOversupplyScore(
            DeviceRecommendationOptionDto option,
            int requiredIpPhones,
            int requiredAnalogPhones,
            int requiredConcurrentCalls,
            int requiredTotalPhones)
        {
            var score = 0;
            score += Math.Max(0, option.ProvidedIpPhones - requiredIpPhones) * 4;
            score += Math.Max(0, option.BaseIpPhones - requiredIpPhones) * 6;
            score += Math.Max(0, option.ProvidedAnalogPhones - requiredAnalogPhones) * 4;
            score += Math.Max(0, option.ProvidedConcurrentCalls - requiredConcurrentCalls) * 5;
            score += Math.Max(0, option.ProvidedTotalPhones - requiredTotalPhones) * 2;
            return score;
        }

        private static int DivideAndRoundUp(int required, int capacity)
        {
            return (int)Math.Ceiling(required / (decimal)capacity);
        }

        private static void AddOrUpdate(
            ICollection<DeviceRecommendationRequirementDto> requirements,
            string name,
            int requested,
            bool inferred,
            string sourceText)
        {
            var existing = requirements.FirstOrDefault(x => x.Name == name);

            if (existing == null)
            {
                requirements.Add(new DeviceRecommendationRequirementDto
                {
                    Name = name,
                    Requested = requested,
                    Inferred = inferred,
                    SourceText = sourceText
                });
                return;   
            }

            existing.Requested = Math.Max(existing.Requested, requested);
            existing.Inferred = existing.Inferred && inferred;
            existing.SourceText = sourceText;
        }
    }
}