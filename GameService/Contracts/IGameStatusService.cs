using MagicOnion;

namespace GameService.Contracts
{
   public interface IGameStatusService : IService<IGameStatusService>
    {
        // The return type must be `UnaryResult<T>`.
        UnaryResult<int> SumAsync(int x, int y);
    }
}
