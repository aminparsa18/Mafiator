using MessagePack;

namespace Mafiator.Common.Data.Dtos.GameMembers
{
    /// <summary>
    /// Waiting game member dto.
    /// </summary>
    [MessagePackObject()]
    public sealed class WaitingPlayerResult
    {
        /// <summary>
        /// Game member user key identifier.
        /// </summary>
        [Key(0)]
        public string UserId { get; set; }

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

        /// <summary>
        /// Score.
        /// </summary>
        [Key(3)]
        public int Score { get; set; }
    }
}