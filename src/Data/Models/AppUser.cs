using Microsoft.AspNetCore.Identity;

namespace bestvinnytsa.web.Data.Models
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
        public bool IsEmailConfirmed { get; set; }

        public byte RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}