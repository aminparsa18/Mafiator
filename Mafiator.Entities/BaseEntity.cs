using System;

namespace Mafiator.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
