using Mafiator.Entities.Enums;
using Mafiator.Entities.Identity;
using System;

namespace Mafiator.Entities
{
    public class GameViolationReport:BaseEntity
    {

        public Guid GameId { get; set; }
        public Guid ReporterId { get; set; }
        public Guid ReportedId { get; set; }
        public GameViolationType ViolationType { get; set; }
        public virtual Game Game { get; set; }
        public virtual User Reporter { get; set; }
        public virtual User Reported { get; set; }
    }
}
