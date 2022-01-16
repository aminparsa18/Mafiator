using Mafiator.Entities.Enums;

namespace Mafiator.Entities.Extensions
{
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
}
