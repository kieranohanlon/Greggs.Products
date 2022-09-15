using FluentValidation;
using FluentValidation.AspNetCore;
using Greggs.Products.Application.QueryHandlers;
using Greggs.Products.Application.Validators;
using Greggs.Products.Infrastructure;
using Greggs.Products.Models;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Greggs.Products.Api;

public class Startup
{
    //TODO - This is not a big issue, but consider if we want to remove the startup and just have a Program.cs as is the case with default ASP.NET Core 6 APIs, since this was from v5.
    public void ConfigureServices(IServiceCollection services)
    {
        //Will add all validators in the project
        services.AddControllers();

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<GetProductsRequestValidator>();


        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Greggs Products API",
                Description = "An ASP.NET Core Web API for Greggs Products"
            });

            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        // Add Api Versioning
        services.AddApiVersioning(o =>
        {
            o.ReportApiVersions = true;
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);
        });

        //MediatR
        services.AddMediatR(typeof(GetProductsQueryHandler).Assembly);

        services.AddScoped<IDataAccess<Product>, ProductAccess>();
        services.AddScoped<ICurrencyAccess, CurrencyAccess>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Greggs Products API V1"); });

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        //TODO - Add in exception handling
        //app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}