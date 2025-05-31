using System;

namespace bestvinnytsa.web.Data.Models
{
    /// <summary>
    /// Статус заявки від інфлюенсера на участь у кампанії.
    /// </summary>
    public enum ApplicationStatus
    {
        Pending,
        Approved,
        Rejected
    }

    /// <summary>
    /// Заявка інфлюенсера на участь у певній кампанії.
    /// </summary>
    public class Application
    {
        /// <summary>
        /// Унікальний ідентифікатор заявки.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ідентифікатор кампанії, до якої подається заявка.
        /// </summary>
        public int CampaignId { get; set; }

        /// <summary>
        /// Інформація про повʼязані дані кампанії.
        /// </summary>
        public Campaign Campaign { get; set; } = null!;

        /// <summary>
        /// Ідентифікатор інфлюенсера, який подає заявку.
        /// </summary>
        public int InfluencerId { get; set; }

        /// <summary>
        /// Інформація про користувача-інфлюенсера.
        /// </summary>
        public User Influencer { get; set; } = null!;

        /// <summary>
        /// Повідомлення від інфлюенсера (точка продажу / пояснення, чому саме він).
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Контактні дані (email, Telegram, інше).
        /// </summary>
        public string ContactInfo { get; set; } = null!;

        /// <summary>
        /// Дата та час створення заявки (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Поточний статус заявки (Pending, Approved, Rejected).
        /// </summary>
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    }
}
