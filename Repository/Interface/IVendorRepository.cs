using NivelaService.Models.Domain;
using NivelaService.Models.Dto;

namespace NivelaService.Repository.Interface
{
    public interface IVendorRepository
    {
        Task<VendorToDisplayDto> CreateAsync(CreateVendorDto dto);
        Task<List<VendorToDisplayDto>> GetAllAsync(int? pageNumber = 1, int? pageSize = 10);
        Task<VendorToDisplayDto?> GetByIdAsync(long id);
        Task<List<VendorToDisplayDto>> GetByServiceIdAsync(long serviceId);
        Task<VendorToDisplayDto?> UpdateAsync(long id, UpdateVendorDto dto);
        Task<VendorToDisplayDto?> DeleteAsync(long id);
    }
}
