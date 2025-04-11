using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Government.Core.DTO
{
    public class TrafficViolationDTO
    {
        public string PlateNumber { get; set; }

        public string ViolationType { get; set; }

        public decimal FineAmount { get; set; }

        public DateTime ViolationDate { get; set; }

        public string PaymentStatus { get; set; }

        public string PaymentMethod { get; set; }

        public string PaymentReceipt { get; set; }
    }
}
