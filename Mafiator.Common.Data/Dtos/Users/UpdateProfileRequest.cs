using MessagePack;

namespace Mafiator.Common.Data.Dtos.Users
{
    /// <summary>
    /// Update profile dto.
    /// </summary>
    [MessagePackObject()]
    public sealed class UpdateProfileRequest
    {
        /// <summary>
        /// Name.
        /// </summary>
        [Key(0)]
        public string Name { get; set; }

        /// <summary>
        /// Image.
        /// </summary>
        [Key(1)]
        public string Image { get; set; }
    }
}