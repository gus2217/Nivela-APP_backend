namespace NivelaService.Models.Dto
{
    public class AddVendorRatingRequestDto
    {
        public int Rating { get; set; } // 1–5 stars
        public string Review { get; set; }
        public string ReviewerName { get; set; }
    }
}
