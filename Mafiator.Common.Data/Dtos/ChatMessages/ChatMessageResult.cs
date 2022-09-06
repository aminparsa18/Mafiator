using Mafiator.Common.Data.Enums;

namespace Mafiator.Common.Data.Dtos.ChatMessages;

/// <summary>
/// Chat message dto.
/// </summary>
[MessagePackObject()]
public class ChatMessageResult
{
    /// <summary>
    /// Content.
    /// </summary>
    [Key(0)]
    public string Content { get; set; }

    /// <summary>
    /// User image.
    /// </summary>
    [Key(1)]
    public string Image { get; set; }

    /// <summary>
    /// Display name.
    /// </summary>
    [Key(2)]
    public string DisplayName { get; set; }

    /// <summary>
    /// Game message type.
    /// </summary>
    [Key(3)]
    public GameMessageType Type { get; set; }
}