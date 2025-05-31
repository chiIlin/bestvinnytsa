using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace bestvinnytsa.web.Data.Models
{
    /// <summary>
    /// Сутність ролі для MongoDB.
    /// Id => ObjectId (string), 
    /// Name — назва ролі, 
    /// NormalizedName — назва у верхньому реєстрі для пошуку.
    /// </summary>
    public class AppRole
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("normalizedName")]
        public string NormalizedName { get; set; } = null!;
    }
}
