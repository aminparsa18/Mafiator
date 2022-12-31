using MemoryPack;

namespace Mafiator.Common.Data.Dtos.Countries;

[MemoryPackable]
public sealed partial class CountryResult
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Sign { get; set; }
}