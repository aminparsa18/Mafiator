using MessagePack;

namespace Mafiator.Common.Data.Dtos.Users
{
    /// <summary>
    /// User login dto.
    /// </summary>
    [MessagePackObject]
    public sealed class UserLoginRequest
    {
        /// <summary>
        /// Username.
        /// </summary>
        [Key(0)]
        public string Username { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [Key(1)]
        public string Password { get; set; }
    }
}