using NivelaService.Models.Domain;
using NivelaService.Models.Dto;

namespace NivelaService.Repository.Interface
{
    public interface ISocialsRepository
    {
        Task<Social> CreateAsync(Social social);
        Task<List<Social>> GetAllAsync(int? pageNumber = 1, int? pageSize = 10);
        Task<Social?> GetByIdAsync(long id);
        Task<Social?> DeleteAsync(long id);
        Task<Social?> UpdateAsync(long id, UpdateSocialDto service);
    }
}
