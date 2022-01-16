using MafiatorApp.Dtos.User;
using MafiatorApp.Models;
using System.Collections.Generic;

namespace MafiatorApp
{
    public class SystemConstant
    {
        public static List<ValidateUserDto> Members { get; set; } = new();
        public static List<NewGameRole> SelectedRoles { get; set; }
        public static string PlayingVoice { get; set; }
    }
}