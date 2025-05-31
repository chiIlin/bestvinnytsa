using System.Collections.Generic;

namespace bestvinnytsa.web.Data.Models
{
    public class Category
    {
        public short Id { get; set; }
        public string Name { get; set; } = null!;     
        public string? Description { get; set; }       

        public ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();
    }
}