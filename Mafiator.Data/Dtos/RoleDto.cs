using System.Text.Json.Serialization;

namespace Mafiator.Data.Dtos
{
    public class RoleDto : BaseDto
    {

        public string Name { get; set; }

        public string CaptionPersian { get; set; }

        [JsonPropertyName("تعداد کاربران")]
        public int UsersCount { get; set; }

    }
}
