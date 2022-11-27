using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Game.Models;

namespace Mafiator.Game
{
    public class SystemConstant
    {
        public static List<ValidateUserResult> Members { get; set; } = new();
        public static List<NewGameRole> SelectedRoles { get; set; }
        public static string PlayingVoice { get; set; }
    }
}