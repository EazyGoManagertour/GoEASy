using GoEASy.Filters;
using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    [AdminAuthorize]
    [Route("admin/tour-admin")]
    public class AdminTourController : Controller
    {
        private readonly TourService _tourService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AdminTourController(TourService tourService, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _tourService = tourService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // GET: admin/tour-admin
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var tours = await _tourService.GetAllToursForAdminAsync();
            // Lấy danh sách category và destination
            var categories = await _tourService.GetAllCategoriesAsync();
            var destinations = await _tourService.GetAllDestinationsAsync();
            // Lấy danh sách ảnh có sẵn trong thư mục Tourimg
            var availableImages = GetAvailableImages();
            ViewBag.AvailableImages = availableImages;
            ViewBag.Categories = categories;
            ViewBag.Destinations = destinations;
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

        private List<string> GetAvailableImagesByDestination(int? destinationID)
        {
            var images = new List<string>();
            if (destinationID == null) return images;
            // Lấy tên destination từ database
            var destination = _tourService.GetDestinationById(destinationID.Value);
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

        // GET: admin/tour-admin/get-images-by-tour/{tourID}
        [HttpGet("get-images-by-tour/{tourID}")]
        public async Task<IActionResult> GetImagesByTour(int tourID)
        {
            try
            {
                var images = GetAvailableImagesByTour(tourID);
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
                // Lấy các trường TourDetail từ form
                var included = Request.Form["Included"].ToString();
                var excluded = Request.Form["Excluded"].ToString();
                var activities = Request.Form["Activities"].ToString();
                // Lấy các trường Itinerary
                var days = Request.Form["addItineraryDay[]"].ToArray();
                var titles = Request.Form["addItineraryTitle[]"].ToArray();
                var descs = Request.Form["addItineraryDescription[]"].ToArray();
                var accs = Request.Form["addItineraryAccommodation[]"].ToArray();
                var meals = Request.Form["addItineraryMeals[]"].ToArray();
                // Lấy các trường FAQ
                var faqQs = Request.Form["addFAQQuestion[]"].ToArray();
                var faqAs = Request.Form["addFAQAnswer[]"].ToArray();
                // Validate tour như cũ
                var (isValid, errors) = ValidationService.ValidateTour(tour);
                if (!isValid)
                {
                    TempData["Error"] = string.Join("<br/>", errors);
                    return RedirectToAction("Index");
                }
                // Validate ảnh như cũ
                var allImages = new List<IFormFile>();
                if (images != null) allImages.AddRange(images);
                if (!string.IsNullOrEmpty(selectedImages))
                {
                    var selectedImagePaths = selectedImages.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var imagePath in selectedImagePaths)
                    {
                        var trimmedPath = imagePath.Trim();
                        if (trimmedPath.StartsWith("/image/Tourimg/"))
                        {
                            var imageName = Path.GetFileName(trimmedPath);
                            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "Tourimg", imageName);
                            if (System.IO.File.Exists(fullPath))
                                allImages.Add(CreateMockFormFile(fullPath, imageName));
                        }
                        else if (trimmedPath.StartsWith("/assets/tours/"))
                        {
                            var relativePath = trimmedPath.TrimStart('/');
                            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);
                            if (System.IO.File.Exists(fullPath))
                                allImages.Add(CreateMockFormFile(fullPath, Path.GetFileName(trimmedPath)));
                        }
                        else if (!trimmedPath.Contains("/"))
                        {
                            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "Tourimg", trimmedPath);
                            if (System.IO.File.Exists(fullPath))
                                allImages.Add(CreateMockFormFile(fullPath, trimmedPath));
                        }
                    }
                }
                // Lưu tour trước để lấy TourID
                await _tourService.AddTourAsync(tour, allImages);
                // Lưu TourDetail
                var detail = new TourDetail
                {
                    TourID = tour.TourID,
                    Included = included,
                    Excluded = excluded,
                    Activities = activities,
                    CreatedAt = DateTime.Now
                };
                using (var db = new GoEasyContext())
                {
                    db.TourDetails.Add(detail);
                    await db.SaveChangesAsync();
                    // Lưu Itinerary
                    for (int i = 0; i < days.Length; i++)
                    {
                        db.TourItineraries.Add(new TourItinerary
                        {
                            TourID = tour.TourID,
                            DayNumber = int.TryParse(days[i], out var d) ? d : (int?)null,
                            Title = titles.ElementAtOrDefault(i),
                            Description = descs.ElementAtOrDefault(i),
                            Accommodation = accs.ElementAtOrDefault(i),
                            Meals = meals.ElementAtOrDefault(i),
                            CreatedAt = DateTime.Now
                        });
                    }
                    // Lưu FAQ
                    for (int i = 0; i < faqQs.Length; i++)
                    {
                        db.TourFAQs.Add(new TourFAQ
                        {
                            TourID = tour.TourID,
                            Question = faqQs.ElementAtOrDefault(i),
                            Answer = faqAs.ElementAtOrDefault(i),
                            CreatedAt = DateTime.Now
                        });
                    }
                    await db.SaveChangesAsync();
                }
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
                // Lấy các trường TourDetail từ form
                var included = Request.Form["Included"].ToString();
                var excluded = Request.Form["Excluded"].ToString();
                var activities = Request.Form["Activities"].ToString();
                // Lấy các trường Itinerary
                var days = Request.Form["editItineraryDay[]"].ToArray();
                var titles = Request.Form["editItineraryTitle[]"].ToArray();
                var descs = Request.Form["editItineraryDescription[]"].ToArray();
                var accs = Request.Form["editItineraryAccommodation[]"].ToArray();
                var meals = Request.Form["editItineraryMeals[]"].ToArray();
                // Lấy các trường FAQ
                var faqQs = Request.Form["editFAQQuestion[]"].ToArray();
                var faqAs = Request.Form["editFAQAnswer[]"].ToArray();
                // Validate ảnh như cũ
                var allImages = new List<IFormFile>();
                if (images != null) allImages.AddRange(images);
                if (!string.IsNullOrEmpty(selectedImages))
                {
                    var selectedImagePaths = selectedImages.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var imagePath in selectedImagePaths)
                    {
                        var trimmedPath = imagePath.Trim();
                        if (trimmedPath.StartsWith("/image/Tourimg/"))
                        {
                            var imageName = Path.GetFileName(trimmedPath);
                            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "Tourimg", imageName);
                            if (System.IO.File.Exists(fullPath))
                                allImages.Add(CreateMockFormFile(fullPath, imageName));
                        }
                        else if (trimmedPath.StartsWith("/assets/tours/"))
                        {
                            var relativePath = trimmedPath.TrimStart('/');
                            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);
                            if (System.IO.File.Exists(fullPath))
                                allImages.Add(CreateMockFormFile(fullPath, Path.GetFileName(trimmedPath)));
                        }
                        else if (!trimmedPath.Contains("/"))
                        {
                            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "Tourimg", trimmedPath);
                            if (System.IO.File.Exists(fullPath))
                                allImages.Add(CreateMockFormFile(fullPath, trimmedPath));
                        }
                    }
                }
                // Cập nhật thông tin tour
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
                // Update TourDetail
                using (var db = new GoEasyContext())
                {
                    var detail = db.TourDetails.FirstOrDefault(x => x.TourID == tour.TourID);
                    if (detail == null)
                    {
                        detail = new TourDetail { TourID = tour.TourID, CreatedAt = DateTime.Now };
                        db.TourDetails.Add(detail);
                    }
                    detail.Included = included;
                    detail.Excluded = excluded;
                    detail.Activities = activities;
                    // Xóa hết itinerary và FAQ cũ
                    var oldItineraries = db.TourItineraries.Where(x => x.TourID == tour.TourID);
                    db.TourItineraries.RemoveRange(oldItineraries);
                    var oldFaqs = db.TourFAQs.Where(x => x.TourID == tour.TourID);
                    db.TourFAQs.RemoveRange(oldFaqs);
                    await db.SaveChangesAsync();
                    // Lưu Itinerary mới
                    for (int i = 0; i < days.Length; i++)
                    {
                        db.TourItineraries.Add(new TourItinerary
                        {
                            TourID = tour.TourID,
                            DayNumber = int.TryParse(days[i], out var d) ? d : (int?)null,
                            Title = titles.ElementAtOrDefault(i),
                            Description = descs.ElementAtOrDefault(i),
                            Accommodation = accs.ElementAtOrDefault(i),
                            Meals = meals.ElementAtOrDefault(i),
                            CreatedAt = DateTime.Now
                        });
                    }
                    // Lưu FAQ mới
                    for (int i = 0; i < faqQs.Length; i++)
                    {
                        db.TourFAQs.Add(new TourFAQ
                        {
                            TourID = tour.TourID,
                            Question = faqQs.ElementAtOrDefault(i),
                            Answer = faqAs.ElementAtOrDefault(i),
                            CreatedAt = DateTime.Now
                        });
                    }
                    await db.SaveChangesAsync();
                }
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

        [HttpPost("generate-description")]
        public async Task<IActionResult> GenerateDescription([FromBody] GenerateDescriptionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.TourName))
            {
                return BadRequest(new { message = "Tour name is required." });
            }
            var apiKey = _configuration["OpenAI:ApiKey"];
            var endpoint = _configuration["OpenAI:Endpoint"];
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(endpoint))
            {
                return BadRequest(new { message = "OpenAI configuration is missing." });
            }
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
                // Tạo prompt chi tiết hơn
                var prompt = $"" +
                    $"Hãy viết một đoạn mô tả du lịch thật hấp dẫn, chuyên nghiệp và chuẩn SEO cho tour '{request.TourName}'. " +
                    $"\n\nCác thông tin sau đây sẽ giúp bạn mô tả chính xác hơn:\n" +
                    (string.IsNullOrWhiteSpace(request.Included) ? "" : $"Bao gồm: {request.Included}\n") +
                    (string.IsNullOrWhiteSpace(request.Excluded) ? "" : $"Không bao gồm: {request.Excluded}\n") +
                    (string.IsNullOrWhiteSpace(request.Activities) ? "" : $"Hoạt động nổi bật: {request.Activities}\n") +
                    (request.Itinerary != null && request.Itinerary.Count > 0 ? $"Lịch trình tour:\n{string.Join("\n", request.Itinerary)}\n" : "") +
                    $"\nĐoạn mô tả nên dài khoảng 3-5 câu, nêu bật những điểm đặc sắc nhất, và kết thúc bằng một lời kêu gọi hành động mạnh mẽ để khuyến khích khách hàng đặt tour.";
                var payload = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                        new { role = "system", content = "Bạn là một chuyên gia viết nội dung du lịch chuyên nghiệp." },
                        new { role = "user", content = prompt }
                    },
                    temperature = 0.7,
                    max_tokens = 700
                };
                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(endpoint, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    using var jsonDoc = JsonDocument.Parse(responseBody);
                    var generatedDescription = jsonDoc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                    return Ok(new { description = generatedDescription?.Trim() });
                }
                else
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, new { message = "Failed to generate description from OpenAI.", details = errorBody });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An internal error occurred.", details = ex.Message });
            }
        }

        public class GenerateDescriptionRequest
        {
            public string TourName { get; set; }
            public string Included { get; set; }
            public string Excluded { get; set; }
            public string Activities { get; set; }
            public List<string> Itinerary { get; set; }
        }

        [HttpGet("get-tour-details/{tourID}")]
        public async Task<IActionResult> GetTourDetails(int tourID)
        {
            try
            {
                var tour = await _tourService.GetTourByIdForAdminAsync(tourID);
                if (tour == null)
                {
                    return Json(new { success = false, message = "Tour not found" });
                }

                // Lấy Included, Excluded, Activities từ TourDetail
                var included = new List<string>();
                var excluded = new List<string>();
                var activities = new List<string>();

                // Lấy TourDetail
                var tourDetail = await _tourService.GetTourDetailAsync(tourID);
                if (tourDetail != null)
                {
                    // Parse Included - tách bằng dấu phẩy
                    if (!string.IsNullOrEmpty(tourDetail.Included))
                    {
                        included = tourDetail.Included.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(item => item.Trim())
                            .Where(item => !string.IsNullOrEmpty(item))
                            .ToList();
                    }

                    // Parse Excluded - tách bằng dấu phẩy
                    if (!string.IsNullOrEmpty(tourDetail.Excluded))
                    {
                        excluded = tourDetail.Excluded.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(item => item.Trim())
                            .Where(item => !string.IsNullOrEmpty(item))
                            .ToList();
                    }

                    // Parse Activities - tách bằng dấu phẩy
                    if (!string.IsNullOrEmpty(tourDetail.Activities))
                    {
                        activities = tourDetail.Activities.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(item => item.Trim())
                            .Where(item => !string.IsNullOrEmpty(item))
                            .ToList();
                    }
                }

                // Lấy Itinerary từ database
                var itinerary = await _tourService.GetTourItinerariesAsync(tourID);

                // Lấy FAQ từ database
                var faqs = await _tourService.GetTourFAQsAsync(tourID);

                return Json(new
                {
                    success = true,
                    included = included,
                    excluded = excluded,
                    activities = activities,
                    itinerary = itinerary?.Select(i => new
                    {
                        dayNumber = i.DayNumber,
                        title = i.Title,
                        description = i.Description,
                        accommodation = i.Accommodation,
                        meals = i.Meals
                    }),
                    faqs = faqs?.Select(f => new
                    {
                        question = f.Question,
                        answer = f.Answer
                    })
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}