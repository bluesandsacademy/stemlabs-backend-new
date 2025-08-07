namespace BlueSandsLMS.Common.DTOs
{
   public class RegisterUserDto
{
    public string FullName { get; set; } = null!;
    public string Email    { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Phone    { get; set; } = null!;
    public string Gender   { get; set; } = null!;
    public string Country  { get; set; } = null!;
    public string Dob      { get; set; } = null!; // Consider using DateTime if possible
}

}
