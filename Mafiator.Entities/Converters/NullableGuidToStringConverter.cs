using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Mafiator.Entities.Converters
{
    public class NullableGuidToStringConverter : ValueConverter<Guid?, string>
    {
        private static readonly ConverterMappingHints DefaultHints = new(26);

        public NullableGuidToStringConverter(ConverterMappingHints mappingHints = null)
            : base(
                x => x.HasValue ? x.ToString() : "",
                x => Guid.Parse(x),
                DefaultHints.With(mappingHints))
        {
        }
    }
}