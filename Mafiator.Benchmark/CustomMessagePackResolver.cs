using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;

namespace Mafiator.Benchmark;

/// <summary>
/// Storage of typed serializers.
/// </summary>
public sealed class CustomMessagePackResolver : IFormatterResolver
{
    public static readonly IFormatterResolver Instance = new CustomMessagePackResolver();

    /// <summary>
    /// configure your custom resolvers.
    /// </summary>
    private static readonly IFormatterResolver resolver = ContractlessStandardResolver.Instance;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomMessagePackResolver"/> class.
    /// </summary>
    public CustomMessagePackResolver()
    {
    }

    /// <summary>
    /// Get Formatter from cache.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>IMessagePackFormatter.</returns>
    public IMessagePackFormatter<T> GetFormatter<T>()
    {
        return Cache<T>.Formatter;
    }

    /// <summary>
    /// Cache formatter type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private static class Cache<T>
    {
        public static readonly IMessagePackFormatter<T> Formatter;

        static Cache() => Formatter = resolver.GetFormatter<T>();
    }
}