using Mafiator.Game.Resources.Texts;
using Microsoft.Extensions.Localization;

namespace Mafiator.Game.Validations;

public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
{
    public IsNotNullOrEmptyRule(IStringLocalizer<AppResources> localizer)
    {
        ValidationMessage = localizer["EmptyInput"];
    }

    public string ValidationMessage { get; set; }

    public bool Check(T value)
    {
        return value switch
        {
            null => false,
            //var str = value as string;
            string str => !string.IsNullOrWhiteSpace(str),
            _ => true
        };
    }
}