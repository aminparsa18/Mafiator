using Mafiator.Data;
using Mafiator.Repository.Contracts;
using Mafiator.Repository.Repositories;
using System.Data;
using System.Threading.Tasks;

namespace Mafiator.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbConnection _connection;
        private IAvatarRepository _avatar;
        private IEventJoinRepository _eventJoin;
        private IEventRepository _event;
        private IChatMessageRepository _chatMessage;
        private IGameEventRepository _gameEvent;
        private IGameMemberRepository _gameMember;
        private IGameMessageRepository _gameMessage;
        private IGameRepository _game;
        private IGameViolationReportRepository _gameViolation;
        private IGemRepository _gem;
        private IReactionRepository _reaction;
        private IRefreshTokenRepository _refreshToken;
        private IRoomMemberRepository _roomMember;
        private IRoomRepository _room;
        private IVoteRepository _vote;

        public UnitOfWork(ApplicationDbContext context, IDbConnection connection)
        {
            this._context = context;
            this._connection = connection;
        }


        public IAvatarRepository Avatar => _avatar ??= new AvatarRepository(_context,_connection);
        public IEventJoinRepository EventJoin => _eventJoin ??= new EventJoinRepository(_context,_connection);
        public IEventRepository Event => _event ??= new EventRepository(_context,_connection);
        public IChatMessageRepository ChatMessage => _chatMessage ??= new ChatMessageRepository(_context, _connection);
        public IGameEventRepository GameEvent => _gameEvent ??= new GameEventRepository(_context,_connection);
        public IGameMemberRepository GameMember => _gameMember ??= new GameMemberRepository(_context,_connection);
        public IGameMessageRepository GameMessage => _gameMessage ??= new GameMessageRepository(_context,_connection);
        public IGameRepository Game => _game ??= new GameRepository(_context,_connection);
        public IGameViolationReportRepository GameViolation =>
            _gameViolation ??= new GameViolationReportRepository(_context,_connection);
        public IGemRepository Gem => _gem ??= new GemRepository(_context, _connection);
        public IReactionRepository Reaction => _reaction ??= new ReactionRepository(_context, _connection);
        public IRefreshTokenRepository RefreshToken => _refreshToken ??= new RefreshTokenRepository(_context, _connection);
        public IRoomMemberRepository RoomMember => _roomMember ??= new RoomMemberRepository(_context,_connection);
        public IRoomRepository Room => _room ??= new RoomRepository(_context,_connection);
        public IVoteRepository Vote => _vote ??= new VoteRepository(_context, _connection);

        public Task<int> Commit()
        {
            return _context.SaveChangesAsync();
        }
    }
}
