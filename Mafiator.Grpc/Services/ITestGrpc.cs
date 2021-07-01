using MagicOnion;

namespace Mafiator.Grpc.Services
{
    public interface ITestGrpc : IService<ITestGrpc>
    {
        UnaryResult<int> SumAsync(int x, int y);
    }
}
