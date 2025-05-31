using System.Collections.Generic;

namespace bestvinnytsa.web.Data.Models
{
    public class Role
    {
        public byte Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
    }
}
