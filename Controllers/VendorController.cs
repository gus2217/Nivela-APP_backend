using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NivelaService.Models.Dto;
using NivelaService.Repository.Interface;

namespace NivelaService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorController(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        // POST: api/vendor
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateVendorDto dto)
        {
            if (Request.Form.TryGetValue("socials", out var socialsJson))
            {
                try
                {
                    dto.Socials = JsonSerializer.Deserialize<List<CreateSocialsDto>>(socialsJson!) ?? new();
                }
                catch (JsonException ex)
                {
                    return BadRequest($"Invalid socials format: {ex.Message}");
                }
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdVendor = await _vendorRepository.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdVendor.Id }, createdVendor);
        }

        // GET: api/vendor
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? pageNumber = 1, [FromQuery] int? pageSize = 10)
        {
            var vendors = await _vendorRepository.GetAllAsync(pageNumber, pageSize);
            return Ok(vendors);
        }

        // GET: api/vendor/{id}
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var vendor = await _vendorRepository.GetByIdAsync(id);
            if (vendor == null)
                return NotFound();

            return Ok(vendor);
        }

        [HttpGet("service/{serviceId:long}")]
        public async Task<IActionResult> GetVendorsByService(long serviceId)
        {
            var vendors = await _vendorRepository.GetByServiceIdAsync(serviceId);
            if (vendors == null || !vendors.Any())
                return NotFound("No vendors found for this service.");

            return Ok(vendors);
        }

        // PUT: api/vendor/{id}
        [HttpPut("{id:long}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(long id, [FromForm] UpdateVendorDto dto)
        {
            if (Request.Form.TryGetValue("socials", out var socialsJson))
            {
                try
                {
                    dto.Socials = JsonSerializer.Deserialize<List<UpdateSocialDto>>(socialsJson!) ?? new();
                }
                catch (JsonException ex)
                {
                    return BadRequest($"Invalid socials format: {ex.Message}");
                }
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _vendorRepository.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        // DELETE: api/vendor/{id}
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _vendorRepository.DeleteAsync(id);
            if (deleted == null)
                return NotFound();

            return NoContent();
        }
    }
}
