using Mafiator.Game.Platforms.Android.Services.Toasts;
using Mafiator.Game.Services;

namespace Mafiator.Game.Platforms.Android;

public class ToastService : IToastService
{
    public void ShortAlert(string message, MessageType type)
    {
        switch (type)
        {
            case MessageType.None:
                Toasty.Normal(Platform.AppContext, message).Show();
                break;
            case MessageType.Success:
                Toasty.Success(Platform.AppContext, message).Show();
                break;
            case MessageType.Error:
                Toasty.Error(Platform.AppContext, message).Show();
                break;
            case MessageType.Warning:
                Toasty.Warning(Platform.AppContext, message).Show();
                break;
            case MessageType.Info:
                Toasty.Info(Platform.AppContext, message).Show();
                break;
        }
    }
}