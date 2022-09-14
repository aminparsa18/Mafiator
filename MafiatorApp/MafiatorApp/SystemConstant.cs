using Mafiator.Common.Data.Dtos.Users;
using MafiatorApp.Models;
using System.Collections.Generic;

namespace MafiatorApp
{
    public class SystemConstant
    {
        public static List<ValidateUserResult> Members { get; set; } = new();
        public static List<NewGameRole> SelectedRoles { get; set; }
        public static string PlayingVoice { get; set; }
    }
}