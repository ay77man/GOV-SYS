using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Government.Core.DTO
{
    public class DrivingLicenseDTO
    {

        public string Name { get; set; }
        public int NID { get; set; }

        public DateOnly DateOfBirth { get; set; } = new DateOnly();
        public string Address { get; set; }
        public string LicenseType { get; set; }

        public IFormFile photo { get; set; }

        public string MedicalTest { get; set; }

        public string TheoryTest { get; set; }
        public string PracticalTest { get; set; }

        public DateOnly IssueDate { get; set; } = new DateOnly();
        public DateOnly ExpiryDate { get; set; } = new DateOnly();
    }
}
