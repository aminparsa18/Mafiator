namespace Mafiator.Repository;

/// <summary>
/// Unit of work.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Repository provides methods to retrieve/handle avatar data.
    /// </summary>
    IAvatarRepository Avatar { get; }

    /// <summary>
    /// Repository provides methods to retrieve/handle event join data.
    /// </summary>
    IEventJoinRepository EventJoin { get; }

    /// <summary>
    /// Repository provides methods to retrieve/handle event data.
    /// </summary>
    IEventRepository Event { get;  }

    /// <summary>
    /// Repository provides methods to retrieve/handle chat message data.
    /// </summary>
    IChatMessageRepository ChatMessage { get; }

    /// <summary>
    /// Repository provides methods to retrieve/handle game event data.
    /// </summary>
    IGameEventRepository GameEvent { get; }

    /// <summary>
    /// Repository provides methods to retrieve/handle game member data.
    /// </summary>
    IGameMemberRepository GameMember { get; }

    /// <summary>
    /// Repository provides methods to retrieve/handle game message data.
    /// </summary>
    IGameMessageRepository GameMessage { get; }

    /// <summary>
    /// Repository provides methods to retrieve/handle game data.
    /// </summary>
    IGameRepository Game { get;  }

    /// <summary>
    /// Repository provides methods to retrieve/handle game violation data.
    /// </summary>
    IGameViolationReportRepository GameViolation { get; }

    /// <summary>
    /// Repository provides methods to retrieve/handle gem data.
    /// </summary>
    IGemRepository Gem { get; }

    /// <summary>
    /// Repository provides methods to retrieve/handle reaction data.
    /// </summary>
    IReactionRepository Reaction { get; }

    /// <summary>
    /// Repository provides methods to retrieve/handle refresh token data.
    /// </summary>
    IRefreshTokenRepository RefreshToken { get; }

    /// <summary>
    /// Repository provides methods to retrieve/handle room member data.
    /// </summary>
    IRoomMemberRepository RoomMember { get; }

    /// <summary>
    /// Repository provides methods to retrieve/handle room data.
    /// </summary>
    IRoomRepository Room { get; }

    /// <summary>
    /// Repository provides methods to retrieve/handle vote data.
    /// </summary>
    IVoteRepository Vote { get; }

    /// <summary>
    /// Commit changes to database.
    /// </summary>
    Task<int> Commit();
}