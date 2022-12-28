namespace Mafiator.Game.Models;

public enum MenuItemType
{
    Profile,
    Wallet,
    Instruction,
    Events,
    Invitation,
    Rule,
    About
}

public sealed class HomeMenuItem
{
    public MenuItemType Id { get; set; }

    public string Title { get; set; }

    public string ImageUrl { get; set; }
}