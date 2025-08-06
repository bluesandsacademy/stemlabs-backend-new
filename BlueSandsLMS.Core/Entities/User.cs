namespace BlueSandsLMS.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public Guid? SchoolId { get; set; }
        public Guid RoleId { get; set; }        // FK to Role
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastLogin { get; set; }

        public School? School { get; set; }
        public Role? Role { get; set; }
    }
}
