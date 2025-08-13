using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NivelaService.Data;
using NivelaService.Models.Domain;
using NivelaService.Models.Dto;
using NivelaService.Repository.Interface;

namespace NivelaService.Repository.Implementations
{
    public class VendorRepository : IVendorRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public VendorRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<VendorToDisplayDto> CreateAsync(CreateVendorDto dto)
        {
            var vendor = _mapper.Map<Vendor>(dto);

            vendor.Services = await _applicationDbContext.Services
                .Where(s => dto.ServiceIds.Contains(s.Id)).ToListAsync();

            vendor.Socials = _mapper.Map<List<Social>>(dto.Socials);

            // Manually map uploaded images
            if (dto.Images != null && dto.Images.Count > 0)
            {
                vendor.Images = new List<VendorImage>();

                foreach (var formFile in dto.Images)
                {
                    using var ms = new MemoryStream();
                    await formFile.CopyToAsync(ms);

                    vendor.Images.Add(new VendorImage
                    {
                        FileName = Path.GetFileNameWithoutExtension(formFile.FileName),
                        Extension = Path.GetExtension(formFile.FileName),
                        Content = ms.ToArray()
                    });
                }
            }

            await _applicationDbContext.Vendors.AddAsync(vendor);
            await _applicationDbContext.SaveChangesAsync();

            return _mapper.Map<VendorToDisplayDto>(vendor);
        }

        public async Task<List<VendorToDisplayDto>> GetAllAsync(int? pageNumber = 1, int? pageSize = 10)
        {
            var page = Math.Max(pageNumber ?? 1, 1);
            var size = Math.Max(pageSize ?? 10, 1);

            var vendors = await _applicationDbContext.Vendors
                .Include(v => v.Services)
                .Include(v => v.Socials)
                .Include(v => v.Images)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return _mapper.Map<List<VendorToDisplayDto>>(vendors);
        }

        public async Task<VendorToDisplayDto?> GetByIdAsync(long id)
        {
            var vendor = await _applicationDbContext.Vendors
                .Include(v => v.Services)
                .Include(v => v.Socials)
                .Include(v => v.Images)
                .FirstOrDefaultAsync(v => v.Id == id);

            return vendor == null ? null : _mapper.Map<VendorToDisplayDto>(vendor);
        }

        public async Task<List<VendorToDisplayDto>> GetByServiceIdAsync(long serviceId)
        {
            var vendors = await _applicationDbContext.Vendors
                .Include(v => v.Services)
                .Include(v => v.Socials)
                .Include(v => v.Images)
                .Where(v => v.Services.Any(s => s.Id == serviceId))
                .ToListAsync();

            return _mapper.Map<List<VendorToDisplayDto>>(vendors);
        }

        public async Task<VendorToDisplayDto?> UpdateAsync(long id, UpdateVendorDto dto)
        {
            var vendor = await _applicationDbContext.Vendors
                .Include(v => v.Services)
                .Include(v => v.Socials)
                .Include(v => v.Images)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vendor == null) return null;

            // Map primitive and scalar fields (e.g., Name, Contact, etc.)
            _mapper.Map(dto, vendor);

            // --- Update Services ---
            vendor.Services.Clear();
            var updatedServices = await _applicationDbContext.Services
                .Where(s => dto.ServiceIds.Contains(s.Id)).ToListAsync();
            foreach (var service in updatedServices)
            {
                vendor.Services.Add(service); // Maintain EF tracking
            }

            // --- Update Socials ---
            if (dto.Socials != null && dto.Socials.Any())
            {
                _applicationDbContext.Socials.RemoveRange(vendor.Socials);
                vendor.Socials.Clear();

                var newSocials = _mapper.Map<List<Social>>(dto.Socials);
                foreach (var social in newSocials)
                {
                    vendor.Socials.Add(social);
                }
            }


            // --- Update Images if new ones are provided ---
            if (dto.Images != null && dto.Images.Any())
            {
                _applicationDbContext.Images.RemoveRange(vendor.Images);
                vendor.Images.Clear();

                foreach (var formFile in dto.Images)
                {
                    using var ms = new MemoryStream();
                    await formFile.CopyToAsync(ms);

                    vendor.Images.Add(new VendorImage
                    {
                        FileName = Path.GetFileNameWithoutExtension(formFile.FileName),
                        Extension = Path.GetExtension(formFile.FileName),
                        Content = ms.ToArray()
                    });
                }
            }

            await _applicationDbContext.SaveChangesAsync();
            return _mapper.Map<VendorToDisplayDto>(vendor);
        }

        public async Task<VendorToDisplayDto?> DeleteAsync(long id)
        {
            var vendor = await _applicationDbContext.Vendors
                .Include(v => v.Socials)
                .Include(v => v.Services)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vendor == null) return null;

            // Explicitly delete socials
            _applicationDbContext.Socials.RemoveRange(vendor.Socials);

            _applicationDbContext.Vendors.Remove(vendor);
            await _applicationDbContext.SaveChangesAsync();

            return _mapper.Map<VendorToDisplayDto>(vendor);
        }

    }
}