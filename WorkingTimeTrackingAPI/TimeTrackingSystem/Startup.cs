using System;
using System.IO;
using System.Net.Http.Formatting;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using TimeTrackingSystem.Api.Core;
using TimeTrackingSystem.Data.Access.Context;
using TimeTrackingSystem.Data.Access.DAL;
using TimeTrackingSystem.Extensions;

namespace TimeTrackingSystem
{
    public class Startup
    {
        private readonly string _apiName = "Time tracking API";
        private readonly string _apiVersion = "V1";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddEntityFrameworkSqlite().AddDbContext<TimeTrackingSystemDbContext>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info() { Title = _apiName, Version = _apiVersion });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddMvc() .AddJsonOptions(options => 
                {
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm";
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<ITimeTrackingSystemRepository, TimeTrackingSystemRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //можно включить логирование в файл
             loggerFactory.AddFile("Logs/logfile-{Date}.txt");


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{_apiName} {_apiVersion}");
                
            });


            app.UseMiddleware<ExceptionMiddleware>();

            app.UseMvc();
        }
    }
}
