using Mafiator.Common.Data.Enums;
using MemoryPack;

namespace Mafiator.Common.Data.Dtos.ChatMessages;

/// <summary>
/// Chat message dto.
/// </summary>
[MemoryPackable]
public sealed partial class ChatMessageResult
{
    /// <summary>
    /// Content.
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// User image.
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// Display name.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Game message type.
    /// </summary>
    public GameMessageType Type { get; set; }
}