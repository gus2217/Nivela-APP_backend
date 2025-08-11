using Microsoft.EntityFrameworkCore;
using NivelaService.Data;
using NivelaService.Models.Domain;
using NivelaService.Models.Dto;
using NivelaService.Repository.Interface;

namespace NivelaService.Repository.Implementations
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ServiceRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Service> CreateAsync(Service service)
        {
            await _applicationDbContext.Services.AddAsync(service);
            await _applicationDbContext.SaveChangesAsync();
            return service;
        }
        public async Task<List<Service>> GetAllAsync(int? pageNumber = 1, int? pageSize = 10)
        {
            var services = _applicationDbContext.Services.AsQueryable();

            var skip = (pageNumber - 1) * pageSize;
            services = services.Skip(skip ?? 0).Take(pageSize ?? 10);


            return await services.ToListAsync();
        }
        public async Task<Service?> GetByIdAsync(long id)
        {
            return await _applicationDbContext.Services.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Service?> DeleteAsync(long id)
        {
            var service = await _applicationDbContext.Services.FindAsync(id);
            if (service == null) return null;


            _applicationDbContext.Services.Remove(service);
            await _applicationDbContext.SaveChangesAsync();

            return service ?? null;
        }
        public async Task<Service?> UpdateAsync(long id, UpdateServiceDto service)
        {
            var updatedService = await _applicationDbContext.Services.FindAsync(id);
            if (updatedService == null) return null;

            updatedService.Name = service.Name;
            updatedService.Id = id;

            await _applicationDbContext.SaveChangesAsync();
            return updatedService;
        }
    }

}

