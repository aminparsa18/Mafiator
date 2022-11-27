using System.Text.Json.Serialization;

namespace Mafiator.Game.Models
{
    public sealed class Country
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("dial_code")]
        public string DialCode { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}