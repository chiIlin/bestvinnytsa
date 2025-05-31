using System.ComponentModel.DataAnnotations;

namespace bestvinnytsa.web.Data.DTOs
{
    public class PersonRegisterRequest
    {
        // === ОБОВ'ЯЗКОВІ ПОЛЯ ===

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6, ErrorMessage = "Пароль має містити щонайменше 6 символів.")]
        public string Password { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;


        // === НЕОБОВ'ЯЗКОВІ ПОЛЯ ===

        public string? City { get; set; }
        public string? Biography { get; set; }
        public string? ContentCategories { get; set; }
        public string? InstagramHandle { get; set; }
        public string? YoutubeHandle { get; set; }
        public string? TiktokHandle { get; set; }
        public string? TelegramHandle { get; set; }
    }
}