using Cysharp.Serialization.MessagePack;
using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;

namespace Mafiator.Common.Api
{
    public class CustomMessagePackResolver : IFormatterResolver
    {
        public static readonly IFormatterResolver Instance = new CustomMessagePackResolver();

        // configure your custom resolvers.
        private static readonly IFormatterResolver[] Resolvers = {
            UlidMessagePackResolver.Instance,
            ContractlessStandardResolver.Instance
        };

        private CustomMessagePackResolver()
        {
        }

        public IMessagePackFormatter<T> GetFormatter<T>()
        {
            return Cache<T>.Formatter;
        }

        private static class Cache<T>
        {
            public static readonly IMessagePackFormatter<T> Formatter;

            static Cache()
            {
                
                foreach (var resolver in Resolvers)
                {
                    var f = resolver.GetFormatter<T>();
                    if (f == null) continue;
                    Formatter = f;
                    return;
                }
            }
        }
    }
}
