using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NivelaService.Models.Domain;
using NivelaService.Models.Dto;
using NivelaService.Repository.Implementations;
using NivelaService.Repository.Interface;

namespace NivelaService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialController : ControllerBase
    {
        private readonly ISocialsRepository _socialsRepository;

        public SocialController(ISocialsRepository socialsRepository)
        {
            _socialsRepository = socialsRepository;
        }

        // POST: api/Service
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSocialsDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var social = new Social { Name = dto.Name, Link = dto.Link };
            var created = await _socialsRepository.CreateAsync(social);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // GET: api/Service
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? pageNumber = 1, [FromQuery] int? pageSize = 10)
        {
            var socials = await _socialsRepository.GetAllAsync(pageNumber, pageSize);
            return Ok(socials);
        }

        // GET: api/Service/{id}
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var social = await _socialsRepository.GetByIdAsync(id);
            if (social == null) return NotFound();

            return Ok(social);
        }

        // PUT: api/Service/{id}
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateSocialDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _socialsRepository.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        // DELETE: api/Service/{id}
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _socialsRepository.DeleteAsync(id);
            if (deleted == null) return NotFound();

            return NoContent();
        }
    }
}
