namespace Mafiator.Game.Services;

public interface IToastService
{
    void ShortAlert(string message, MessageType type);
}

public enum MessageType
{
    None,
    Success,
    Warning,
    Error,
    Info
}