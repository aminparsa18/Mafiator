using RepoDb;
using RepoDb.Interfaces;
using System;

namespace Mafiator.Common.Helpers
{
    public class NullableGuidPropertyHandler:IPropertyHandler<string,Guid?>
    {
        public Guid? Get(string input, ClassProperty property)
        {
            if (Guid.TryParse(input, out var result))
                return result;
            return null;
        }

        public string Set(Guid? input, ClassProperty property)
        {
            return input?.ToString();
        }
    }
}
