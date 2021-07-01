using GameService.Contracts;
using MagicOnion;
using MagicOnion.Server;

namespace GameService.Services
{
    public class GameStatusService : ServiceBase<IGameStatusService>, IGameStatusService
    {
        // `UnaryResult<T>` allows the method to be treated as `async` method.
        public async UnaryResult<int> SumAsync(int x, int y)
        {
            return x + y;
        }
    }
}
