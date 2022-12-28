using Azure.ResourceManager.Media.Models;
using Mafiator.Common.Server.Media;
using Mafiator.Service.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mafiator.Service.Services;

public class LiveEventManager : ILiveEventManager
{
    private readonly MediaServiceCredential _credential;
    private readonly IMemoryCache _cache;

    public LiveEventManager(IOptions<MediaServiceCredential> credential, IMemoryCache cache)
    {
        _credential = credential.Value;
        _cache = cache;
    }
    public static readonly string TokenType = "Bearer";

    /// <summary>
    /// Creates the AzureMediaServicesClient object based on the credentials
    /// supplied in local configuration file.
    /// </summary>
    /// <returns>A task.</returns>
    // <CreateMediaServicesClientAsync>
    //public async Task<IAzureMediaServicesClient> CreateMediaServicesClientAsync()
    //{
    //    var credentials = await GetCredentialsAsync();
    //    return new AzureMediaServicesClient(_credential.ArmEndpoint, credentials)
    //    {
    //        SubscriptionId = _credential.SubscriptionId,
    //    };
    //}
    private async Task<ServiceClientCredentials> GetCredentialsAsync()
    {
        // Use ConfidentialClientApplicationBuilder.AcquireTokenForClient to get a token using a service principal with symmetric key

        var scopes = new[] { _credential.ArmAadAudience + "/.default" };

        var app = ConfidentialClientApplicationBuilder.Create(_credential.AadClientId)
            .WithClientSecret(_credential.AadSecret)
            .WithAuthority(AzureCloudInstance.AzurePublic, _credential.AadTenantId)
            .Build();

        var authResult = await app.AcquireTokenForClient(scopes)
            .ExecuteAsync()
            .ConfigureAwait(false);

        return new TokenCredentials(authResult.AccessToken, TokenType);
    }

    public async Task<Tuple<string, string>> CreateLiveEvent(string liveEventName)
    {
        //var client = await CreateMediaServicesClientAsync();
        //var mediaService = await client.Mediaservices.GetAsync(_credential.ResourceGroup, _credential.AccountName);
        //IPRange allAllowIpRange = new IPRange();
        //LiveEventInputAccessControl liveEventInputAccess = new()
        //{
        //    Ip = new IPAccessControl(
        //        new[]
        //        {
        //            allAllowIpRange
        //        }
        //    )
        //};

        //LiveEventPreview liveEventPreview = new()
        //{
        //    AccessControl = new LiveEventPreviewAccessControl(
        //        new IPAccessControl(
        //            new[]
        //            {
        //                allAllowIpRange
        //            }
        //        )
        //    )
        //};

        //LiveEvent liveEvent = new(
        //    mediaService.Location,
        //    description: "Game Live Event",
        //    useStaticHostname: true,
        //    input: new LiveEventInput(
        //        LiveEventInputProtocol
        //            .RTMP,
        //        accessToken:
        //        "acf7b6ef-8a37-425f-b8fc-51c2d6a5a86a",
        //        accessControl: liveEventInputAccess, 
        //        keyFrameIntervalDuration: "PT2S"
        //    ),
        //    encoding: new LiveEventEncoding(
        //        LiveEventEncodingType.None 
        //    ),
        //    preview: liveEventPreview,
        //    streamOptions: new List<StreamOptionsFlag?>()
        //    {
        //        StreamOptionsFlag.LowLatency
        //    }
        //);

        Console.WriteLine("Creating the LiveEvent, please be patient as this can take time to complete async.");
        Console.WriteLine(
            "Live Event creation is an async operation in Azure and timing can depend on resources available.");

        //var watch = Stopwatch.StartNew();
        //liveEvent = await client.LiveEvents.CreateAsync(
        //    _credential.ResourceGroup,
        //    _credential.AccountName,
        //    liveEventName,
        //    liveEvent,
        //    autoStart: false);
        //watch.Stop();
        //var elapsedTime =
        //    $":{watch.Elapsed.Seconds:00}.{watch.Elapsed.Milliseconds / 10:00}";
        //Console.WriteLine($"Create Live Event run time : {elapsedTime}");

        //#region CreateAsset
        //// Create an Asset for the LiveOutput to use. Think of this as the "tape" that will be recorded to. 
        //// The asset entity points to a folder/container in your Azure Storage account. 
        //Console.WriteLine($"Creating an asset named mftorlive");
        //Console.WriteLine();
        //Asset asset = await client.Assets.CreateOrUpdateAsync(credential.ResourceGroup, credential.AccountName, "mftorlive", new Asset());
        //#endregion

        #region CreateLiveOutput
        var manifestName = "output";
        Console.WriteLine($"Creating a live output named mftorliveoutput");
        Console.WriteLine();

        //watch = Stopwatch.StartNew();
        //// See the REST API for details on each of the settings on Live Output
        //// https://docs.microsoft.com/rest/api/media/liveoutputs/create
        //LiveOutput liveOutput = new(
        //    "mftorlive",
        //    manifestName: manifestName, 
        //    archiveWindowLength: TimeSpan.FromHours(2)
        //);
        //liveOutput = await client.LiveOutputs.CreateAsync(
        //    _credential.ResourceGroup,
        //    _credential.AccountName,
        //    liveEventName,
        //    "mftorliveoutput",
        //    liveOutput);
        //elapsedTime = $":{watch.Elapsed.Seconds:00}.{watch.Elapsed.Milliseconds / 10:00}";
        //Console.WriteLine($"Create Live Output run time : {elapsedTime}");
        Console.WriteLine();
        #endregion


        Console.WriteLine("Starting the Live Event now... please stand by as this can take time...");
        //watch = Stopwatch.StartNew();
        //// Start the Live Event - this will take some time...
        //await client.LiveEvents.StartAsync(_credential.ResourceGroup, _credential.AccountName, liveEventName);
        //elapsedTime = $":{watch.Elapsed.Seconds:00}.{watch.Elapsed.Milliseconds / 10:00}";
        //Console.WriteLine($"Start Live Event run time : {elapsedTime}");
        //Console.WriteLine();

        //// Refresh the liveEvent object's settings after starting it...
        //liveEvent = await client.LiveEvents.GetAsync(_credential.ResourceGroup, _credential.AccountName, liveEventName);
        //var ingestUrl = liveEvent.Input.Endpoints.First().Url;
        //var previewUrl = liveEvent.Preview.Endpoints.First().Url;
        //_cache.SetCache(ingestUrl,"Ingest-"+liveEventName);
        //_cache.SetCache(previewUrl,"Preview-"+liveEventName);
        return Tuple.Create("", "");// ingestUrl,previewUrl);
    }
}