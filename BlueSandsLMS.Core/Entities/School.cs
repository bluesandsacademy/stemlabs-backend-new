namespace BlueSandsLMS.Core.Entities
{
    public class School
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Subdomain { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
