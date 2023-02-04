using MagicOnion;

namespace Mafiator.Realtime.Services;

public interface ITestService : IService<ITestService>
{
    UnaryResult<int> Sum(int x, int y);
}