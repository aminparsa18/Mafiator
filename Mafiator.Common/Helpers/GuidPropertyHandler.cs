using RepoDb;
using RepoDb.Interfaces;
using System;

namespace Mafiator.Common.Helpers
{
    public class GuidPropertyHandler:IPropertyHandler<string,Guid>
    {
        public Guid Get(string input, ClassProperty property)
        {
            return Guid.Parse(input);
        }

        public string Set(Guid input, ClassProperty property)
        {
            return input.ToString();
        }
    }
}
