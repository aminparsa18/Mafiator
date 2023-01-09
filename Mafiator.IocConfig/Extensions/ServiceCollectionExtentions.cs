using FluentValidation;
using Hangfire;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Extensions;
using Mafiator.Common.Server.Media;
using Mafiator.Data;
using Mafiator.Repository;
using Mafiator.Repository.Cache;
using Mafiator.Service.Contracts;
using Mafiator.Service.Contracts.Avatars;
using Mafiator.Service.Hubs;
using Mafiator.Service.Models;
using Mafiator.Service.Services;
using Mafiator.Service.Validations.Users;
using MemoryPack.AspNetCoreMvcFormatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RepoDb;
using Serilog;
using System.Collections.Generic;
using System.Data;
using System.Net;

namespace Mafiator.IocConfig.Extensions;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection ConfigureDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
    {
        Barrel.ApplicationId = "MafiatorAPi";

       //  services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("HangfireContext")));
       // services.AddHangfireServer();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MafiatorContext")).EnableSensitiveDataLogging());
        RepoDb.GlobalConfiguration.Setup().UseSqlServer();
        services.AddTransient<IDbConnection>(sp => new SqlConnection(configuration.GetConnectionString("MafiatorContext")));
        return services;
    }

    public static IServiceCollection ConfigureController(this IServiceCollection services)
    {
        services.AddAntiforgery();

        services.AddControllers(options =>
        {
            options.InputFormatters.Insert(0, new MemoryPackInputFormatter());
            // If checkContentType: true then can output multiple format(JSON/MemoryPack, etc...). default is false.
            options.OutputFormatters.Insert(0, new MemoryPackOutputFormatter(checkContentType: false));
        }).AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);

        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
        });
        return services;
    }

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
            options.IgnoreObsoleteActions();
            options.IgnoreObsoleteProperties();
            options.EnableAnnotations();

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Mafiator API",
                Description = "legendary online game forever",
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new System.Uri("https://opensource.org/licenses/MIT")
                },
                Contact = new OpenApiContact
                {
                    Name = "Amin Parsa",
                    Email = "aminparsa18@gmail.com",
                    Url = new System.Uri("https://aminparsa.me")
                }
            });
        });
        return services;
    }

    public static IServiceCollection ConfigureCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<UserLoginRequestValidator>();
        services.Scan(scan => scan
        .FromAssemblyOf<IUnitOfWork>()
        .AddClasses(classes => classes.AssignableTo<IUnitOfWork>()).AsMatchingInterface().WithScopedLifetime()
        .FromAssemblyOf<IAvatarService>()
        .AddClasses().AsImplementedInterfaces().WithScopedLifetime());
        services.AddSingleton<INotificationService, NotificationHubService>();
        services.AddDistributedMemoryCache();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetSection("Redis").GetValue<string>("Connection");
            options.InstanceName = configuration.GetSection("Redis").GetValue<string>("InstanceName");
        });
        services.AddOptions<NotificationHubOptions>()
            .Configure(configuration.GetSection("NotificationHub").Bind)
            .ValidateDataAnnotations();
        services.AddOptions<MediaServiceCredential>()
            .Configure(configuration.GetSection("MediaService").Bind)
            .ValidateDataAnnotations();
        services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapping>());

        services.AddSignalR().AddMessagePackProtocol()
            .AddAzureSignalR("Endpoint=https://mftor.service.signalr.net;AccessKey=/bXupX8SacE1iztiuK/ZqxdZopEVKtaYTUVb3xUjs9U=;Version=1.0;");
     
        return services;
    }

    public static void ConfigureCustomIdentityServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        services.AddIdentityWithOptions(configuration, webHostEnvironment);
        services.AddScoped<ApplicationIdentityErrorDescriber>();
    }

    public static void UseMainMiddlewares(this IApplicationBuilder app)
    {
        app.UseHsts();
        app.UseStatusCodePages(async context =>
        {
            context.HttpContext.Response.ContentType = "application/x-msgpack";
            if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                await context.HttpContext.Response.WriteAsync(new ApiResult()
                {
                    Errors = new[] {"Token not validated"},
                    StatusCode = ApiResultStatusCode.Unauthorized
                }.ToString());
            }
            else
            {
                await context.HttpContext.Response.WriteAsync(new ApiResult()
                {
                    Errors = new[] {"Internal Error"},
                    StatusCode = ApiResultStatusCode.ServerError
                }.ToString());
            }
        });
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/x-msgpack";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var err = contextFeature.Error;
                    await context.Response.WriteAsync(contextFeature.Error.DetailedMessage());
                }
            });
        });
        //app.UseHttpsRedirection();
        app.UseSerilogRequestLogging();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<ChatHub>("/chathub");
            endpoints.MapHub<GameHub>("/gamehub");
            endpoints.MapHub<RoomHub>("/roomhub");
        });
        app.CallDbInitializer();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}