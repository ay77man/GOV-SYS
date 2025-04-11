using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Government.Core.DTO
{
   public class StripeSettings
    {
        public required string SecretKey { get; set; }
        public required string WebhookSecret { get; set; }
    }
}
