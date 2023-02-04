using MagicOnion;

namespace Mafiator.Game.Services;

public interface ITestService : IService<ITestService>
{
    UnaryResult<int> Sum(int x, int y);
}