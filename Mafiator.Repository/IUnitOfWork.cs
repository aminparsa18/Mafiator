using Mafiator.Repository.Contracts;
using System.Threading.Tasks;

namespace Mafiator.Repository
{
    public interface IUnitOfWork
    {
        IAvatarRepository Avatar{ get; }
        IEventJoinRepository EventJoin { get; }
        IEventRepository Event { get;  }
        IChatMessageRepository ChatMessage{ get; }
        IGameEventRepository GameEvent { get; }
        IGameMemberRepository GameMember { get; }
        IGameMessageRepository GameMessage { get; }
        IGameRepository Game { get;  }
        IGameViolationReportRepository GameViolation { get; }
        IGemRepository Gem{ get; }
        IReactionRepository Reaction{ get; }
        IRefreshTokenRepository RefreshToken { get; }
        IRoomMemberRepository RoomMember { get; }
        IRoomRepository Room { get; }
        IVoteRepository Vote{ get; }
        Task<int> Commit();
    }
}