namespace bestvinnytsa.web.Data.DTOs
{
    public class RegisterRequest
    {
        /// <summary>
        /// Email і одночасно UserName
        /// </summary>
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string FullName { get; set; } = null!;

        /// <summary>
        /// Назва ролі: "Producer" або "Influencer"
        /// </summary>
        public string Role { get; set; } = null!;
    }
}