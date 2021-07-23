using Api.Core.Commands;
using Api.Core.Data;
using Api.Core.Middleware;
using Logging.Extensions;
using Logging.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PaymentGateway.Api.Core.Commands;
using PaymentGateway.Api.Core.Configurations;
using PaymentGateway.Api.Core.Data.Model;
using PaymentGateway.Api.Core.Data.Service;
using PaymentGateway.Api.Core.Service;
using PaymentGateway.Api.Core.Stimulator;
using System;
using System.IO;
using System.Reflection;


namespace PaymentGateway.Api
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Start up class
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //Commands
            services.AddCommandsFromAssembly();
            services.AddScoped<ProcessPaymentsCommand>();
            services.AddScoped<HealthCommand>();
            services.AddScoped<FetchPaymentsByIdCommand>();
            
            //ElasticSearch
            services.AddElasticSearch(Configuration);

            //Service
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IMockedBankStimulator, MockedBankStimulator>();

            //DataService
            services.AddScoped<IPaymentRecordDataService, PaymentRecordDataService>();

            //Logging
            services.AddConsoleLogging();
            services.AddScoped<IContextLogModel, ApiLogContext>();

            //Automapper
            services.AddAutoMapper(Assembly.GetAssembly(typeof(PaymentRecord)));

            //swagger
            AddSwagger(services);
        }

        
        ///This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Gateway API (v1.0.0)"));
        }

        /// <summary>
        /// Adds Swagger documentation for the web api 
        /// </summary>
        /// <param name="services"></param>
        public void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Payment Gateway API",
                    Version = "v1.0.0",
                    Description =
                        "process payment",
                    Contact = new OpenApiContact()
                    {
                        Name = "Sreejan Kumar",
                        Url = new Uri("https://github.com/sreejankumar")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "License",
                        Url = new Uri("https://github.com/sreejankumar/Api.Core/blob/main/LICENSE")
                    }
                });

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                    "PaymentGateway.Api.xml"));
            });
        }
    }
}
