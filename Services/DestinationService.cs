using GoEASy.Models;
using GoEASy.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace GoEASy.Services
{
    public class DestinationService
    {
        private readonly IGenericRepository<Destination> _destinationRepo;
        private readonly GoEasyContext _context;

        public DestinationService(IGenericRepository<Destination> destinationRepo, GoEasyContext context)
        {
            _destinationRepo = destinationRepo;
            _context = context;
        }

        // Sử dụng repository cho CRUD cơ bản
        public async Task<IEnumerable<Destination>> GetAllDestinationsAsync()
        {
            // Nếu muốn include images/tours thì cần custom thêm
            return await _context.Destinations
                .Include(d => d.DestinationImages)
                .Include(d => d.Tours)
                .ToListAsync();
        }

        public async Task<Destination?> GetDestinationByIdAsync(int id)
        {
            return await _context.Destinations
                .Include(d => d.DestinationImages)
                .Include(d => d.Tours)
                .FirstOrDefaultAsync(d => d.DestinationID == id);
        }

        public async Task<bool> AddDestinationAsync(Destination destination, List<IFormFile> images = null)
        {
            destination.CreatedAt = DateTime.Now;
            destination.UpdatedAt = DateTime.Now;
            await _destinationRepo.AddAsync(destination);
            await _destinationRepo.SaveAsync();

            if (images != null && images.Count > 0)
            {
                await SaveDestinationImagesAsync(destination.DestinationID, images);
            }

            return true;
        }

        public async Task<bool> UpdateDestinationAsync(Destination destination, List<IFormFile> images = null)
        {
            destination.UpdatedAt = DateTime.Now;
            _destinationRepo.Update(destination);
            await _destinationRepo.SaveAsync();

            // Nếu có ảnh mới (upload hoặc chọn lại), thì xóa toàn bộ ảnh cũ và thêm ảnh mới
            if (images != null && images.Count > 0)
            {
                var existingImages = await _context.DestinationImages
                    .Where(img => img.DestinationID == destination.DestinationID)
                    .ToListAsync();

                foreach (var image in existingImages)
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImageURL.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.DestinationImages.RemoveRange(existingImages);
                await _context.SaveChangesAsync();

                await SaveDestinationImagesAsync(destination.DestinationID, images);
            }
            return true;
        }

        public async Task<bool> DeleteDestinationAsync(int id)
        {
            var destination = await _context.Destinations
                .Include(d => d.DestinationImages)
                .FirstOrDefaultAsync(d => d.DestinationID == id);
            if (destination == null) return false;

            foreach (var image in destination.DestinationImages)
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImageURL.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _destinationRepo.Delete(destination);
            await _destinationRepo.SaveAsync();
            return true;
        }

        // Hàm xử lý ảnh giữ nguyên
        private async Task SaveDestinationImagesAsync(int DestinationID, List<IFormFile> images)
        {
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "destinations");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var destinationImages = new List<DestinationImage>();
            var isFirstImage = true;

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    var destinationImage = new DestinationImage
                    {
                        DestinationID = DestinationID,
                        ImageURL = "/uploads/destinations/" + fileName,
                        Caption = Path.GetFileNameWithoutExtension(image.FileName),
                        IsCover = isFirstImage,
                        UploadedAt = DateTime.Now
                    };

                    destinationImages.Add(destinationImage);
                    isFirstImage = false;
                }
            }

            if (destinationImages.Any())
            {
                _context.DestinationImages.AddRange(destinationImages);
                await _context.SaveChangesAsync();
            }
        }
    }
} 