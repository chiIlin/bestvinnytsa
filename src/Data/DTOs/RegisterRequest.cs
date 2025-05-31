namespace bestvinnytsa.web.Data.DTOs
{
    public class RegisterRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FullName { get; set; }
        public byte RoleId { get; set; }     // 1 = Producer, 2 = Influencer
    }
}