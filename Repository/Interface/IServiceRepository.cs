using NivelaService.Models.Domain;
using NivelaService.Models.Dto;

namespace NivelaService.Repository.Interface
{
    public interface IServiceRepository
    {
        Task<Service> CreateAsync(Service service);
        Task<List<Service>> GetAllAsync(int? pageNumber = 1, int? pageSize = 10);
        Task<Service?> GetByIdAsync(long id);
        Task<Service?> DeleteAsync(long id);
        Task<Service?> UpdateAsync(long id, UpdateServiceDto service);
    }
}
