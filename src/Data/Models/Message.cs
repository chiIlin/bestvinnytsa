using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace bestvinnytsa.web.Data.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("chatId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ChatId { get; set; } = null!;

        [BsonElement("senderId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SenderId { get; set; } = null!;

        [BsonElement("text")]
        public string Text { get; set; } = null!;

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [BsonElement("isRead")]
        public bool IsRead { get; set; } = false;
    }
}