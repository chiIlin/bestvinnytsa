using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bestvinnytsa.web.Data.Models
{
    /// <summary>
    /// Користувач системи: продюсер або інфлюенсер.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Унікальний ідентифікатор користувача.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Email користувача (унікальний).
        /// </summary>
        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Хеш паролю користувача.
        /// </summary>
        [Required, StringLength(256)]
        public string PasswordHash { get; set; } = null!;

        /// <summary>
        /// Повне ім’я користувача (опціонально).
        /// </summary>
        [StringLength(150)]
        public string? FullName { get; set; }

        /// <summary>
        /// Ідентифікатор ролі користувача.
        /// </summary>
        public byte RoleId { get; set; }

        /// <summary>
        /// Деталі ролі (Role).
        /// </summary>
        public Role Role { get; set; } = null!;

        /// <summary>
        /// Дата та час реєстрації користувача (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Прапорець, чи підтверджено email користувача.
        /// </summary>
        public bool IsEmailConfirmed { get; set; } = false;

        /// <summary>
        /// Кампанії, які створив цей користувач (як продюсер).
        /// </summary>
        public ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();

        /// <summary>
        /// Заявки, які подав цей користувач (як інфлюенсер).
        /// </summary>
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
