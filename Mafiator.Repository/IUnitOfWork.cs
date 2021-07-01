using System.Threading.Tasks;
using Mafiator.Repository.Contracts;

namespace Mafiator.Repository
{
    public interface IUnitOfWork
    {
        IAvatarRepository Avatar{ get; }
        IEventJoinRepository EventJoin { get; }
        IEventRepository Event { get;  }
        IGameEventRepository GameEvent { get; }
        IGameMemberRepository GameMember { get; }
        IGameMessageRepository GameMessage { get; }
        IGameRepository Game { get;  }
        IGameViolationReportRepository GameViolation { get; }
        IGemRepository Gem{ get; }
        IRefreshTokenRepository RefreshToken { get; }
        IRoomMemberRepository RoomMember { get; }
        IRoomRepository Room { get; }
        IVoteRepository Vote{ get; }
        Task<int> Commit();
    }
}