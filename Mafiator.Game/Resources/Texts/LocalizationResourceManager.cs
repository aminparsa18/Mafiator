using System.ComponentModel;
using System.Globalization;

namespace Mafiator.Game.Resources.Texts;

public class LocalizationResourceManager : INotifyPropertyChanged
{
    private LocalizationResourceManager()
    {
        AppResources.Culture = CultureInfo.CurrentCulture;
    }

    public static LocalizationResourceManager Instance { get; } = new();

    public string this[string resourceKey] =>
        AppResources.ResourceManager.GetString(resourceKey, AppResources.Culture) ?? throw new ArgumentNullException("resource not translated");

    public event PropertyChangedEventHandler? PropertyChanged;

    public void SetCulture(CultureInfo culture)
    {
        AppResources.Culture = culture;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    }
}