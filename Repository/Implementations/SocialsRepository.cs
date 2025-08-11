using Microsoft.EntityFrameworkCore;
using NivelaService.Data;
using NivelaService.Models.Domain;
using NivelaService.Models.Dto;
using NivelaService.Repository.Interface;

namespace NivelaService.Repository.Implementations
{
    public class SocialsRepository : ISocialsRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public SocialsRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Social> CreateAsync(Social social)
        {
            await _applicationDbContext.Socials.AddAsync(social);
            await _applicationDbContext.SaveChangesAsync();
            return social;
        }
        public async Task<List<Social>> GetAllAsync(int? pageNumber = 1, int? pageSize = 10)
        {
            var social = _applicationDbContext.Socials.AsQueryable();

            var skip = (pageNumber - 1) * pageSize;
            social = social.Skip(skip ?? 0).Take(pageSize ?? 10);


            return await social.ToListAsync();
        }
        public async Task<Social?> GetByIdAsync(long id)
        {
            return await _applicationDbContext.Socials.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Social?> DeleteAsync(long id)
        {
            var social = await _applicationDbContext.Socials.FindAsync(id);
            if (social == null) return null;


            _applicationDbContext.Socials.Remove(social);
            await _applicationDbContext.SaveChangesAsync();

            return social ?? null;
        }
        public async Task<Social?> UpdateAsync(long id, UpdateSocialDto social)
        {
            var updatedSocial = await _applicationDbContext.Socials.FindAsync(id);
            if (updatedSocial == null) return null;

            updatedSocial.Name = social.Name;
            updatedSocial.Link = social.Link;
            updatedSocial.Id = id;

            await _applicationDbContext.SaveChangesAsync();
            return updatedSocial;
        }
    }
}
