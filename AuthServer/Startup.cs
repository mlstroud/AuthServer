using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using AuthServer.Authorization;
using AuthServer.Library.Config;
using AuthServer.Library.Repositories;
using AuthServer.Library.Services;

namespace AuthServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = GetTokenValidationParameters();
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.GetData, policy =>
                {
                    policy.RequireClaim(ClaimType.Data, ClaimValue.Get);
                });
                options.AddPolicy(Policies.Member, policy =>
                {
                    policy.RequireClaim(ClaimType.UserType, ClaimValue.Member);
                });
                options.AddPolicy(Policies.Admin, policy =>
                {
                    policy.RequireClaim(ClaimType.UserType, ClaimValue.Admin);
                });
            });
            services.AddConfiguration<IDbConfig, DbConfig>(Configuration);
            services.AddConfiguration<IAuthConfig, AuthConfig>(Configuration);
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IClaimService, ClaimService>();
            services.AddTransient<ICryptographyService, CryptographyService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IClaimRepository, ClaimRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private TokenValidationParameters GetTokenValidationParameters() =>
            new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSigningKey(),
                ClockSkew = TimeSpan.Zero
            };

        private SymmetricSecurityKey GetSigningKey()
        {
            var authConfig = Configuration
                .GetSection(nameof(AuthConfig))
                .Get<AuthConfig>();

            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authConfig.Secret));
        }
    }
}
