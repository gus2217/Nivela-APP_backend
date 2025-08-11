using NivelaService.Models.Domain;

namespace NivelaService.Models.Dto
{
    public class CreateVendorDto
    {
        public string Name { get; set; }
        public string Contact { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string Location { get; set; } 
        public string? Email { get; set; } 
        public string? YearsOfExperience { get; set; }
        public string AreasServed  { get; set; }

        // Accept an array of existing Service IDs
        public List<long> ServiceIds { get; set; } = new List<long>();

        // Accept a list of new Socials to be created
        public List<CreateSocialsDto> Socials { get; set; } = new List<CreateSocialsDto>();
        public List<IFormFile> Images { get; set; }
    }
}
