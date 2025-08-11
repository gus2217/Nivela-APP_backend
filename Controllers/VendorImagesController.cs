using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NivelaService.Data;
using NivelaService.Models.Dto;
using NivelaService.Repository.Interface;

namespace NivelaService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorImagesController : ControllerBase
    {
        private readonly IVendorImageRepository _vendorImageRepository;

        public VendorImagesController(IVendorImageRepository vendorImageRepository)
        {
            _vendorImageRepository = vendorImageRepository;
        }

        [HttpPost("{vendorId}/images")]
        public async Task<IActionResult> UploadImage(long vendorId, [FromForm] VendorImageUploadDto dto)
        {
            var image = await _vendorImageRepository.UploadAsync(vendorId, dto.File);
            if (image == null) return NotFound("Vendor not found");
            return Ok(image);
        }

        [HttpDelete("images/{imageId}")]
        public async Task<IActionResult> DeleteImage(long imageId)
        {
            var deleted = await _vendorImageRepository.DeleteAsync(imageId);
            return deleted ? NoContent() : NotFound();
        }

        [HttpPut("images/{imageId}")]
        public async Task<IActionResult> EditImage(long imageId, [FromBody] VendorImageEditDto dto)
        {
            var updated = await _vendorImageRepository.EditAsync(imageId, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }
    }
}
