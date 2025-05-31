using System.Collections.Generic;

namespace bestvinnytsa.web.Data.Models
{
    /// <summary>
    /// Роль користувача (наприклад: Admin, Producer, Influencer).
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Унікальний ідентифікатор ролі.
        /// </summary>
        public byte Id { get; set; }

        /// <summary>
        /// Назва ролі.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Користувачі, які мають цю роль.
        /// </summary>
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
