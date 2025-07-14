using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GoEASy.Controllers
{
    [Route("provider/tour")]
    public class ProviderTourController : Controller
    {
        private readonly TourService _tourService;

        public ProviderTourController(TourService tourService)
        {
            _tourService = tourService;
        }

        // GET: provider/tour
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            int? providerId = HttpContext.Session.GetInt32("UserId");
            if (providerId == null)
                return RedirectToAction("Index", "LoginAdmin");

            var tours = await _tourService.GetToursByProviderAsync(providerId.Value);
            var availableImages = GetAvailableImages();
            ViewBag.AvailableImages = availableImages;
            return View("~/Views/provider/ProviderTour.cshtml", tours);
        }

        private List<string> GetAvailableImages()
        {
            var images = new List<string>();
            var toursPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "tours");
            if (Directory.Exists(toursPath))
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var tourFolders = Directory.GetDirectories(toursPath);
                foreach (var tourFolder in tourFolders)
                {
                    var tourFolderName = Path.GetFileName(tourFolder);
                    var files = Directory.GetFiles(tourFolder)
                        .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
                        .Select(file => $"/assets/tours/{tourFolderName}/{Path.GetFileName(file)}")
                        .ToList();
                    images.AddRange(files);
                }
            }
            return images;
        }

        // POST: provider/tour/create
        [HttpPost("create")]
        public async Task<IActionResult> Create(Tour tour, List<IFormFile> images, string selectedImages)
        {
            int? providerId = HttpContext.Session.GetInt32("UserId");
            if (providerId == null)
                return RedirectToAction("Index", "LoginAdmin");
            tour.CreatedBy = providerId;
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
                        if (image.Length > 5 * 1024 * 1024)
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
                // Process selected images from existing folders
                var allImages = new List<IFormFile>();
                if (images != null)
                {
                    allImages.AddRange(images);
                }
                if (!string.IsNullOrEmpty(selectedImages))
                {
                    var selectedImagePaths = selectedImages.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var imagePath in selectedImagePaths)
                    {
                        var trimmedPath = imagePath.Trim();
                        if (trimmedPath.StartsWith("/assets/tours/"))
                        {
                            var relativePath = trimmedPath.TrimStart('/');
                            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);
                            if (System.IO.File.Exists(fullPath))
                            {
                                var imageName = Path.GetFileName(trimmedPath);
                                var mockImage = CreateMockFormFile(fullPath, imageName);
                                allImages.Add(mockImage);
                            }
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

        // POST: provider/tour/update
        [HttpPost("update")]
        public async Task<IActionResult> Update(Tour tour, List<IFormFile> images, string selectedImages)
        {
            int? providerId = HttpContext.Session.GetInt32("UserId");
            if (providerId == null)
                return RedirectToAction("Index", "LoginAdmin");
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
            var existingTour = (await _tourService.GetToursByProviderAsync(providerId.Value)).FirstOrDefault(t => t.TourId == tour.TourId);
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
                    if (image.Length > 5 * 1024 * 1024)
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
                var selectedImagePaths = selectedImages.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var imagePath in selectedImagePaths)
                {
                    var trimmedPath = imagePath.Trim();
                    if (trimmedPath.StartsWith("/assets/tours/"))
                    {
                        var relativePath = trimmedPath.TrimStart('/');
                        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);
                        if (System.IO.File.Exists(fullPath))
                        {
                            var imageName = Path.GetFileName(trimmedPath);
                            var mockImage = CreateMockFormFile(fullPath, imageName);
                            allImages.Add(mockImage);
                        }
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
            return RedirectToAction("Index");
        }

        [HttpPost("toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            int? providerId = HttpContext.Session.GetInt32("UserId");
            if (providerId == null)
                return RedirectToAction("Index", "LoginAdmin");
            var tour = (await _tourService.GetToursByProviderAsync(providerId.Value)).FirstOrDefault(t => t.TourId == id);
            if (tour == null)
            {
                return NotFound();
            }
            // Toggle status
            tour.Status = !(tour.Status ?? true);
            await _tourService.UpdateTourAsync(tour, null);
            return Ok();
        }

        [HttpPost("delete-confirm")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            int? providerId = HttpContext.Session.GetInt32("UserId");
            if (providerId == null)
                return RedirectToAction("Index", "LoginAdmin");
            var tour = (await _tourService.GetToursByProviderAsync(providerId.Value)).FirstOrDefault(t => t.TourId == id);
            if (tour == null)
            {
                TempData["Error"] = "Tour not found!";
                return RedirectToAction("Index");
            }
            // Xóa mềm - chỉ set status = false
            tour.Status = false;
            await _tourService.UpdateTourAsync(tour, null);
            TempData["Success"] = "Tour deleted successfully!";
            return RedirectToAction("Index");
        }
    }
} 