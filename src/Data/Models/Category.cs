using System.Collections.Generic;

namespace bestvinnytsa.web.Data.Models
{
    /// <summary>
    /// Категорія для кампаній (наприклад: "Beauty", "Tech", "Food").
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Унікальний ідентифікатор категорії.
        /// </summary>
        public short Id { get; set; }

        /// <summary>
        /// Назва категорії.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Опис категорії (опціонально).
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Кампанії, що належать до цієї категорії.
        /// </summary>
        public ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();
    }
}
