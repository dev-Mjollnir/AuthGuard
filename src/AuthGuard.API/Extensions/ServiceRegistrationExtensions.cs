using AuthGuard.API.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthGuard.API.Extensions
{
    public static class ServiceRegistrationExtensions
    {
        public static void RegisterJWTAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = builder.Configuration["AuthenticationSettings:Authority"];

                //Token'daki 'JwtRegisteredClaimNames.Aud' karşılık verilen değer
                options.Audience = builder.Configuration["AuthenticationSettings:Audience"];

                options.RequireHttpsMetadata = false;

                //Gelen token'da 'scope' claim'i içerisinde yan yana yazılmış
                //yetkileri ayırarak tek tek scope claim'i olarak tekrardan ayarlayabilmek için
                //token'ın doğrulanması ardından 'OnTokenValidated' event'inde aşağıdaki işlemi yaptık
                options.Events = new()
                {
                    OnTokenValidated = async context =>
                    {
                        if (context.Principal?.Identity is ClaimsIdentity claimsIdentity)
                        {
                            Claim? scopeClaim = claimsIdentity.FindFirst("scope");
                            if (scopeClaim is not null)
                            {
                                claimsIdentity.RemoveClaim(scopeClaim);
                                claimsIdentity.AddClaims(scopeClaim.Value.Split(" ").Select(s => new Claim("scope", s)).ToList());
                            }
                        }

                        await Task.CompletedTask;
                    }
                };
            });
        }

        public static void RegisterPolicy(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("APolicy", policy => policy.RequireClaim("scope", "read"));
                options.AddPolicy("BPolicy", policy => policy.RequireClaim("scope", "write"));
                options.AddPolicy("CPolicy", policy => policy.RequireClaim("scope", "read", "write"));
                options.AddPolicy("DPolicy", policy => policy.RequireClaim("custom-claim", "custom-claim-value"));
            });
        }

        public static void RegisterDbContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<EmployeeContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));
            });
        }
    }
}
