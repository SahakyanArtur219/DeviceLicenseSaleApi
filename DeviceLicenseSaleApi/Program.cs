using System.Security.Claims;
using System.Text;
using DeviceLicenseSaleApi.Configuration;
using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Helpers;
using DeviceLicenseSaleApi.Repositories;
using DeviceLicenseSaleApi.Services;
using DeviceLicenseSaleApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
                    policy.WithOrigins(
                            "http://localhost:5173",
                            "https://localhost:5173",
                            "http://localhost:5177",
                            "https://localhost:5177")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.Configure<JwtOptions>(
                builder.Configuration.GetSection(JwtOptions.SectionName));

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
            builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
            builder.Services.AddScoped<ILicenseRepository, LicenseRepository>();

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
            builder.Services.AddScoped<IDeviceService, DeviceService>();
            builder.Services.AddScoped<ILicenseService, LicenseService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<JwtHelper>();

            var jwtOptions = builder.Configuration
                .GetSection(JwtOptions.SectionName)
                .Get<JwtOptions>() ?? new JwtOptions();

            if (string.IsNullOrWhiteSpace(jwtOptions.Key))
            {
                throw new InvalidOperationException("Jwt:Key is not configured.");
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = signingKey,
                        NameClaimType = ClaimTypes.Name,
                        RoleClaimType = ClaimTypes.Role,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your JWT token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

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
