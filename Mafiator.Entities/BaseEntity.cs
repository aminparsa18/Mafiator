using System;
using System.Data;
using Mafiator.Common.Helpers;
using RepoDb.Attributes;

namespace Mafiator.Entities
{
    public class BaseEntity
    {
        [PropertyHandler(typeof(UlidPropertyHandler))]
        public Ulid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
