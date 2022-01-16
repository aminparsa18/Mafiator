using MafiatorApp.Models;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MafiatorApp.Services
{
    public interface IOverlayService
    {
		void AddOverlay(Dictionary<View,string>targets, ShowCaseConfig config);
		//void HideOverlay();
	}
}
