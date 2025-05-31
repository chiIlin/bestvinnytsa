using System;
using System.Collections.Generic;

namespace bestvinnytsa.web.Data.Models
{
    public class Campaign
    {
        public int Id { get; set; }

        public int ProducerId { get; set; }
        public User Producer { get; set; } = null!;

        public short CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public string Name { get; set; } = null!;       
        public string Budget { get; set; } = null!;      
        public string Description { get; set; } = null!; 
        public string Link { get; set; } = null!;        

        public bool IsOpen { get; set; } = true;       
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }        

        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}