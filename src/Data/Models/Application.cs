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

        public int InfluencerId { get; set; }
        public User Influencer { get; set; } = null!;

        public string? Message { get; set; }
        public string ContactInfo { get; set; } = null!;  // Email, тг і тд
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    }
}