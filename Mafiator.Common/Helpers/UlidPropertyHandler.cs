using System;
using RepoDb;
using RepoDb.Interfaces;

namespace Mafiator.Common.Helpers
{
    public class UlidPropertyHandler:IPropertyHandler<string,Ulid>
    {
        public Ulid Get(string input, ClassProperty property)
        {
            return Ulid.Parse(input);
        }

        public string Set(Ulid input, ClassProperty property)
        {
            return input.ToString();
        }
    }
}
