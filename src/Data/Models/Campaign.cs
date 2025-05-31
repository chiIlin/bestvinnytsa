using System;
using System.Collections.Generic;

namespace bestvinnytsa.web.Data.Models
{
    /// <summary>
    /// Кампанія, яку створив продюсер (Producer), до якої можуть подаватися заявки.
    /// </summary>
    public class Campaign
    {
        /// <summary>
        /// Унікальний ідентифікатор кампанії.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ідентифікатор користувача-Продюсера, який створив кампанію.
        /// </summary>
        public int ProducerId { get; set; }

        /// <summary>
        /// Деталі про продюсера (User).
        /// </summary>
        public User Producer { get; set; } = null!;

        /// <summary>
        /// Ідентифікатор категорії кампанії.
        /// </summary>
        public short CategoryId { get; set; }

        /// <summary>
        /// Деталі категорії (Category).
        /// </summary>
        public Category Category { get; set; } = null!;

        /// <summary>
        /// Назва кампанії.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Бюджет кампанії (рядок, наприклад "500$" або "UAH 10000").
        /// </summary>
        public string Budget { get; set; } = null!;

        /// <summary>
        /// Опис умов співпраці, вимоги до інфлюенсерів тощо.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Посилання на сторінку або ресурс (можливо, на товар/послугу).
        /// </summary>
        public string Link { get; set; } = null!;

        /// <summary>
        /// Прапорець, чи кампанія відкрита для нових заявок.
        /// </summary>
        public bool IsOpen { get; set; } = true;

        /// <summary>
        /// Дата та час створення кампанії (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// (Опціонально) Дата завершення кампанії, після якої вона вважається закритою.
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// Колекція заявок (Application), поданих інфлюенсерами на цю кампанію.
        /// </summary>
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
