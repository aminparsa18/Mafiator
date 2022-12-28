using System.Net.Http;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Extensions;

public static class MemoryPackHttpContentExtensions
{
    public static async Task<T> ReadAsMessagePackAsync<T>(this HttpContent content) =>
        await content.ReadAsAsync<T>(MessagePackMediaTypeFormatter.DefaultMediaTypeFormatters).ConfigureAwait(false);
}