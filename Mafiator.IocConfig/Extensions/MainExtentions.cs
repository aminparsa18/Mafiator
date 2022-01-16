using Mafiator.Common.Api;
using Mafiator.Common.Extensions;
using Mafiator.Common.SiteSetting;
using Mafiator.Data;
using Mafiator.IocConfig.Formatters;
using Mafiator.IocConfig.Hubs;
using Mafiator.Repository;
using Mafiator.Service.Contracts;
using Mafiator.Service.Contracts.Impl;
using Mafiator.Service.Models;
using MessagePack;
using MessagePack.AspNetCoreMvcFormatter;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using System.Net;

namespace Mafiator.IocConfig.Extensions
{
    public static class MainExtentions
    {
        public static IServiceCollection AddMainServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAntiforgery();
            var resolver =StandardResolver.Instance;
            var messagePackOption = MessagePackSerializerOptions.Standard.WithResolver(resolver);
            services.AddControllers(option =>
            {
              option.OutputFormatters.Add(new MessagePackOutputFormatter(messagePackOption)); 
              option.InputFormatters.Add(new MessagePackInputFormatter(messagePackOption));
            }).AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ISmsSender, TwilioSmsSender>();
            services.AddScoped<ILiveEventManager, LiveEventManager>();
            services.AddSingleton<INotificationService, NotificationHubService>();
            services.AddDistributedMemoryCache();
            services.AddScoped<IMemoryCache, MemoryCache>();
            services.AddOptions<NotificationHubOptions>()
                .Configure(configuration.GetSection("NotificationHub").Bind)
                .ValidateDataAnnotations();
            services.AddOptions<MediaServiceCredential>()
                .Configure(configuration.GetSection("MediaService").Bind)
                .ValidateDataAnnotations();
            services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapping>());
            services.AddSwaggerDocument(setting =>
            {
                setting.SchemaProcessors.Add(new MessagePackAttributesSchemaProcessor());
                setting.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Mafiator API";
                    document.Info.Description = "legendary online game forever";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Amin Parsa",
                        Email = "aminparsa18@gmail.com",
                        Url = "https://aminparsa.me"
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = "https://opensource.org/licenses/MIT"
                    };
                };
            });
            services.AddSignalR().AddMessagePackProtocol(o=>o.SerializerOptions=messagePackOption)
                .AddAzureSignalR("Endpoint=https://mftor.service.signalr.net;AccessKey=/bXupX8SacE1iztiuK/ZqxdZopEVKtaYTUVb3xUjs9U=;Version=1.0;");
         
            return services;
        }

        public static void UseMainMiddlewares(this IApplicationBuilder app)
        {
            app.UseHsts();
            app.UseStatusCodePages(async context =>
            {
                context.HttpContext.Response.ContentType = "application/x-msgpack";
                if (context.HttpContext.Response.StatusCode == 401)
                {
                    await context.HttpContext.Response.WriteAsync(new ApiResult()
                    {
                        Errors = new[] {"Token not validated"},
                        StatusCode = ApiResultStatusCode.UnAuthorized
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
            app.UseHttpsRedirection();
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
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}