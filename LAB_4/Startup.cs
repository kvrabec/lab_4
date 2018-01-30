using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using LAB4.MODEL.Entities;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace LAB_4
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = Configuration["Auth:ValidIssuer"],
                    ValidAudience = Configuration["Auth:ValidAudience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(Configuration["Auth:JwtSecurityKey"]))
                };
            });

            services.AddMvc()
                    .AddJsonOptions(options =>
                    options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<StoreDbContext>(options =>
                    options.UseSqlServer(Configuration["connStr"]));

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;                       
                options.AssumeDefaultVersionWhenUnspecified = true;     
                options.DefaultApiVersion = new ApiVersion(1, 1);       
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");  
            });

            //services.UseApiVersioning();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1.0", new Info { Title = "Customer API", Version = "v1.0" }); 
                options.SwaggerDoc("v1.1", new Info { Title = "Customer API", Version = "v1.1" });

                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var versions = apiDesc.ControllerAttributes()
                                        .OfType<ApiVersionAttribute>()
                                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v.ToString()}" == docName);
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Customer API V1.0");
                c.SwaggerEndpoint("/swagger/v1.1/swagger.json", "Customer API V1.1");
            });

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
