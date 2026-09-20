using NOVA.Domain.Enums;
namespace NOVA.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public GlobalRole Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public Cart? Cart { get; set; }

    }
}
