﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Government.Core.Domain.Entities
{
    public class Meter
    {
        public int Id { get; set; }
        public string MeterNumber { get; set; } // رقم العداد
        public MeterType Type { get; set; } // نوع العداد
        public bool IsActive { get; set; } // شغال ولا مفصول
        public DateTime InstallationDate { get; set; } = DateTime.UtcNow;
        // علاقته بالزبون
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        // علاقته بالقراءات
        public List<MeterReading> Readings { get; set; } = new();
        public ICollection<Bill> Bills { get; set; } = new List<Bill>();

    }


}
