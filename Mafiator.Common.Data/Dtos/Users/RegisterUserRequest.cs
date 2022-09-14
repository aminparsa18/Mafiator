using MessagePack;

namespace Mafiator.Common.Data.Dtos.Users
{
    /// <summary>
    /// Register user dto.
    /// </summary>
    [MessagePackObject]
    public sealed class RegisterUserRequest
    {
        /// <summary>
        /// Phone number.
        /// </summary>
        [Key(0)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [Key(1)]
        public string Password { get; set; }

        /// <summary>
        /// Username.
        /// </summary>
        [Key(2)]
        public string Username { get; set; }

        /// <summary>
        /// Country code.
        /// </summary>
        [Key(3)]
        public string CountryCode { get; set; }
    }
}