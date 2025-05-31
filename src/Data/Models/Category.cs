using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace bestvinnytsa.web.Data.Models
{
    /// <summary>
    /// Сутність категорії (наприклад, "Косметика", "Їжа", "Технології").
    /// Зберігається в колекції "Categories".
    /// </summary>
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("description")]
        public string Description { get; set; } = null!;

        [BsonIgnore]
        public List<Campaign> Campaigns { get; set; } = new();
    }
}
