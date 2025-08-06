namespace BlueSandsLMS.Common.DTOs
{
    public class RegisterUserDto
    {
        public string FullName { get; set; } = null!;
        public string Email    { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
