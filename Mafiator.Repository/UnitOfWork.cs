using System.Data;
using System.Threading.Tasks;
using Mafiator.Data;
using Mafiator.Repository.Contracts;
using Mafiator.Repository.Repositories;

namespace Mafiator.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        private readonly IDbConnection connection;
        private IAvatarRepository avatar;
        private IEventJoinRepository eventJoin;
        private IEventRepository eventt;
        private IChatMessageRepository chatMessage;
        private IGameEventRepository gameEvent;
        private IGameMemberRepository gameMember;
        private IGameMessageRepository gameMessage;
        private IGameRepository game;
        private IGameViolationReportRepository gameViolation;
        private IGemRepository gem;
        private IReactionRepository reaction;
        private IRefreshTokenRepository refreshToken;
        private IRoomMemberRepository roomMember;
        private IRoomRepository room;
        private IVoteRepository vote;

        public UnitOfWork(ApplicationDbContext context, IDbConnection connection)
        {
            this.context = context;
            this.connection = connection;
        }


        public IAvatarRepository Avatar => avatar ?? new AvatarRepository(context,connection);
        public IEventJoinRepository EventJoin => eventJoin ??= new EventJoinRepository(context,connection);
        public IEventRepository Event => eventt ??= new EventRepository(context,connection);
        public IChatMessageRepository ChatMessage => chatMessage ??= new ChatMessageRepository(context, connection);
        public IGameEventRepository GameEvent => gameEvent ??= new GameEventRepository(context,connection);
        public IGameMemberRepository GameMember => gameMember ??= new GameMemberRepository(context,connection);
        public IGameMessageRepository GameMessage => gameMessage ??= new GameMessageRepository(context,connection);
        public IGameRepository Game => game ?? new GameRepository(context,connection);
        public IGameViolationReportRepository GameViolation =>
            gameViolation ??= new GameViolationReportRepository(context,connection);
        public IGemRepository Gem => gem ??= new GemRepository(context, connection);
        public IReactionRepository Reaction => reaction ??= new ReactionRepository(context, connection);
        public IRefreshTokenRepository RefreshToken => refreshToken ??= new RefreshTokenRepository(context, connection);
        public IRoomMemberRepository RoomMember => roomMember ?? new RoomMemberRepository(context,connection);
        public IRoomRepository Room => room ?? new RoomRepository(context,connection);
        public IVoteRepository Vote => vote ?? new VoteRepository(context, connection);

        public Task<int> Commit()
        {
            return context.SaveChangesAsync();
        }
    }
}
