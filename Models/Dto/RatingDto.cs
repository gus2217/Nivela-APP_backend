namespace NivelaService.Models.Dto
{
    public class RatingDto
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
    }
}
