using Microsoft.AspNetCore.Mvc;
using NivelaService.Models.Dto;
using NivelaService.Repository.Interface;
using NivelaService.Services.Interface;

namespace NivelaService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorImagesController : ControllerBase
    {
        private readonly IVendorImageRepository _vendorImageRepository;
        private readonly ICacheService _cacheService;

        public VendorImagesController(IVendorImageRepository vendorImageRepository, ICacheService cacheService)
        {
            _vendorImageRepository = vendorImageRepository;
            _cacheService = cacheService;
        }

        [HttpPost("{vendorId}/images")]
        public async Task<IActionResult> UploadImage(long vendorId, [FromForm] VendorImageUploadDto dto)
        {
            var image = await _vendorImageRepository.UploadAsync(vendorId, dto.File);
            if (image == null) return NotFound("Vendor not found");
            
            // Clear vendor cache since images are part of vendor data
            await _cacheService.RemoveAsync($"vendor_{vendorId}");
            await _cacheService.RemoveAsync("vendors_all");
            
            return Ok(image);
        }

        [HttpDelete("images/{imageId}")]
        public async Task<IActionResult> DeleteImage(long imageId)
        {
            var deleted = await _vendorImageRepository.DeleteAsync(imageId);
            if (!deleted) return NotFound();
            
            // Clear all vendor caches since we don't know which vendor this image belonged to
            await _cacheService.RemoveAsync("vendors_all");
            await _cacheService.RemovePatternAsync("vendor_*");
            
            return NoContent();
        }

        [HttpPut("images/{imageId}")]
        public async Task<IActionResult> EditImage(long imageId, [FromBody] VendorImageEditDto dto)
        {
            var updated = await _vendorImageRepository.EditAsync(imageId, dto);
            if (updated == null) return NotFound();
            
            // Clear all vendor caches since we don't know which vendor this image belongs to
            await _cacheService.RemoveAsync("vendors_all");
            await _cacheService.RemovePatternAsync("vendor_*");
            
            return Ok(updated);
        }
    }
}