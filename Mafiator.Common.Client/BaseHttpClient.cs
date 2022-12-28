using Mafiator.Common.Client.Cache;
using Mafiator.Common.Client.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Mafiator.Common.Client;

public class BaseHttpClient
{
    private static HttpClient _instance;
    private static readonly object Padlock = new object();

    private BaseHttpClient()
    {
    }

    public static HttpClient Instance
    {
        get
        {
            lock (Padlock)
            {
                return _instance ??= CreateInstance();
            }
        }
    }

    private static HttpClient CreateInstance()
    {
        var client = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
        if (Barrel.Current.Exists("Token"))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<string>("Token"));
        }

        client.AddDefaultMessagePackAcceptHeader();
        return new HttpClient();
    }
}
