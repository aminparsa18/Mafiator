using System.Collections.Generic;
using MafiatorApp.Dtos;
using MafiatorApp.Models;

namespace MafiatorApp
{
    public class SystemConstant
    {
        public static List<ValidateUserDto> Members { get; set; }=new List<ValidateUserDto>();
        public static List<NewGameRole> SelectedRoles { get; set; }
        public static string PlayingVoice { get; set; }
    }
}