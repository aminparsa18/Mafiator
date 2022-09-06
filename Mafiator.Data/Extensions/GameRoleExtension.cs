using Mafiator.Common.Data.Enums;

namespace Mafiator.Data.Extensions;

public static class GameRoleExtension
{
    public static bool IsCitizen(this GameRole gameRole)
    {
        return gameRole != GameRole.Mafia && gameRole != GameRole.GodFather && gameRole != GameRole.Terrorist;
    }

    public static bool IsMafia(this GameRole gameRole)
    {
        return gameRole is GameRole.Mafia or GameRole.GodFather or GameRole.Terrorist;
    }
}