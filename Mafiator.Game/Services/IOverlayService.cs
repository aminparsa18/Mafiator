using Mafiator.Game.Models;

namespace Mafiator.Game.Services;

public interface IOverlayService
{
		void AddOverlay(Dictionary<View,string>targets);
		//void HideOverlay();
	}
