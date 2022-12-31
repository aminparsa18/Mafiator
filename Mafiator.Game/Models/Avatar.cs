using CommunityToolkit.Mvvm.ComponentModel;

namespace Mafiator.Game.Models;

public sealed partial class Avatar : ObservableObject
{
    public string Name { get; set; }

    [ObservableProperty]
    private double _scale = 1;
}