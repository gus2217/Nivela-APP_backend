namespace NivelaService.Models.Domain
{
    public class Service
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public ICollection<Vendor> Vendors { get; set; }
    }

}
