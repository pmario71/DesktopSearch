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
            // Setup options with DI
            services.AddOptions();

            // Configure MyOptions using config by installing Microsoft.Extensions.Options.ConfigurationExtensions
            //services.Configure<ElasticSearchConfig>(_configuration.GetSection("ElasticSearch"));
            //services.Configure<ElasticSearchConfig>(t => t. this.Configuration.GetSection("ElasticSearch"));

            services.AddMvcCore();
            //services.AddSwaggerGen();
            //services.ConfigureSwaggerGen(options =>
            //{
            //    options.SingleApiVersion(new Swashbuckle.Swagger.Model.Info
            //    {
            //        Version = "v1",
            //        Title = "Desktop Search API",
            //        Description = "A simple api to search using Elasticsearch",
            //        TermsOfService = "None"
            //    });
            //    //options.IncludeXmlComments(pathToDoc);
            //    options.DescribeAllEnumsAsStrings();
            //});

            //todo: switch to options later
            services.AddSingleton<ElasticSearchConfig>(p => new ElasticSearchConfig { Uri="localhost:9200" });
            services.AddTransient<SearchService>();
            
            // Add our repository type
            services.AddSingleton<TodoApi.Models.ITodoRepository, TodoApi.Models.TodoRepository>();



        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory log)
        {
            log.AddConsole(_configuration.GetSection("Logging"));
            log.AddDebug();
            //app.UseMvc();
            app.UseMvc();

            //app.UseSwagger();
            //app.UseSwaggerUi();

            //var routeBuilder = new RouteBuilder(app);
            ////{ controller}/{ action}/{ id ?}
            //routeBuilder.MapRoute("", "{controller}/");

            if (env.IsDevelopment())
            {
            }
            //app.Run(context =>
            //{
            //    return context.Response.WriteAsync("Hello from ASP.NET Core!");
            //});
        }
    }
}
