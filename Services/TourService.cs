using GoEASy.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoEASy.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;

namespace GoEASy.Services
{
    public class TourService
    {
        private readonly GoEasyContext _context;

        public TourService(GoEasyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tour>> GetAllToursAsync()
        {
            return await _context.Tours
                .Include(t => t.TourImages)
                .Include(t => t.Destination)
                .Include(t => t.Category)
                .Where(t => t.Status == true || t.Status == null) // Chỉ lấy tours active hoặc chưa set status
                .ToListAsync();
        }

        public async Task<IEnumerable<Tour>> GetAllToursForAdminAsync()
        {
            return await _context.Tours
                .Include(t => t.TourImages)
                .Include(t => t.Destination)
                .Include(t => t.Category)
                .ToListAsync(); // Lấy tất cả tours cho admin
        }

        public async Task<Tour?> GetTourByIdAsync(int id)
        {
            var tour = await _context.Tours
                .Include(t => t.TourImages)
                .Include(t => t.Destination)
                .Include(t => t.Category)
                .Include(t => t.TourDetail)
                .Where(t => t.TourID == id && (t.Status == true || t.Status == null))
                .FirstOrDefaultAsync();
            return tour;
        }

        public async Task<Tour?> GetTourByIdForAdminAsync(int id)
        {
            var tour = await _context.Tours
                .Include(t => t.TourImages)
                .Include(t => t.Destination)
                .Include(t => t.Category)
                .Include(t => t.TourDetail)
                .FirstOrDefaultAsync(t => t.TourID == id);
            return tour;
        }

        public List<string> GetIncludedList(Tour tour)
        {
            var included = tour.TourDetail?.Included ?? string.Empty;
            return included.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
        }

        public List<string> GetExcludedList(Tour tour)
        {
            var excluded = tour.TourDetail?.Excluded ?? string.Empty;
            return excluded.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
        }

        public List<string> GetActivitiesList(Tour tour)
        {
            var activities = tour.TourDetail?.Activities ?? string.Empty;
            return activities.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
        }

        // Phương thức search và filter tours
        public async Task<IEnumerable<Tour>> SearchToursAsync(
            string? searchTerm = null,
            int? CategoryID = null,
            int? DestinationID = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? sortBy = null,
            string? sortOrder = "asc")
        {
            var query = _context.Tours
                .Include(t => t.TourImages)
                .Include(t => t.Destination)
                .Include(t => t.Category)
                .Where(t => t.Status == true || t.Status == null) // Chỉ lấy tours active
                .AsQueryable();

            // Filter theo search term
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.TourName.Contains(searchTerm) ||
                                       t.Description.Contains(searchTerm) ||
                                       t.Destination.DestinationName.Contains(searchTerm));
            }

            // Filter theo category
            if (CategoryID.HasValue)
            {
                query = query.Where(t => t.CategoryID == CategoryID.Value);
            }

            // Filter theo destination
            if (DestinationID.HasValue)
            {
                query = query.Where(t => t.DestinationID == DestinationID.Value);
            }

            // Filter theo price range (sử dụng AdultPrice)
            if (minPrice.HasValue)
            {
                query = query.Where(t => t.AdultPrice >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(t => t.AdultPrice <= maxPrice.Value);
            }

            // Filter theo start date
            if (startDate.HasValue)
            {
                query = query.Where(t => t.StartDate >= startDate.Value);
            }

            // Filter theo end date
            if (endDate.HasValue)
            {
                query = query.Where(t => t.EndDate <= endDate.Value);
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        query = sortOrder.ToLower() == "desc"
                            ? query.OrderByDescending(t => t.TourName)
                            : query.OrderBy(t => t.TourName);
                        break;
                    case "price":
                        query = sortOrder.ToLower() == "desc"
                            ? query.OrderByDescending(t => t.AdultPrice)
                            : query.OrderBy(t => t.AdultPrice);
                        break;
                    case "startdate":
                        query = sortOrder.ToLower() == "desc"
                            ? query.OrderByDescending(t => t.StartDate)
                            : query.OrderBy(t => t.StartDate);
                        break;
                    case "enddate":
                        query = sortOrder.ToLower() == "desc"
                            ? query.OrderByDescending(t => t.EndDate)
                            : query.OrderBy(t => t.EndDate);
                        break;
                    default:
                        query = query.OrderBy(t => t.TourID);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(t => t.TourID);
            }

            return await query.ToListAsync();
        }

        // Lấy tất cả categories cho filter
        public async Task<IEnumerable<TourCategory>> GetAllCategoriesAsync()
        {
            return await _context.TourCategories.ToListAsync();
        }

        // Lấy tất cả destinations cho filter
        public async Task<IEnumerable<Destination>> GetAllDestinationsAsync()
        {
            return await _context.Destinations.ToListAsync();
        }

        // Lấy tours theo category
        public async Task<IEnumerable<Tour>> GetToursByCategoryAsync(int CategoryID)
        {
            return await _context.Tours
                .Include(t => t.TourImages)
                .Include(t => t.Destination)
                .Include(t => t.Category)
                .Where(t => t.CategoryID == CategoryID && (t.Status == true || t.Status == null))
                .ToListAsync();
        }

        // Lấy tours theo destination
        public async Task<IEnumerable<Tour>> GetToursByDestinationAsync(int DestinationID)
        {
            return await _context.Tours
                .Include(t => t.TourImages)
                .Include(t => t.Destination)
                .Include(t => t.Category)
                .Where(t => t.DestinationID == DestinationID && (t.Status == true || t.Status == null))
                .ToListAsync();
        }

        // Lấy tours theo price range
        public async Task<IEnumerable<Tour>> GetToursByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Tours
                .Include(t => t.TourImages)
                .Include(t => t.Destination)
                .Include(t => t.Category)
                .Where(t => t.AdultPrice >= minPrice && t.AdultPrice <= maxPrice && (t.Status == true || t.Status == null))
                .ToListAsync();
        }

        // Lấy tours theo start date
        public async Task<IEnumerable<Tour>> GetToursByStartDateAsync(DateTime startDate)
        {
            return await _context.Tours
                .Include(t => t.TourImages)
                .Include(t => t.Destination)
                .Include(t => t.Category)
                .Where(t => t.StartDate >= startDate && (t.Status == true || t.Status == null))
                .ToListAsync();
        }

        public async Task<IEnumerable<Tour>> GetToursByProviderAsync(int providerId)
        {
            return await _context.Tours
                .Include(t => t.TourImages)
                .Include(t => t.Destination)
                .Include(t => t.Category)
                .Where(t => t.CreatedBy == providerId)
                .ToListAsync();
        }

        public async Task<bool> AddTourAsync(Tour tour, List<IFormFile>? images)
        {
            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();

            if (images != null && images.Count > 0)
            {
                // Tạo tên folder từ tên tour
                var folderName = tour.TourName?.ToLowerInvariant()
                    .Replace(" ", "-")
                    .Replace("đ", "d").Replace("ă", "a").Replace("â", "a")
                    .Replace("ê", "e").Replace("ô", "o").Replace("ơ", "o").Replace("ư", "u")
                    .Replace(":", "").Replace(";", "").Replace(",", "").Replace(".", "")
                    .Replace("!", "").Replace("?", "").Replace("(", "").Replace(")", "")
                    .Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "")
                    .Replace("'", "").Replace("\"", "").Replace("\\", "").Replace("/", "")
                    .Replace("|", "").Replace("<", "").Replace(">", "");
                
                if (string.IsNullOrEmpty(folderName))
                {
                    folderName = "tour-" + tour.TourID;
                }
                
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "tours", folderName);
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                for (int i = 0; i < images.Count; i++)
                {
                    var image = images[i];
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    
                    var ImageURL = $"/assets/tours/{folderName}/{fileName}";
                    
                    var tourImage = new TourImage
                    {
                        TourID = tour.TourID,
                        ImageURL = ImageURL,
                        IsCover = (i == 0),
                        UploadedAt = DateTime.Now
                    };
                    _context.TourImages.Add(tourImage);
                }
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> UpdateTourAsync(Tour tour, List<IFormFile>? images)
        {
            _context.Tours.Update(tour);
            await _context.SaveChangesAsync();

            if (images != null && images.Count > 0)
            {
                // Tạo tên folder từ tên tour
                var folderName = tour.TourName?.ToLowerInvariant()
                    .Replace(" ", "-")
                    .Replace("đ", "d").Replace("ă", "a").Replace("â", "a")
                    .Replace("ê", "e").Replace("ô", "o").Replace("ơ", "o").Replace("ư", "u")
                    .Replace(":", "").Replace(";", "").Replace(",", "").Replace(".", "")
                    .Replace("!", "").Replace("?", "").Replace("(", "").Replace(")", "")
                    .Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "")
                    .Replace("'", "").Replace("\"", "").Replace("\\", "").Replace("/", "")
                    .Replace("|", "").Replace("<", "").Replace(">", "");
                
                if (string.IsNullOrEmpty(folderName))
                {
                    folderName = "tour-" + tour.TourID;
                }
                
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "tours", folderName);
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);
                    
                // Xóa tất cả ảnh cũ của tour này
                var oldImages = _context.TourImages.Where(i => i.TourID == tour.TourID).ToList();
                _context.TourImages.RemoveRange(oldImages);
                await _context.SaveChangesAsync();
                
                for (int i = 0; i < images.Count; i++)
                {
                    var image = images[i];
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    var ImageURL = $"/assets/tours/{folderName}/{fileName}";
                    var tourImage = new TourImage
                    {
                        TourID = tour.TourID,
                        ImageURL = ImageURL,
                        IsCover = (i == 0),
                        UploadedAt = DateTime.Now
                    };
                    _context.TourImages.Add(tourImage);
                }
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> DeleteTourAsync(int id)
        {
            var tour = await _context.Tours.Include(t => t.TourImages).FirstOrDefaultAsync(t => t.TourID == id);
            if (tour != null)
            {
                // Xóa ảnh liên quan
                if (tour.TourImages != null && tour.TourImages.Count > 0)
                {
                    _context.TourImages.RemoveRange(tour.TourImages);
                }
                _context.Tours.Remove(tour);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<TourItinerary>> GetTourItinerariesAsync(int TourID)
        {
            return await _context.TourItineraries
                .Where(i => i.TourID == TourID)
                .OrderBy(i => i.DayNumber)
                .ToListAsync();
        }

        public async Task<List<TourFAQ>> GetTourFAQsAsync(int TourID)
        {
            return await _context.TourFAQs
                .Where(faq => faq.TourID == TourID)
                .ToListAsync();
        }

        public Destination? GetDestinationById(int DestinationID)
        {
            return _context.Destinations.FirstOrDefault(d => d.DestinationID == DestinationID);
        }
    }
}
