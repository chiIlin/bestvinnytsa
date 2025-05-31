// Data/Models/Application.cs
using System;

namespace bestvinnytsa.web.Data.Models
{
    public enum ApplicationStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class Application
    {
        public int Id { get; set; }

        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; } = null!;

        // GUID користувача (AppUser.Id)
        public string InfluencerId { get; set; } = null!;
        public AppUser Influencer { get; set; } = null!;

        public string? Message { get; set; }
        public string ContactInfo { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    }
}
