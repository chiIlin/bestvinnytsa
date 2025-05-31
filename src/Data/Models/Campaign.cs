using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace bestvinnytsa.web.Data.Models
{
    /// <summary>
    /// Сутність кампанії. Зберігається в колекції "Campaigns".
    /// </summary>
    public class Campaign
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("budget")]
        public decimal Budget { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } = null!;

        [BsonElement("link")]
        public string Link { get; set; } = null!;

        [BsonElement("isOpen")]
        public bool IsOpen { get; set; } = true;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("expiresAt")]
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// ObjectId продюсера (AppUser.Id) як string.
        /// </summary>
        [BsonElement("producerId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProducerId { get; set; } = null!;

        [BsonIgnore]
        public AppUser? Producer { get; set; }

        /// <summary>
        /// ObjectId категорії (Category.Id) як string.
        /// </summary>
        [BsonElement("categoryId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; } = null!;

        [BsonIgnore]
        public Category? Category { get; set; }

        [BsonIgnore]
        public List<Application> Applications { get; set; } = new();
    }
}
