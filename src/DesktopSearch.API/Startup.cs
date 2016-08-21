using DesktopSearch.Core.Configuration;
using DesktopSearch.Core.ElasticSearch;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopSearch.API
{
    public class Startup
    {
        private IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SingleApiVersion(new Swashbuckle.Swagger.Model.Info
                {
                    Title = "Values API",
                    Version = "v1",
                    Description = "An API API With Swagger for RC2",
                    TermsOfService = "None"
                });
            });

            // Setup options with DI
            services.AddOptions();

            // Configure MyOptions using config by installing Microsoft.Extensions.Options.ConfigurationExtensions
            //services.Configure<ElasticSearchConfig>(_configuration.GetSection("ElasticSearch"));
            //services.Configure<ElasticSearchConfig>(t => t. this.Configuration.GetSection("ElasticSearch"));

            services.AddMvc();

            //todo: switch to options later
            services.AddSingleton<ElasticSearchConfig>(p => new ElasticSearchConfig());
            services.AddTransient<SearchService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory log)
        {
            log.AddConsole(_configuration.GetSection("Logging"));
            log.AddDebug();

            app.Use(async (context, next) =>
            {
                Console.WriteLine($"Handling request: {context.Request}");
                await next.Invoke();
                Console.WriteLine("Finished handling request.");
            });

            app.UseMvc();

            if (env.IsDevelopment())
            {
            }

            app.UseSwagger();
            app.UseSwaggerUi();
            //app.Run(context =>
            //{
            //    return context.Response.WriteAsync("Hello from ASP.NET Core!");
            //});
        }
    }
}
