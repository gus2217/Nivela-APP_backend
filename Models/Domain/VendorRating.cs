namespace NivelaService.Models.Domain
{
    public class VendorRating
    {
        public long Id { get; set; }

        // The number of stars (1 to 5)
        public int Rating { get; set; }

        // The text review
        public string Review { get; set; }

        // Who posted it
        public string ReviewerName { get; set; }

        // When it was posted
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key (which vendor this rating belongs to)
        public long VendorId { get; set; }

        // Navigation property
        public Vendor Vendor { get; set; }
    }
}
