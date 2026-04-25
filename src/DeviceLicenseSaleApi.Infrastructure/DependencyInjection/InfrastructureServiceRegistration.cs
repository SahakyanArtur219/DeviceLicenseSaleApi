using DeviceLicenseSaleApi.Configuration;
using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Helpers;
using DeviceLicenseSaleApi.Repositories;
using DeviceLicenseSaleApi.Services;
using DeviceLicenseSaleApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceLicenseSaleApi.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.Configure<JwtOptions>(
                configuration.GetSection(JwtOptions.SectionName));
            services.Configure<PayPalOptions>(
                configuration.GetSection(PayPalOptions.SectionName));

            services.AddScoped<IAdministrativeRepository, AdministrativeRepository>();
            services.AddScoped<ICallAnsweringRepository, CallAnsweringRepository>();
            services.AddScoped<ICallManagementRepository, CallManagementRepository>();
            services.AddScoped<ICallScreeningRepository, CallScreeningRepository>();
            services.AddScoped<ICostBandwidthSavingRepository, CostBandwidthSavingRepository>();
            services.AddScoped<IGroupConvenienceRepository, GroupConvenienceRepository>();
            services.AddScoped<ISecurityToolsRepository, SecurityToolsRepository>();
            services.AddScoped<IUnifiedCommunicationsRepository, UnifiedCommunicationsRepository>();
            services.AddScoped<IUtilityRepository, UtilityRepository>();
            services.AddScoped<IFeaturesRepository, FeaturesRepository>();
            services.AddScoped<ILicensableFeaturesRepository, LicensableFeaturesRepository>();
            services.AddScoped<IDeviceTypesRepository, DeviceTypesRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IBuildingRepository, BuildingRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<ILicenseRepository, LicenseRepository>();

            services.AddScoped<IJwtTokenService, JwtHelper>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddHttpClient<IPayPalService, PayPalService>();

            return services;
        }
    }
}
