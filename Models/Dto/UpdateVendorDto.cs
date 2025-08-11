namespace NivelaService.Models.Dto
{
    public class UpdateVendorDto
    {
        public string Name { get; set; }
        public string Contact { get; set; } 
        public string Description { get; set; } 
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? YearsOfExperience { get; set; }
        public string AreasServed { get; set; }
        public bool IsVisible { get; set; }

        // New list of service IDs to update associations
        public List<long> ServiceIds { get; set; } = new List<long>();

        // New list of socials (can replace old ones)
        public List<UpdateSocialDto> Socials { get; set; } = new List<UpdateSocialDto>();
        public List<IFormFile>? Images { get; set; }
    }
}
