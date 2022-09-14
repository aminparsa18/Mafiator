using MessagePack;

namespace Mafiator.Common.Data.Dtos.Users
{
    /// <summary>
    /// Confirm phone dto.
    /// </summary>
    [MessagePackObject]
    public sealed class ConfirmPhoneRequest
    {
        /// <summary>
        /// Phone number.
        /// </summary>
        [Key(0)]
        public string PhoneNo { get; set; }

        /// <summary>
        /// Jwt token.
        /// </summary>
        [Key(1)]
        public string Token { get; set; }
    }
}