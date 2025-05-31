using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bestvinnytsa.web.Data.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; } = null!;

        [Required, StringLength(256)]
        public string PasswordHash { get; set; } = null!;

        [StringLength(150)]
        public string? FullName { get; set; }

        public byte RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsEmailConfirmed { get; set; } = false;
        
        public ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();
        
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}