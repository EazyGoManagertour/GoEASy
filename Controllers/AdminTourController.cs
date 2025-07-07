using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace GoEASy.Controllers
{
    [Route("admin/tour-admin")]
    public class AdminTourController : Controller
    {
        private readonly TourService _tourService;

        public AdminTourController(TourService tourService)
        {
            _tourService = tourService;
        }

        // GET: admin/tour-admin
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var tours = await _tourService.GetAllToursAsync();
            
            // Lấy danh sách ảnh có sẵn trong thư mục Tourimg
            var availableImages = GetAvailableImages();
            ViewBag.AvailableImages = availableImages;
            
            return View("~/Views/admin/tour_admin/TourAdmin.cshtml", tours);
        }

        private List<string> GetAvailableImages()
        {
            var images = new List<string>();
            var tourImgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "Tourimg");
            
            if (Directory.Exists(tourImgPath))
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var files = Directory.GetFiles(tourImgPath)
                    .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
                    .Select(file => "/image/Tourimg/" + Path.GetFileName(file))
                    .ToList();
                
                images.AddRange(files);
            }
            
            return images;
        }

        // POST: admin/tour-admin/create
        [HttpPost("create")]
        public async Task<IActionResult> Create(Tour tour, List<IFormFile> images, string selectedImages)
        {
            try
            {
                var (isValid, errors) = ValidationService.ValidateTour(tour);

                if (!isValid)
                {
                    TempData["Error"] = string.Join("<br/>", errors);
                    return RedirectToAction("Index");
                }

                // Validate uploaded images
                if (images != null && images.Count > 0)
                {
                    foreach (var image in images)
                    {
                        if (image.Length > 5 * 1024 * 1024) // 5MB limit
                        {
                            TempData["Error"] = "Mỗi ảnh không được vượt quá 5MB";
                            return RedirectToAction("Index");
                        }

                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            TempData["Error"] = "Chỉ chấp nhận file ảnh: .jpg, .jpeg, .png, .gif";
                            return RedirectToAction("Index");
                        }
                    }
                }

                // Process selected images from existing folder
                var allImages = new List<IFormFile>();
                if (images != null)
                {
                    allImages.AddRange(images);
                }

                if (!string.IsNullOrEmpty(selectedImages))
                {
                    var selectedImageNames = selectedImages.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var imageName in selectedImageNames)
                    {
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "Tourimg", imageName.Trim());
                        if (System.IO.File.Exists(imagePath))
                        {
                            // Create a mock IFormFile from existing file
                            var mockImage = CreateMockFormFile(imagePath, imageName);
                            allImages.Add(mockImage);
                        }
                    }
                }

                await _tourService.AddTourAsync(tour, allImages);
                TempData["Success"] = "Tour created successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to create tour: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        private IFormFile CreateMockFormFile(string filePath, string fileName)
        {
            var fileInfo = new FileInfo(filePath);
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            
            return new FormFile(stream, 0, fileInfo.Length, "images", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = GetContentType(fileName)
            };
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }

        // POST: admin/tour-admin/update
        [HttpPost("update")]
        public async Task<IActionResult> Update(Tour tour, List<IFormFile> images, string selectedImages)
        {
            try
            {
                if (tour.TourId == 0)
                {
                    TempData["Error"] = "Tour ID is required!";
                    return RedirectToAction("Index");
                }

                var (isValid, errors) = ValidationService.ValidateTour(tour, isUpdate: true);

                if (!isValid)
                {
                    TempData["Error"] = string.Join("<br/>", errors);
                    return RedirectToAction("Index");
                }

                var existingTour = await _tourService.GetTourByIdAsync(tour.TourId);
                if (existingTour == null)
                {
                    TempData["Error"] = "Tour not found!";
                    return RedirectToAction("Index");
                }

                // Validate uploaded images if provided
                if (images != null && images.Count > 0)
                {
                    foreach (var image in images)
                    {
                        if (image.Length > 5 * 1024 * 1024) // 5MB limit
                        {
                            TempData["Error"] = "Mỗi ảnh không được vượt quá 5MB";
                            return RedirectToAction("Index");
                        }

                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            TempData["Error"] = "Chỉ chấp nhận file ảnh: .jpg, .jpeg, .png, .gif";
                            return RedirectToAction("Index");
                        }
                    }
                }

                // Process selected images from existing folder
                var allImages = new List<IFormFile>();
                if (images != null)
                {
                    allImages.AddRange(images);
                }

                if (!string.IsNullOrEmpty(selectedImages))
                {
                    var selectedImageNames = selectedImages.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var imageName in selectedImageNames)
                    {
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "Tourimg", imageName.Trim());
                        if (System.IO.File.Exists(imagePath))
                        {
                            // Create a mock IFormFile from existing file
                            var mockImage = CreateMockFormFile(imagePath, imageName);
                            allImages.Add(mockImage);
                        }
                    }
                }

                // Cập nhật thông tin
                existingTour.TourName = tour.TourName;
                existingTour.Description = tour.Description;
                existingTour.AdultPrice = tour.AdultPrice;
                existingTour.ChildPrice = tour.ChildPrice;
                existingTour.StartDate = tour.StartDate;
                existingTour.EndDate = tour.EndDate;
                existingTour.MaxSeats = tour.MaxSeats;
                existingTour.AvailableSeats = tour.AvailableSeats;
                existingTour.DestinationId = tour.DestinationId;
                existingTour.CategoryId = tour.CategoryId;

                await _tourService.UpdateTourAsync(existingTour, allImages);
                TempData["Success"] = "Tour updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to update tour: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost("delete-confirm")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            try
            {
                var tour = await _tourService.GetTourByIdAsync(id);
                if (tour == null)
                {
                    TempData["Error"] = "Tour not found!";
                    return RedirectToAction("Index");
                }

                await _tourService.DeleteTourAsync(id);
                TempData["Success"] = "Tour deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to delete tour: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
} 