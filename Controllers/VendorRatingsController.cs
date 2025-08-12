using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NivelaService.Models.Domain;
using NivelaService.Models.Dto;
using NivelaService.Repository.Interface;

namespace NivelaService.Controllers
{
    [Route("api/rating")]
    [ApiController]
    public class VendorRatingsController : ControllerBase
    {
        private readonly IVendorRatingRepository _ratingRepository;

        public VendorRatingsController(IVendorRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        [HttpPost("{vendorId}")]
        public async Task<IActionResult> AddRating(long vendorId, [FromBody] AddVendorRatingRequestDto request)
        {
            var createdRating = await _ratingRepository.AddRatingAsync(vendorId, request);
            return Ok(createdRating);
        }

        [HttpGet("{vendorId}")]
        public async Task<IActionResult> GetRatings(long vendorId)
        {
            var ratings = await _ratingRepository.GetRatingsByVendorIdAsync(vendorId);
            return Ok(ratings);
        }

        [HttpGet("average/{vendorId}")]
        public async Task<IActionResult> GetAverageRating(long vendorId)
        {
            var average = await _ratingRepository.GetAverageRatingAsync(vendorId);
            return Ok(Math.Round(average, 2));
        }
    }
}

