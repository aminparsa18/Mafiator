using MagicOnion;
using MagicOnion.Server;

namespace Mafiator.Realtime.Services;

public class TestService : ServiceBase<ITestService>, ITestService
{
    public async UnaryResult<int> Sum(int x, int y) => x + y;
}