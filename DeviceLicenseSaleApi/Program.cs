using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Helpers;
using DeviceLicenseSaleApi.Repositories;
using DeviceLicenseSaleApi.Services;
using DeviceLicenseSaleApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DeviceLicenseSaleApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("VueDevPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:5173", "https://localhost:5173", "http://localhost:5177", "https://localhost:5177")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });



            // ✅ Connection string from appsettings.json
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ✅ Register repositories
            builder.Services.AddScoped<IAdministrativeRepository, AdministrativeRepository>();
            builder.Services.AddScoped<ICallAnsweringRepository, CallAnsweringRepository>();
            builder.Services.AddScoped<ICallManagementRepository, CallManagementRepository>();
            builder.Services.AddScoped<ICallScreeningRepository, CallScreeningRepository>();
            builder.Services.AddScoped<ICostBandwidthSavingRepository, CostBandwidthSavingRepository>();
            builder.Services.AddScoped<IGroupConvenienceRepository, GroupConvenienceRepository>();
            builder.Services.AddScoped<ISecurityToolsRepository, SecurityToolsRepository>();
            builder.Services.AddScoped<IUnifiedCommunicationsRepository, UnifiedCommunicationsRepository>();
            builder.Services.AddScoped<IUtilityRepository, UtilityRepository>();
            builder.Services.AddScoped<IFeaturesRepository, FeaturesRepository>();
            builder.Services.AddScoped<ILicensableFeaturesRepository, LicensableFeaturesRepository>();
            builder.Services.AddScoped<IDeviceTypesRepository, DeviceTypesRepository>();
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();

            // ✅ Register services
            builder.Services.AddScoped<AdministrativeService>();
            builder.Services.AddScoped<CallAnsweringService>();
            builder.Services.AddScoped<CallManagementService>();
            builder.Services.AddScoped<CallScreeningService>();
            builder.Services.AddScoped<CostBandwidthSavingService>();
            builder.Services.AddScoped<GroupConvenienceService>();
            builder.Services.AddScoped<SecurityToolsService>();
            builder.Services.AddScoped<UnifiedCommunicationsService>();
            builder.Services.AddScoped<UtilityService>();
            builder.Services.AddScoped<FeaturesService>();
            builder.Services.AddScoped<LicensableFeaturesService>();
            builder.Services.AddScoped<DeviceTypesService>();
            builder.Services.AddScoped<ICompanyService, CompanyService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserProfileService, UserProfileService>();
            builder.Services.AddScoped<IBuildingService, BuildingService>();


            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<JwtHelper>();

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            builder.Services.AddAuthorization();
            // ✅ Add controllers
            builder.Services.AddControllers();

            // ✅ Swagger (optional, for testing APIs)
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // ✅ Enable Swagger UI in development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("VueDevPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}
