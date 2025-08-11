namespace NivelaService.Models.Domain
{
    public class VendorImage
    {
        public long Id { get; set; }

        public string FileName { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public byte[] Content { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Foreign key relationship
        public long VendorId { get; set; }
        public Vendor Vendor { get; set; }
    }
}
