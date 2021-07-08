using System;
using Mafiator.Common.Helpers;
using Mafiator.Entities.Enums;
using Mafiator.Entities.Identity;
using RepoDb.Attributes;

namespace Mafiator.Entities
{
  public class GameMessage:BaseEntity
    {
        [PropertyHandler(typeof(UlidPropertyHandler))]
        public Ulid GameId { get; set; }
        [PropertyHandler(typeof(UlidPropertyHandler))]
        public Ulid UserId { get; set; }
        public GameMessageType MessageType { get; set; }
        public string Content { get; set; }
        public virtual Game Game { get; set; }
        public virtual User User { get; set; }
    }
}
