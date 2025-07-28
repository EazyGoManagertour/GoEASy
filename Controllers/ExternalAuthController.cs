using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using GoEASy.Services;
using GoEASy.Models;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoEASy.Controllers
{
    public class ExternalAuthController : Controller
    {
        private readonly LoginService _loginService;

        public ExternalAuthController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet("/signin-google")]
        public IActionResult GoogleLogin()
        {
            var clientId = "825404770214-t0rnrbukqg4ir489d2veatkkfr45jp11.apps.googleusercontent.com";
            var redirectUri = "http://localhost:5233/signin-google-callback";
            var scope = "email profile";
            
            var googleAuthUrl = $"https://accounts.google.com/o/oauth2/v2/auth?" +
                               $"client_id={clientId}&" +
                               $"redirect_uri={Uri.EscapeDataString(redirectUri)}&" +
                               $"scope={Uri.EscapeDataString(scope)}&" +
                               $"response_type=code&" +
                               $"access_type=offline";
            
            return Redirect(googleAuthUrl);
        }

        [HttpGet("/signin-google-callback")]
        public async Task<IActionResult> GoogleCallback(string code, string error)
        {
            // Debug logging
            Console.WriteLine($"Google Callback - Code: {code}, Error: {error}");
            
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine($"Google Error: {error}");
                TempData["Error"] = "Google authentication failed: " + error;
                return RedirectToAction("Index", "Login");
            }

            if (string.IsNullOrEmpty(code))
            {
                Console.WriteLine("No authorization code received");
                TempData["Error"] = "No authorization code received";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                Console.WriteLine("Starting token exchange...");
                
                // Exchange code for access token
                var tokenResponse = await ExchangeCodeForToken(code);
                if (tokenResponse == null)
                {
                    Console.WriteLine("Failed to get access token");
                    TempData["Error"] = "Failed to get access token";
                    return RedirectToAction("Index", "Login");
                }

                Console.WriteLine($"Token received: {tokenResponse.access_token?.Substring(0, 10)}...");

                // Get user info from Google
                var userInfo = await GetGoogleUserInfo(tokenResponse.access_token);
                if (userInfo == null)
                {
                    Console.WriteLine("Failed to get user information");
                    TempData["Error"] = "Failed to get user information";
                    return RedirectToAction("Index", "Login");
                }

                Console.WriteLine($"User info received: {userInfo.email}");

                // Process user login/registration
                var existingUser = await _loginService.GetUserByEmailAsync(userInfo.email);

                if (existingUser != null)
                {
                    SignInUser(existingUser);
                    TempData["Success"] = "Đăng nhập Google thành công!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var newUser = new User
                    {
                        Email = userInfo.email,
                        FullName = userInfo.name ?? "Google User",
                        Username = userInfo.email.Split('@')[0], // Tạo username từ email
                        Password = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                        RoleID = 1,
                        Status = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    var success = await _loginService.RegisterAsync(newUser);
                    
                    if (success)
                    {
                        var user = await _loginService.GetUserByEmailAsync(userInfo.email);
                        SignInUser(user);
                        HttpContext.Session.SetString("NeedCompleteProfile", "true");
                        TempData["Success"] = "Đăng ký Google thành công! Vui lòng hoàn thiện thông tin.";
                        return RedirectToAction("CompleteProfile");
                    }
                    else
                    {
                        TempData["Error"] = "Có lỗi khi tạo tài khoản";
                        return RedirectToAction("Index", "Login");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GoogleCallback: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("Index", "Login");
            }
        }

        private async Task<TokenResponse> ExchangeCodeForToken(string code)
        {
            var clientId = "825404770214-t0rnrbukqg4ir489d2veatkkfr45jp11.apps.googleusercontent.com";
            var clientSecret = "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";
            var redirectUri = "http://localhost:5233/signin-google-callback";

            var tokenRequest = new Dictionary<string, string>
            {
                {"code", code},
                {"client_id", clientId},
                {"client_secret", clientSecret},
                {"redirect_uri", redirectUri},
                {"grant_type", "authorization_code"}
            };

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync("https://oauth2.googleapis.com/token", 
                new FormUrlEncodedContent(tokenRequest));

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TokenResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return null;
        }

        private async Task<GoogleUserInfo> GetGoogleUserInfo(string accessToken)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<GoogleUserInfo>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return null;
        }

        private void SignInUser(User user)
        {
            HttpContext.Session.SetInt32("UserID", user.UserID); // Đổi từ "UserId" thành "UserID"
            HttpContext.Session.SetString("UserEmail", user.Email ?? "");
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserRole", user.Role?.RoleName ?? "User");
            HttpContext.Session.SetInt32("RoleId", user.RoleID ?? 1);
        }

        [HttpGet("/complete-profile")]
        public IActionResult CompleteProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserID"); // Đổi từ "UserId" thành "UserID"
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }
            
            return View("~/Views/client/auth/complete-profile.cshtml");
        }

        [HttpPost("/complete-profile")]
        public async Task<IActionResult> CompleteProfile(string phone, string address, string sex)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            try
            {
                var user = await _loginService.GetUserByEmailAsync(HttpContext.Session.GetString("UserEmail"));
                if (user != null)
                {
                    user.Phone = phone;
                    user.Address = address;
                    user.Sex = sex ?? "Male";
                    user.UpdatedAt = DateTime.Now;
                    
                    await _loginService.UpdateUserAsync(user);
                    HttpContext.Session.Remove("NeedCompleteProfile");
                    
                    TempData["Success"] = "Hoàn thiện thông tin thành công!";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
            }
            
            return View("~/Views/client/auth/complete-profile.cshtml");
        }
    }

    public class TokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }

    public class GoogleUserInfo
    {
        public string id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
    }
}











