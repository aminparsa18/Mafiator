using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Mafiator.Entities.Converters
{
    public class NullableUlidToStringConverter : ValueConverter<Ulid?, string>
    {
        private static readonly ConverterMappingHints DefaultHints = new ConverterMappingHints(size: 26);

        public NullableUlidToStringConverter(ConverterMappingHints mappingHints = null)
            : base(
                convertToProviderExpression: x => x.HasValue ? x.ToString() : "",
                convertFromProviderExpression: x => Ulid.Parse(x),
                mappingHints: DefaultHints.With(mappingHints))
        {
        }
    }
}