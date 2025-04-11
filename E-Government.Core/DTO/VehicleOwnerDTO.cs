using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Government.Core.DTO
{
    public class VehicleOwnerDTO
    {
        public string NationalId { get; set; }

        public string OwnerName { get; set; }

        public string VehicleType { get; set; }

        public string Model { get; set; }

        public int ManufactureYear { get; set; }

        public string Color { get; set; }

        public string ChassisNumber { get; set; }

        public string InspectionReport { get; set; }

        public string InsuranceDocument { get; set; }

        public string OwnershipProof { get; set; }
    }
}
