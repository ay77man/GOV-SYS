using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Government.Core.DTO
{
    public class LicenseReplacementDto
    {
        public string LicenseType { get; set; }

        public string OriginalLicenseNumber { get; set; }

        public string Reason { get; set; } // Lost or Damaged

        public string PoliceReport { get; set; }

        public string DamagedLicensePhoto { get; set; }

        public decimal ReplacementFee { get; set; }

        public string PaymentMethod { get; set; }
    }
}
