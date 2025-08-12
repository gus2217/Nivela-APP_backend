using NivelaService.Models.Domain;
using NivelaService.Models.Dto;

namespace NivelaService.Repository.Interface
{
    public interface IVendorRatingRepository
    {
        Task<VendorRating> AddRatingAsync(long vendorId, AddVendorRatingRequestDto rating);
        Task<IEnumerable<VendorRating>> GetRatingsByVendorIdAsync(long vendorId);
        Task<double> GetAverageRatingAsync(long vendorId);
    }
}
