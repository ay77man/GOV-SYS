using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Government.Core.DTO
{
    public class RenewDrivingDTO
    {
        public int CurrentLicenseNumber { get; set; }

        //National ID
        public int NID { get; set; }
        //Current Expiry Date
        public DateOnly CurrentExpiryDate { get; set; }
        //Medical Checkup Required(Yes/No)
        public string MedicalCheckRequired { get; set; }
        //New Photo
        public IFormFile NewPhoto { get; set; }
        //Payment Method
        public string PaymentMethod { get; set; }
        //Renewal Date
        public string RenewalDate { get; set; }
        //New Expiry Date

        public string NewExpirayDate { get; set; }
    }
}
