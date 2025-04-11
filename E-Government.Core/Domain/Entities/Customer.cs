namespace E_Government.Core.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string NationalId { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string AccountNumber { get; set; } // New property from JSON
        public CustomerCategory Category { get; set; } = CustomerCategory.Residential;
        public CustomerStatus Status { get; set; } = CustomerStatus.Active;

        // Navigation properties
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<Meter> Meters { get; set; } = new List<Meter>();
        public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    }

}