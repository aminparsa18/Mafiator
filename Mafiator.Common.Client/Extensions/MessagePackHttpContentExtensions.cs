using System.Net.Http;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Extensions
{
    public static class MessagePackHttpContentExtensions
    {
        public static async Task<T> ReadAsMessagePackAsync<T>(this HttpContent content)
        {
            return await content.ReadAsAsync<T>(MessagePackMediaTypeFormatter.DefaultMediaTypeFormatters).ConfigureAwait(false);
        }
    }
}