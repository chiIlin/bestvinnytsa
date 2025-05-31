using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace bestvinnytsa.web.Data.Models
{
    /// <summary>
    /// Сутність заявки (Application). Зберігається в колекції "Applications".
    /// </summary>
    public class Application
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        /// <summary>
        /// ObjectId кампанії (Campaign.Id) як string.
        /// </summary>
        [BsonElement("campaignId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CampaignId { get; set; } = null!;

        [BsonIgnore]
        public Campaign? Campaign { get; set; }

        /// <summary>
        /// ObjectId інфлюенсера (AppUser.Id) як string.
        /// </summary>
        [BsonElement("influencerId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string InfluencerId { get; set; } = null!;

        [BsonIgnore]
        public AppUser? Influencer { get; set; }

        [BsonElement("contactInfo")]
        public string ContactInfo { get; set; } = null!;

        /// <summary>
        /// Статус заявки: "Pending", "Accepted", "Rejected" тощо.
        /// </summary>
        [BsonElement("status")]
        public string Status { get; set; } = "Pending";

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
