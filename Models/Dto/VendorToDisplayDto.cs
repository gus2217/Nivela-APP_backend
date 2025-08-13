namespace NivelaService.Models.Dto
{
    public class VendorToDisplayDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? YearsOfExperience { get; set; }
        public string AreasServed { get; set; }
        public bool IsVisible { get; set; }
        public List<ServiceDto> Services { get; set; } = new();
        public List<SocialDto> Socials { get; set; } = new();
        public List<VendorImageDto> Images { get; set; } = new();
        public List<RatingDto> Ratings { get; set; } = new();
    }
}
