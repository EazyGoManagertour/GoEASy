using GoEASy.Filters;
using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    //[AdminAuthorize]
    [Route("admin/destination-admin")]
    public class AdminDestinationController : Controller
    {
        private readonly DestinationService _destinationService;

        public AdminDestinationController(DestinationService destinationService)
        {
            _destinationService = destinationService;
        }

        // GET: admin/destination-admin
        [HttpGet("")]
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 6;
            var destinations = await _destinationService.GetPagedDestinationsAsync(page, pageSize);
            
            // Lấy danh sách ảnh có sẵn trong thư mục destinations
            var availableImages = GetAvailableImages();
            ViewBag.AvailableImages = availableImages;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = await _destinationService.GetTotalPagesAsync(pageSize);
            
            return View("~/Views/admin/destination_admin/DestinationAdmin.cshtml", destinations);
        }

        private List<string> GetAvailableImages()
        {
            var images = new List<string>();
            var destinationImgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images", "destinations");
            
            if (Directory.Exists(destinationImgPath))
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var files = Directory.GetFiles(destinationImgPath)
                    .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
                    .Select(file => "/assets/images/destinations/" + Path.GetFileName(file))
                    .ToList();
                
                images.AddRange(files);
            }
            
            return images;
        }

        // POST: admin/destination-admin/create
        [HttpPost("create")]
        public async Task<IActionResult> Create(Destination destination, List<IFormFile> images, string selectedImages)
        {
            try
            {
                var (isValid, errors) = ValidationService.ValidateDestination(destination);

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
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images", "destinations", imageName.Trim());
                        if (System.IO.File.Exists(imagePath))
                        {
                            // Create a mock IFormFile from existing file
                            var mockImage = CreateMockFormFile(imagePath, imageName);
                            if (mockImage != null)
                            {
                                allImages.Add(mockImage);
                            }
                        }
                    }
                }

                await _destinationService.AddDestinationAsync(destination, allImages);
                TempData["Success"] = "Destination created successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to create destination: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        private IFormFile CreateMockFormFile(string filePath, string fileName)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                if (!fileInfo.Exists)
                {
                    return null;
                }

                // Đọc toàn bộ file vào memory stream
                var memoryStream = new MemoryStream();
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    fileStream.CopyTo(memoryStream);
                }
                memoryStream.Position = 0;

                return new FormFile(memoryStream, 0, memoryStream.Length, "images", fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = GetContentType(fileName)
                };
            }
            catch (Exception ex)
            {
                // Log error if needed
                return null;
            }
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

        // POST: admin/destination-admin/update
        [HttpPost("update")]
        public async Task<IActionResult> Update(Destination destination, List<IFormFile> images, string selectedImages)
        {
            try
            {
                if (destination.DestinationID == 0)
                {
                    TempData["Error"] = "Destination ID is required!";
                    return RedirectToAction("Index");
                }

                var (isValid, errors) = ValidationService.ValidateDestination(destination, isUpdate: true);

                if (!isValid)
                {
                    TempData["Error"] = string.Join("<br/>", errors);
                    return RedirectToAction("Index");
                }

                var existingDestination = await _destinationService.GetDestinationByIdAsync(destination.DestinationID);
                if (existingDestination == null)
                {
                    TempData["Error"] = "Destination not found!";
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
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images", "destinations", imageName.Trim());
                        if (System.IO.File.Exists(imagePath))
                        {
                            // Create a mock IFormFile from existing file
                            var mockImage = CreateMockFormFile(imagePath, imageName);
                            if (mockImage != null)
                            {
                                allImages.Add(mockImage);
                            }
                        }
                    }
                }

                // Cập nhật thông tin
                existingDestination.DestinationName = destination.DestinationName;
                existingDestination.Description = destination.Description;
                existingDestination.Location = destination.Location;
                existingDestination.Status = destination.Status;

                await _destinationService.UpdateDestinationAsync(existingDestination, allImages);
                TempData["Success"] = "Destination updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to update destination: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost("delete-confirm")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            try
            {
                var destination = await _destinationService.GetDestinationByIdAsync(id);
                if (destination == null)
                {
                    TempData["Error"] = "Destination not found!";
                    return RedirectToAction("Index");
                }

                await _destinationService.DeleteDestinationAsync(id);
                TempData["Success"] = "Destination deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to delete destination: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
} 