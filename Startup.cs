using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PostgresApplication.Helper;
using PostgresApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgresApplication
{
    public class Startup
    {

        // Super Secret Key

        private static readonly string key = "qwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnm";
        private static readonly SymmetricSecurityKey SymmetricSecuritySingingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Below function will get the Connection String from the System ENV variable Named "DATABASE_CRED". if not available then will get the 
        // connection string from configuration.
        private string GetConnectionStringForDB()
        {
            string ENV_DATA = Environment.GetEnvironmentVariable("DATABASE_CRED");
            if (ENV_DATA == "")
            {
                return Configuration.GetConnectionString("Default");
            }
            return ENV_DATA;

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();



            services.AddDbContext<BookContext>(optionsBuilder => optionsBuilder.UseNpgsql(GetConnectionStringForDB()));
            services.AddScoped<JwtService>();

            services.AddAuthentication(options =>
                {
                    options.DefaultChallengeScheme = "JwtBearer";
                    options.DefaultAuthenticateScheme = "JwtBearer";

                }).AddJwtBearer("JwtBearer", jwtOptions =>
                {
                    jwtOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = SymmetricSecuritySingingKey,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = "https://localhost:44372",
                        ValidAudience = "https://localhost:44372",
                        ValidateLifetime = true
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PostgresApplication", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PostgresApplication v1"));
            }


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
