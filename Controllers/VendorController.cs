using Microsoft.AspNetCore.Mvc;
using NivelaService.Models.Dto;
using NivelaService.Repository.Interface;
using NivelaService.Services.Interface;

namespace NivelaService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly ICacheService _cacheService;
        private readonly IConfiguration _configuration;

        public VendorController(IVendorRepository vendorRepository, ICacheService cacheService, IConfiguration configuration)
        {
            _vendorRepository = vendorRepository;
            _cacheService = cacheService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVendor([FromForm] CreateVendorDto dto)
        {
            var vendor = await _vendorRepository.CreateAsync(dto);
            
            // Clear relevant caches after creation
            await _cacheService.RemoveAsync("vendors_all");
            await _cacheService.RemovePatternAsync("vendors_service_*");
            
            return CreatedAtAction(nameof(GetVendorById), new { id = vendor.Id }, vendor);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVendors([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var cacheKey = $"vendors_all_{pageNumber ?? 1}_{pageSize ?? 10}";
            var cacheExpiration = TimeSpan.FromMinutes(_configuration.GetValue<int>("Cache:VendorCacheExpirationMinutes", 60));
            
            var vendors = await _cacheService.GetOrSetAsync(
                cacheKey,
                () => _vendorRepository.GetAllAsync(pageNumber, pageSize),
                cacheExpiration
            );
            
            return Ok(vendors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVendorById([FromRoute] long id)
        {
            var cacheKey = $"vendor_{id}";
            var cacheExpiration = TimeSpan.FromMinutes(_configuration.GetValue<int>("Cache:VendorCacheExpirationMinutes", 60));
            
            var vendor = await _cacheService.GetOrSetAsync(
                cacheKey,
                () => _vendorRepository.GetByIdAsync(id),
                cacheExpiration
            );
            
            return vendor == null ? NotFound() : Ok(vendor);
        }

        [HttpGet("service/{serviceId}")]
        public async Task<IActionResult> GetVendorsByService([FromRoute] long serviceId)
        {
            var cacheKey = $"vendors_service_{serviceId}";
            var cacheExpiration = TimeSpan.FromMinutes(_configuration.GetValue<int>("Cache:VendorCacheExpirationMinutes", 60));
            
            var vendors = await _cacheService.GetOrSetAsync(
                cacheKey,
                () => _vendorRepository.GetByServiceIdAsync(serviceId),
                cacheExpiration
            );
            
            return Ok(vendors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVendor([FromRoute] long id, [FromForm] UpdateVendorDto dto)
        {
            var vendor = await _vendorRepository.UpdateAsync(id, dto);
            if (vendor == null) return NotFound();
            
            // Clear relevant caches after update
            await _cacheService.RemoveAsync($"vendor_{id}");
            await _cacheService.RemoveAsync("vendors_all");
            await _cacheService.RemovePatternAsync("vendors_service_*");
            
            return Ok(vendor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVendor([FromRoute] long id)
        {
            var vendor = await _vendorRepository.DeleteAsync(id);
            if (vendor == null) return NotFound();
            
            // Clear relevant caches after deletion
            await _cacheService.RemoveAsync($"vendor_{id}");
            await _cacheService.RemoveAsync("vendors_all");
            await _cacheService.RemovePatternAsync("vendors_service_*");
            
            return Ok(vendor);
        }
    }
}