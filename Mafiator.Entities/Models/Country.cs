using System.Text.Json.Serialization;

namespace Mafiator.Entities.Models;

public sealed class Country : BaseEntity
{
    [JsonPropertyName("n")]
    public string Name { get; set; }

    [JsonPropertyName("d")]
    public string Code { get; set; }

    [JsonPropertyName("c")]
    public string Sign { get; set; }
}