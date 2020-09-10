using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using srcds_control_api.Models;
using srcds_control_api.Services;
using srcds_control_api.Utilities;
using Swashbuckle.AspNetCore.Swagger;

namespace srcds_control_api
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
            Console.WriteLine("Configurating Services...");
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var corsSection = Configuration.GetSection("Cors");
            var jwtSection = Configuration.GetSection("JWT");

            services.Configure<JwtSettings>(jwtSection);
            services.Configure<CorsSettings>(corsSection);

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISrcdsService, SrcdsService>();
            services.AddScoped<IRankService, RankService>();

            Console.WriteLine($"Config => {Configuration.ToString()}");

            // Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "srcds-control-api",
                    Description = "SnipeTrain.tf Backend API",
                    Contact = new Contact
                    {
                        Name = "Zeptile",
                        Email = "zeptile@xfault.net",
                        Url = "https://snipetrain.tf"
                    }
                });
            });

            // CORS

            var corsConfig = corsSection.Get<CorsSettings>();
            services.AddCors(options =>
            {
                options.AddPolicy("Cors-Policy",
                builder =>
                {
                    builder.WithOrigins(corsConfig.AllowedOrigins);
                });
            });


            // JWT

            var jwtConfig = jwtSection.Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "srcds-control-api");
                });
            }
            else
            {
                app.UseHsts();
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

            env.ConfigureNLog("nlog.config");

            app.UseAuthentication();

            //app.UseHttpsRedirection();
            app.UseMvc();

            app.UseCors("Cors-Policy");
        }
    }
}
