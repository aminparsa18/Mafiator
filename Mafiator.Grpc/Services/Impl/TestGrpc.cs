using System.Threading.Tasks;
using MagicOnion;
using MagicOnion.Server;

namespace Mafiator.Grpc.Services.Impl
{
   public class TestGrpc : ServiceBase<ITestGrpc>, ITestGrpc
    {
        public async UnaryResult<int> SumAsync(int x, int y)
        {
            await Task.Delay(500);
            return x + y;
        }
    }
}
