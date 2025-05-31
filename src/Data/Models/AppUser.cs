using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace bestvinnytsa.web.Data.Models
{
    /// <summary>
    /// Сутність користувача для збереження в MongoDB.
    /// Id => ObjectId, але в C# працюємо як зі string.
    /// </summary>
    public class AppUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("username")]
        public string UserName { get; set; } = null!;

        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [BsonElement("fullName")]
        public string FullName { get; set; } = null!;

        /// <summary>
        /// Зберігаємо хеш пароля (BCrypt).
        /// </summary>
        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; } = null!;

        [BsonElement("isEmailConfirmed")]
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Список назв ролей (наприклад, ["Producer"] або ["Influencer"]).
        /// </summary>
        [BsonElement("roles")]
        public List<string> Roles { get; set; } = new();

        [BsonIgnore]
        public List<Campaign> Campaigns { get; set; } = new();

        [BsonIgnore]
        public List<Application> Applications { get; set; } = new();
    }
}
