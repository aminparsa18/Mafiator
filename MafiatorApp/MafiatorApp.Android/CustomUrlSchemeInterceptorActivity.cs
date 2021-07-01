using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
   
namespace MafiatorApp.Droid
{
	[Activity(Label = "CustomUrlSchemeInterceptorActivity",LaunchMode =LaunchMode.SingleTop, NoHistory = true)]
    
	public class CustomUrlSchemeInterceptorActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			// Convert Android.Net.Url to Uri
			var uri = new Uri(Intent.Data.ToString());
			// Load redirectUrl page
			//AuthenticationState.Authenticator.OnPageLoading(uri);
			Finish();
		}
	}
}
