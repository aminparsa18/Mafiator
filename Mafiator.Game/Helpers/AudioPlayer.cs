using Plugin.Maui.Audio;

namespace Mafiator.Game.Helpers;

public class AudioPlayer : IDisposable
{
    public static async Task Play()
    {
        var player = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("mafia1.mp3"));
        player.Play();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}