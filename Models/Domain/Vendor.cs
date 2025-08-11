namespace NivelaService.Models.Domain
{
    public class Vendor
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Contact { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string? Email { get; set; }
        public string? YearsOfExperience { get; set; }
        public string AreasServed { get; set; }
        public string Location { get; set; }
        public bool IsVisible { get; set; } = false;
        public ICollection<Service> Services { get; set; }
        public ICollection<Social> Socials { get; set; }
        public ICollection<VendorImage> Images { get; set; }
    }
}
