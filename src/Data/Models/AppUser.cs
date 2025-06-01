using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace bestvinnytsa.web.Data.Models
{
    public class AppUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("userName")]
        public string UserName { get; set; } = null!;

        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; } = null!;

        [BsonElement("isEmailConfirmed")]
        public bool IsEmailConfirmed { get; set; }

        [BsonElement("roles")]
        public List<string> Roles { get; set; } = new List<string>();

        [BsonElement("fullName")]
        [BsonIgnoreIfNull]
        public string? FullName { get; set; }

        // ======= Поля для фізичної особи =======

        [BsonElement("phoneNumber")]
        [BsonIgnoreIfNull]
        public string? PhoneNumber { get; set; }

        [BsonElement("city")]
        [BsonIgnoreIfNull]
        public string? City { get; set; }

        [BsonElement("biography")]
        [BsonIgnoreIfNull]
        public string? Biography { get; set; }

        [BsonElement("contentCategories")]
        [BsonIgnoreIfNull]
        public string? ContentCategories { get; set; }

        [BsonElement("instagramHandle")]
        [BsonIgnoreIfNull]
        public string? InstagramHandle { get; set; }

        [BsonElement("youtubeHandle")]
        [BsonIgnoreIfNull]
        public string? YoutubeHandle { get; set; }

        [BsonElement("tiktokHandle")]
        [BsonIgnoreIfNull]
        public string? TiktokHandle { get; set; }

        [BsonElement("telegramHandle")]
        [BsonIgnoreIfNull]
        public string? TelegramHandle { get; set; }

        // НОВІ ПОЛЯ - кількість підписників
        [BsonElement("instagramFollowers")]
        public int? InstagramFollowers { get; set; }
    
        [BsonElement("youtubeFollowers")]
        public int? YoutubeFollowers { get; set; }
    
        [BsonElement("tiktokFollowers")]
        public int? TiktokFollowers { get; set; }
    
        [BsonElement("telegramFollowers")]
        public int? TelegramFollowers { get; set; }
        
        [BsonElement("photoUrl")]
        [BsonIgnoreIfNull]
        public string? PhotoUrl { get; set; }


        // ======= Поля для компанії =======

        [BsonElement("companyName")]
        [BsonIgnoreIfNull]
        public string? CompanyName { get; set; }

        [BsonElement("contactPerson")]
        [BsonIgnoreIfNull]
        public string? ContactPerson { get; set; }

        [BsonElement("companyPhone")]
        [BsonIgnoreIfNull]
        public string? CompanyPhone { get; set; }

        [BsonElement("website")]
        [BsonIgnoreIfNull]
        public string? Website { get; set; }

        [BsonElement("industry")]
        [BsonIgnoreIfNull]
        public string? Industry { get; set; }

        [BsonElement("companySize")]
        [BsonIgnoreIfNull]
        public string? CompanySize { get; set; }

        [BsonElement("companyDescription")]
        [BsonIgnoreIfNull]
        public string? CompanyDescription { get; set; }

        [BsonElement("collaborationGoals")]
        [BsonIgnoreIfNull]
        public string? CollaborationGoals { get; set; }

        [BsonElement("budgetRange")]
        [BsonIgnoreIfNull]
        public string? BudgetRange { get; set; }

        [BsonElement("targetAudience")]
        [BsonIgnoreIfNull]
        public string? TargetAudience { get; set; }

        [BsonElement("role")]
        public string Role { get; set; } = null!;
    }
}
