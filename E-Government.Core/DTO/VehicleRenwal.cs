using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Government.Core.DTO
{
    public class VehicleRenwal
    {
        public string PlateNumber { get; set; }

        public string VehicleRegistrationNumber { get; set; }

        public string TechnicalInspectionReport { get; set; }

        public string InsuranceDocument { get; set; }

        public decimal? PendingFines { get; set; }

        public string PaymentMethod { get; set; }

        public DateTime? RenewalDate { get; set; }
    }
}
