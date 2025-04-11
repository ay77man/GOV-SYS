using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Government.Core.Domain.Entities
{
    public class Citizens
    {
        [Key] 
        public int Id { get; set; }
        public string NID { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
    }
}
