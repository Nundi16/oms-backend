using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OMS.Common;
using OMS.Common.Interfaces;
using OMS.Presentation.Authorization;

namespace OMS.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, HttpUserContext>();

            services.AddFusionAuthJwtBearer(configuration);

            return services;
        }

        private static IServiceCollection AddFusionAuthJwtBearer(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("Authentication");
            var authority = section["Authority"]
                ?? throw new InvalidOperationException("Missing configuration value 'Authentication:Authority'.");

            var audiences = section.GetSection("Audiences").Get<string[]>()
                ?? (section["Audience"] is { Length: > 0 } single ? new[] { single } : Array.Empty<string>());

            if (audiences.Length == 0)
            {
                throw new InvalidOperationException("Missing configuration value 'Authentication:Audiences' (or 'Authentication:Audience').");
            }

            var requireHttpsMetadata = section.GetValue("RequireHttpsMetadata", defaultValue: true);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    // FusionAuth exposes OIDC discovery at {authority}/.well-known/openid-configuration,
                    // which yields the JWKS endpoint and the canonical issuer string.
                    options.Authority = authority;
                    options.RequireHttpsMetadata = requireHttpsMetadata;

                    // Keep claim types as the JWT spec emits them (sub, roles, email, ...)
                    // so HttpUserContext and AuthorizationGuard see the FusionAuth claim names directly.
                    options.MapInboundClaims = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudiences = audiences,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        NameClaimType = "preferred_username",
                        RoleClaimType = Constants.Auth.ROLE_CLAIM_TYPE,
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}
