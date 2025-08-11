using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NivelaService.Models.Domain;
using NivelaService.Models.Dto;
using NivelaService.Repository.Interface;

namespace NivelaService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        // POST: api/Service
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServiceDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var service = new Service { Name = dto.Name };
            var created = await _serviceRepository.CreateAsync(service);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // GET: api/Service
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? pageNumber = 1, [FromQuery] int? pageSize = 10)
        {
            var services = await _serviceRepository.GetAllAsync(pageNumber, pageSize);
            return Ok(services);
        }

        // GET: api/Service/{id}
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null) return NotFound();

            return Ok(service);
        }

        // PUT: api/Service/{id}
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateServiceDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _serviceRepository.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        // DELETE: api/Service/{id}
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _serviceRepository.DeleteAsync(id);
            if (deleted == null) return NotFound();

            return NoContent();
        }
    }
}
