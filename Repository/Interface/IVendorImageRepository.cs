using NivelaService.Models.Domain;
using NivelaService.Models.Dto;

namespace NivelaService.Repository.Interface
{
    public interface IVendorImageRepository
    {
        Task<VendorImage?> UploadAsync(long vendorId, IFormFile file);
        Task<bool> DeleteAsync(long imageId);
        Task<VendorImage?> EditAsync(long imageId, VendorImageEditDto dto);
    }
}
