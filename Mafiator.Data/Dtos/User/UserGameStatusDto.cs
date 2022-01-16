using Mafiator.Entities.Enums;

namespace Mafiator.Data.Dtos.User
{
   public class UserGameStatusDto
    {
        public string MemberId{ get; set; }
        public GameStatus GameStatus { get; set; }
        public GameRole GameRole { get; set; }
    }
}
