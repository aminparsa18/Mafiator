using System;
using RepoDb;
using RepoDb.Interfaces;

namespace Mafiator.Common.Helpers
{
    public class NullableUlidPropertyHandler:IPropertyHandler<string,Ulid?>
    {
        public Ulid? Get(string input, ClassProperty property)
        {
            if (Ulid.TryParse(input, out var result))
                return result;
            return null;
        }

        public string Set(Ulid? input, ClassProperty property)
        {
            return input?.ToString();
        }
    }
}
