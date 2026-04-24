using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using DeviceLicenseSaleApi.Configuration;
using DeviceLicenseSaleApi.DTOs;
using Microsoft.Extensions.Options;

namespace DeviceLicenseSaleApi.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly HttpClient _httpClient;
        private readonly PayPalOptions _options;

        public PayPalService(HttpClient httpClient, IOptions<PayPalOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;

            if (Uri.TryCreate(_options.BaseUrl, UriKind.Absolute, out var baseUri))
            {
                _httpClient.BaseAddress = baseUri;
            }
        }

        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(_options.BaseUrl) &&
            !string.IsNullOrWhiteSpace(_options.ClientId) &&
            !string.IsNullOrWhiteSpace(_options.ClientSecret);

        public PayPalClientConfigDto GetClientConfig()
        {
            EnsureConfigured();

            return new PayPalClientConfigDto
            {
                ClientId = _options.ClientId,
                CurrencyCode = _options.CurrencyCode
            };
        }

        public async Task<PayPalOrderResponseDto> CreateOrderAsync(PayPalCreateOrderRequestDto dto, CancellationToken cancellationToken = default)
        {
            EnsureConfigured();

            var accessToken = await GetAccessTokenAsync(cancellationToken);
            var currencyCode = string.IsNullOrWhiteSpace(dto.CurrencyCode)
                ? _options.CurrencyCode
                : dto.CurrencyCode.ToUpperInvariant();

            var payload = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        reference_id = $"device-{dto.DeviceId}",
                        description = dto.Description,
                        amount = new
                        {
                            currency_code = currencyCode,
                            value = dto.Amount.ToString("0.00", CultureInfo.InvariantCulture)
                        }
                    }
                }
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "v2/checkout/orders")
            {
                Content = JsonContent.Create(payload)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            using var document = await ReadJsonDocumentAsync(response, cancellationToken, "PayPal order creation failed.");

            return new PayPalOrderResponseDto
            {
                Id = GetRequiredString(document.RootElement, "id", "PayPal did not return an order ID."),
                Status = GetString(document.RootElement, "status") ?? "CREATED"
            };
        }

        public async Task<PayPalCaptureResponseDto> CaptureOrderAsync(string orderId, CancellationToken cancellationToken = default)
        {
            EnsureConfigured();

            var accessToken = await GetAccessTokenAsync(cancellationToken);
            using var request = new HttpRequestMessage(HttpMethod.Post, $"v2/checkout/orders/{Uri.EscapeDataString(orderId)}/capture")
            {
                Content = JsonContent.Create(new { })
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            using var document = await ReadJsonDocumentAsync(response, cancellationToken, "PayPal capture failed.");

            return new PayPalCaptureResponseDto
            {
                OrderId = GetRequiredString(document.RootElement, "id", "PayPal did not return an order ID after capture."),
                Status = GetString(document.RootElement, "status") ?? string.Empty,
                CaptureId = TryGetCaptureId(document.RootElement),
                PayerEmail = TryGetPayerEmail(document.RootElement)
            };
        }

        private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "v1/oauth2/token");
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_options.ClientId}:{_options.ClientSecret}")));
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            using var document = await ReadJsonDocumentAsync(response, cancellationToken, "PayPal authentication failed.");

            return GetRequiredString(document.RootElement, "access_token", "PayPal did not return an access token.");
        }

        private void EnsureConfigured()
        {
            if (IsConfigured)
            {
                return;
            }

            throw new InvalidOperationException(
                "PayPal is not configured on the API. Add PayPal:ClientId and PayPal:ClientSecret to appsettings.Development.json.");
        }

        private static async Task<JsonDocument> ReadJsonDocumentAsync(HttpResponseMessage response, CancellationToken cancellationToken, string fallbackMessage)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    ExtractErrorMessage(content) ?? fallbackMessage,
                    null,
                    response.StatusCode);
            }

            return JsonDocument.Parse(string.IsNullOrWhiteSpace(content) ? "{}" : content);
        }

        private static string? ExtractErrorMessage(string? responseContent)
        {
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                return null;
            }

            try
            {
                using var document = JsonDocument.Parse(responseContent);
                var root = document.RootElement;

                var detailMessage = TryGetNestedString(root, "details", 0, "description");
                if (!string.IsNullOrWhiteSpace(detailMessage))
                {
                    return detailMessage;
                }

                return GetString(root, "message");
            }
            catch (JsonException)
            {
                return responseContent;
            }
        }

        private static string? TryGetCaptureId(JsonElement root)
        {
            return TryGetNestedString(root, "purchase_units", 0, "payments", "captures", 0, "id");
        }

        private static string? TryGetPayerEmail(JsonElement root)
        {
            return TryGetNestedString(root, "payer", "email_address");
        }

        private static string GetRequiredString(JsonElement element, string propertyName, string errorMessage)
        {
            return GetString(element, propertyName) ?? throw new InvalidOperationException(errorMessage);
        }

        private static string? GetString(JsonElement element, string propertyName)
        {
            if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.String)
            {
                return null;
            }

            return property.GetString();
        }

        private static string? TryGetNestedString(JsonElement element, params object[] path)
        {
            JsonElement current = element;

            foreach (var segment in path)
            {
                if (segment is string propertyName)
                {
                    if (!current.TryGetProperty(propertyName, out current))
                    {
                        return null;
                    }
                }
                else if (segment is int index)
                {
                    if (current.ValueKind != JsonValueKind.Array || current.GetArrayLength() <= index)
                    {
                        return null;
                    }

                    current = current[index];
                }
            }

            return current.ValueKind == JsonValueKind.String ? current.GetString() : null;
        }
    }
}
