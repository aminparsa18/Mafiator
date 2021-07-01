using System.Collections.Generic;
using MafiatorApp.Models;
using Xamarin.Forms;

namespace MafiatorApp.Services
{
	public interface IOverlayService
    {
		void AddOverlay(Dictionary<View,string>targets, ShowCaseConfig config);
		//void HideOverlay();
	}
}
