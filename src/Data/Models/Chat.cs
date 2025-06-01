using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace bestvinnytsa.web.Data.Models
{
    public class Chat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        // Учасники чату (userId)
        [BsonElement("participants")]
        public List<string> Participants { get; set; } = new();

        // Для зручності фронта
        [BsonElement("lastMessage")]
        public string? LastMessage { get; set; }

        [BsonElement("lastMessageTime")]
        public DateTime? LastMessageTime { get; set; }

        // Для badge непрочитаних
        [BsonElement("unreadCount")]
        public Dictionary<string, int> UnreadCount { get; set; } = new();
    }
}