using System.ComponentModel.DataAnnotations;

namespace bestvinnytsa.web.Data.DTOs
{
    public class CompanyRegisterRequest
    {
        // === ОБОВ'ЯЗКОВІ ПОЛЯ ===

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6, ErrorMessage = "Пароль має містити щонайменше 6 символів.")]
        public string Password { get; set; } = null!;

        [Required]
        public string CompanyName { get; set; } = null!;

        [Required]
        public string ContactPerson { get; set; } = null!;

        [Required]
        public string CompanyPhone { get; set; } = null!;


        // === НЕОБОВ'ЯЗКОВІ ПОЛЯ ===

        public string? Website { get; set; }
        public string? Industry { get; set; }
        public string? CompanySize { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CollaborationGoals { get; set; }
        public string? BudgetRange { get; set; }
        public string? TargetAudience { get; set; }
    }
}