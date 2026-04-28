using DeviceLicenseSaleApi.Services;
using DeviceLicenseSaleApi.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceLicenseSaleApi.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<AdministrativeService>();
            services.AddScoped<CallAnsweringService>();
            services.AddScoped<CallManagementService>();
            services.AddScoped<CallScreeningService>();
            services.AddScoped<CostBandwidthSavingService>();
            services.AddScoped<GroupConvenienceService>();
            services.AddScoped<SecurityToolsService>();
            services.AddScoped<UnifiedCommunicationsService>();
            services.AddScoped<UtilityService>();
            services.AddScoped<FeaturesService>();
            services.AddScoped<LicensableFeaturesService>();
            services.AddScoped<DeviceTypesService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IBuildingService, BuildingService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<ILicenseService, LicenseService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IActivityLogService, ActivityLogService>();

            return services;
        }
    }
}
