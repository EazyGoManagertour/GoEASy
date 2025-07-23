using GoEASy.Filters;
using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    //[AdminAuthorize]
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
            var tours = await _tourService.GetAllToursForAdminAsync();
            
            // Lấy danh sách ảnh có sẵn trong thư mục Tourimg
            var availableImages = GetAvailableImages();
            ViewBag.AvailableImages = availableImages;
            
            return View("~/Views/admin/tour_admin/TourAdmin.cshtml", tours);
        }

        private List<string> GetAvailableImages()
        {
            var images = new List<string>();
            
            // Lấy ảnh từ thư mục cũ (backward compatibility)
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
            
            // Lấy ảnh từ cấu trúc thư mục mới theo tour
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

        private List<string> GetAvailableImagesByTour(int tourID)
        {
            var images = new List<string>();
            
            // Lấy tour để biết tên folder
            var tour = _tourService.GetTourByIdForAdminAsync(tourID).Result;
            if (tour == null) return images;
            
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
            
            var tourPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "tours", folderName);
            if (Directory.Exists(tourPath))
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var files = Directory.GetFiles(tourPath)
                    .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
                    .Select(file => $"/assets/tours/{folderName}/{Path.GetFileName(file)}")
                    .ToList();
                
                images.AddRange(files);
            }
            
            return images;
        }

        private List<string> GetAvailableImagesByDestination(int? DestinationID)
        {
            var images = new List<string>();
            
            if (DestinationID == null) return images;
            
            // Lấy tên destination từ database
            var destination = _tourService.GetDestinationById(DestinationID.Value);
            if (destination == null) return images;
            
            // Tạo tên thư mục từ tên destination (lowercase, replace spaces with hyphens)
            var folderName = destination.DestinationName?.ToLowerInvariant().Replace(" ", "-").Replace("đ", "d").Replace("ă", "a").Replace("â", "a").Replace("ê", "e").Replace("ô", "o").Replace("ơ", "o").Replace("ư", "u");
            
            if (string.IsNullOrEmpty(folderName)) return images;
            
            var destinationPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "tours", folderName);
            if (Directory.Exists(destinationPath))
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var files = Directory.GetFiles(destinationPath)
                    .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
                    .Select(file => $"/assets/tours/{folderName}/{Path.GetFileName(file)}")
                    .ToList();
                
                images.AddRange(files);
            }
            
            return images;
        }

        // GET: admin/tour-admin/get-images-by-tour/{TourID}
        [HttpGet("get-images-by-tour/{TourID}")]
        public async Task<IActionResult> GetImagesByTour(int TourID)
        {
            try
            {
                var images = GetAvailableImagesByTour(TourID);
                return Json(new { success = true, images = images });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
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
                        
                        // Xử lý ảnh từ thư mục cũ
                        if (trimmedPath.StartsWith("/image/Tourimg/"))
                        {
                            var imageName = Path.GetFileName(trimmedPath);
                            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "Tourimg", imageName);
                            if (System.IO.File.Exists(fullPath))
                            {
                                var mockImage = CreateMockFormFile(fullPath, imageName);
                                allImages.Add(mockImage);
                            }
                        }
                        // Xử lý ảnh từ cấu trúc thư mục mới
                        else if (trimmedPath.StartsWith("/assets/tours/"))
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
                        // Xử lý ảnh chỉ có tên file (từ thư mục cũ)
                        else if (!trimmedPath.Contains("/"))
                        {
                            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "Tourimg", trimmedPath);
                            if (System.IO.File.Exists(fullPath))
                            {
                                var mockImage = CreateMockFormFile(fullPath, trimmedPath);
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

        // POST: admin/tour-admin/update
        [HttpPost("update")]
        public async Task<IActionResult> Update(Tour tour, List<IFormFile> images, string selectedImages)
        {
            try
            {
                if (tour.TourID == 0)
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

                var existingTour = await _tourService.GetTourByIdForAdminAsync(tour.TourID);
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
                    var selectedImagePaths = selectedImages.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var imagePath in selectedImagePaths)
                    {
                        var trimmedPath = imagePath.Trim();
                        
                        // Xử lý ảnh từ thư mục cũ
                        if (trimmedPath.StartsWith("/image/Tourimg/"))
                        {
                            var imageName = Path.GetFileName(trimmedPath);
                            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "Tourimg", imageName);
                            if (System.IO.File.Exists(fullPath))
                            {
                                var mockImage = CreateMockFormFile(fullPath, imageName);
                                allImages.Add(mockImage);
                            }
                        }
                        // Xử lý ảnh từ cấu trúc thư mục mới
                        else if (trimmedPath.StartsWith("/assets/tours/"))
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
                        // Xử lý ảnh chỉ có tên file (từ thư mục cũ)
                        else if (!trimmedPath.Contains("/"))
                        {
                            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "Tourimg", trimmedPath);
                            if (System.IO.File.Exists(fullPath))
                            {
                                var mockImage = CreateMockFormFile(fullPath, trimmedPath);
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
                existingTour.DestinationID = tour.DestinationID;
                existingTour.CategoryID = tour.CategoryID;

                await _tourService.UpdateTourAsync(existingTour, allImages);
                TempData["Success"] = "Tour updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to update tour: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost("toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var tour = await _tourService.GetTourByIdForAdminAsync(id);
                if (tour == null)
                {
                    return NotFound();
                }

                // Toggle status
                tour.Status = !tour.Status;
                await _tourService.UpdateTourAsync(tour, null);
                
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delete-confirm")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            try
            {
                var tour = await _tourService.GetTourByIdForAdminAsync(id);
                if (tour == null)
                {
                    TempData["Error"] = "Tour not found!";
                    return RedirectToAction("Index");
                }

                // Xóa mềm - chỉ set status = false
                tour.Status = false;
                await _tourService.UpdateTourAsync(tour, null);
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