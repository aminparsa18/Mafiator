using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using MafiatorApp.Cache;
using MafiatorApp.Extentions;

namespace MafiatorApp
{
   public class BaseHttpClient
    {
        private static HttpClient _instance;
        private static Stopwatch stopwatch;
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
        public static Stopwatch Stopwatch
        {
            get
            {
                lock (Padlock)
                {
                    return stopwatch ??= new Stopwatch();
                }
            }
        }
        private static HttpClient CreateInstance()
        {
            var client = new HttpClient {Timeout = TimeSpan.FromSeconds(15)};
            if (Barrel.Current.Exists("Token"))
            {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<string>("Token"));
            }

            client.AddDefaultMessagePackAcceptHeader();
            return new HttpClient();
        }
    }
}
