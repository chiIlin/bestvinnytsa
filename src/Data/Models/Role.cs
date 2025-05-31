using System.Collections.Generic;

namespace bestvinnytsa.web.Data.Models
{
    public class Role
    {
        public byte Id { get; set; }
        public string Name { get; set; } = null!; 

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}