using MessagePack;
using System;

namespace Mafiator.Common.Data.Dtos.Users
{
    /// <summary>
    /// Validated user dto.
    /// </summary>
    [MessagePackObject()]
    public sealed class ValidateUserResult
    {
        /// <summary>
        /// User key identifier.
        /// </summary>
        [Key(0)]
        public Guid Id { get; set; }

        /// <summary>
        /// Display name.
        /// </summary>
        [Key(1)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Image.
        /// </summary>
        [Key(2)]
        public string Image { get; set; }
    }
}