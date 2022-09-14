using MessagePack;

namespace Mafiator.Common.Data.Dtos.Users
{
    /// <summary>
    /// User dto.
    /// </summary>
    [MessagePackObject]
    public sealed class UserDetailsResult
    {
        /// <summary>
        /// Display name.
        /// </summary>
        [Key(0)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Image.
        /// </summary>
        [Key(1)]
        public string Image { get; set; }

        /// <summary>
        /// Score.
        /// </summary>
        [Key(2)]
        public int Score { get; set; }

        /// <summary>
        /// Country code.
        /// </summary>
        [Key(3)]
        public string CountryCode { get; set; }
    }
}