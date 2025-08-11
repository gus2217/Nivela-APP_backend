using Microsoft.EntityFrameworkCore;
using NivelaService.Data;
using NivelaService.Models.Domain;
using NivelaService.Models.Dto;
using NivelaService.Repository.Interface;

namespace NivelaService.Repository.Implementations
{
    public class VendorImageRepository : IVendorImageRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public VendorImageRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<VendorImage?> UploadAsync(long vendorId, IFormFile file)
        {
            var vendorExists = await _applicationDbContext.Vendors.AnyAsync(v => v.Id == vendorId);
            if (!vendorExists) return null;

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var image = new VendorImage
            {
                FileName = Path.GetFileNameWithoutExtension(file.FileName),
                Extension = Path.GetExtension(file.FileName),
                Content = ms.ToArray(),
                VendorId = vendorId
            };

            await _applicationDbContext.Images.AddAsync(image);
            await _applicationDbContext.SaveChangesAsync();
            return image;
        }

        public async Task<bool> DeleteAsync(long imageId)
        {
            var image = await _applicationDbContext.Images.FindAsync(imageId);
            if (image == null) return false;

            _applicationDbContext.Images.Remove(image);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<VendorImage?> EditAsync(long imageId, VendorImageEditDto dto)
        {
            var image = await _applicationDbContext.Images.FindAsync(imageId);
            if (image == null) return null;

            image.FileName = dto.FileName;
            image.Extension = dto.Extension;

            await _applicationDbContext.SaveChangesAsync();
            return image;
        }
    }
}
