using RepoDb.Interfaces;
using RepoDb.Options;
using System;

namespace Mafiator.Common.Helpers;

/// <summary>
///  Feature that would allow you to handle the tranformation of the class properties and database columns (inbound/outbound). 
///  It allows you to customize the conversion of the class properties and the .NET CLR types.
/// </summary>
public class GuidPropertyHandler : IPropertyHandler<string, Guid>
{
    public Guid Get(string input, PropertyHandlerGetOptions options)
    {
        return Guid.Parse(input);
    }


    public string Set(Guid input, PropertyHandlerSetOptions options)
    {
        return input.ToString();
    }
}