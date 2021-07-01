using System.Collections.Generic;
using MafiatorApp.Models;
using MafiatorApp.Services;
using Xamarin.Forms;

namespace MafiatorApp.UserControls
{
	public class Overlay
    {
		public Overlay()
		{
			OverlayService = DependencyService.Get<IOverlayService>();
		}

		private IOverlayService OverlayService { get; }

		public void Show(Dictionary<View,string> targets, ShowCaseConfig config)
		{
			OverlayService.AddOverlay(targets, config);
		}

		
	}
}
