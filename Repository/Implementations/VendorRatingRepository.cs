using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NivelaService.Data;
using NivelaService.Models.Domain;
using NivelaService.Models.Dto;
using NivelaService.Repository.Interface;

namespace NivelaService.Repository.Implementations
{
    public class VendorRatingRepository : IVendorRatingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public VendorRatingRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VendorRating> AddRatingAsync( long vendorId, AddVendorRatingRequestDto request)
        {
            var rating = _mapper.Map<VendorRating>(request);

            rating.VendorId = vendorId; // Set the foreign key to the vendor ID
            _context.VendorRatings.Add(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<IEnumerable<VendorRating>> GetRatingsByVendorIdAsync(long vendorId)
        {
            return await _context.VendorRatings
                .Where(r => r.VendorId == vendorId)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync(long vendorId)
        {
            var ratings = await _context.VendorRatings
                .Where(r => r.VendorId == vendorId)
                .ToListAsync();

            return ratings.Any() ? ratings.Average(r => r.Rating) : 0;
        }
    }
}

