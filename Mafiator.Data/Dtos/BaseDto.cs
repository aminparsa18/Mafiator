using System;
using System.ComponentModel;

namespace Mafiator.Data.Dtos
{
    public class BaseDto
    {
        public Ulid Id { get; set; }

        [DisplayName("تاریخ ایجاد")]
        public DateTime? DateCreated { get; set; }

        [DisplayName("تاریخ آخرین بروزرسانی")]
        public DateTime? DateModified { get; set; }

    }
}
