namespace BlueSandsLMS.Common.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public Guid UserId { get; set; }
        
        public Guid? SchoolId { get; set; }

    }
}
