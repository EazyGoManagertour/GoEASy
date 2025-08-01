using System.Text.RegularExpressions;
using GoEASy.Models;

namespace GoEASy.Services
{
    public class ValidationService
    {
        // Kiểm tra email hợp lệ
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return true; // Email có thể null/empty

            try
            {
                var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
                return emailRegex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        // Kiểm tra số điện thoại Việt Nam
        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return true; // Phone có thể null/empty

            try
            {
                // Loại bỏ khoảng trắng và ký tự đặc biệt
                var cleanPhone = Regex.Replace(phone, @"[^\d]", "");
                
                // Kiểm tra format số điện thoại Việt Nam
                var phoneRegex = new Regex(@"^(0|\+84)(3[2-9]|5[689]|7[06-9]|8[1-689]|9[0-46-9])[0-9]{7}$");
                return phoneRegex.IsMatch(cleanPhone);
            }
            catch
            {
                return false;
            }
        }

        // Kiểm tra tên có viết hoa đầu chữ không
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            try
            {
                // Kiểm tra mỗi từ phải viết hoa đầu chữ
                var words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    if (word.Length == 0 || !char.IsUpper(word[0]))
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Kiểm tra username hợp lệ
        public static bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            try
            {
                // Username chỉ chứa chữ cái, số, dấu gạch dưới, độ dài 3-20 ký tự
                var usernameRegex = new Regex(@"^[a-zA-Z0-9_]{3,20}$");
                return usernameRegex.IsMatch(username);
            }
            catch
            {
                return false;
            }
        }

        // Kiểm tra password mạnh
        public static bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            try
            {
                // Ít nhất 8 ký tự, có chữ hoa, chữ thường, số
                var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");
                return passwordRegex.IsMatch(password);
            }
            catch
            {
                return false;
            }
        }

        // Kiểm tra VIPPoints hợp lệ
        public static bool IsValidVIPPoints(int? VIPPoints)
        {
            return VIPPoints == null || VIPPoints >= 0;
        }

        // Validate toàn bộ User object
        public static (bool IsValid, List<string> Errors) ValidateUser(User user, bool isUpdate = false)
        {
            var errors = new List<string>();

            // Username validation
            if (!IsValidUsername(user.Username))
            {
                errors.Add("Username phải có 3-20 ký tự, chỉ chứa chữ cái, số và dấu gạch dưới");
            }

            // Password validation (chỉ khi thêm mới hoặc có thay đổi)
            if (!isUpdate || !string.IsNullOrEmpty(user.Password))
            {
                if (!IsStrongPassword(user.Password))
                {
                    errors.Add("Password phải có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường và số");
                }
            }

            // FullName validation
            if (!IsValidName(user.FullName))
            {
                errors.Add("Họ tên phải viết hoa đầu mỗi từ");
            }

            // Email validation
            if (!string.IsNullOrEmpty(user.Email) && !IsValidEmail(user.Email))
            {
                errors.Add("Email không hợp lệ");
            }

            // Phone validation
            if (!string.IsNullOrEmpty(user.Phone) && !IsValidPhone(user.Phone))
            {
                errors.Add("Số điện thoại không hợp lệ (định dạng Việt Nam)");
            }

            // VIPPoints validation
            if (!IsValidVIPPoints(user.VIPPoints))
            {
                errors.Add("VIPPoints phải là số không âm");
            }

            return (errors.Count == 0, errors);
        }

        // Format số điện thoại
        public static string FormatPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return phone;

            var cleanPhone = Regex.Replace(phone, @"[^\d]", "");
            
            if (cleanPhone.StartsWith("84"))
                cleanPhone = "0" + cleanPhone.Substring(2);
            
            return cleanPhone;
        }

        // Format tên (viết hoa đầu chữ)
        public static string FormatName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return name;

            var words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var formattedWords = words.Select(word => 
                word.Length > 0 ? char.ToUpper(word[0]) + word.Substring(1).ToLower() : word
            );
            
            return string.Join(" ", formattedWords);
        }


        public static (bool IsValid, List<string> Errors) ValidateAdmin(Admin admin, bool isUpdate = false)
        {
            var errors = new List<string>();

            if (!IsValidUsername(admin.Username))
                errors.Add("Username phải có 3-20 ký tự, chỉ chứa chữ cái, số và dấu gạch dưới");

            if (!isUpdate || !string.IsNullOrEmpty(admin.Password))
            {
                if (!IsStrongPassword(admin.Password))
                    errors.Add("Password phải có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường và số");
            }

            if (!IsValidName(admin.FullName))
                errors.Add("Họ tên phải viết hoa đầu mỗi từ");

            if (!string.IsNullOrEmpty(admin.Email) && !IsValidEmail(admin.Email))
                errors.Add("Email không hợp lệ");

            if (!string.IsNullOrEmpty(admin.Phone) && !IsValidPhone(admin.Phone))
                errors.Add("Số điện thoại không hợp lệ (định dạng Việt Nam)");

            if (string.IsNullOrWhiteSpace(admin.Role) || !(admin.Role == "Admin" || admin.Role == "Employee"))
                errors.Add("Role phải là Admin hoặc Employee");

            return (errors.Count == 0, errors);
        }

        // Validate Tour object
        public static (bool IsValid, List<string> Errors) ValidateTour(Tour tour, bool isUpdate = false)
        {
            var errors = new List<string>();

            // TourName validation
            if (string.IsNullOrWhiteSpace(tour.TourName))
            {
                errors.Add("Tên tour không được để trống");
            }
            else if (tour.TourName.Length < 3)
            {
                errors.Add("Tên tour phải có ít nhất 3 ký tự");
            }
            else if (tour.TourName.Length > 100)
            {
                errors.Add("Tên tour không được quá 100 ký tự");
            }

            // Description validation
            if (!string.IsNullOrWhiteSpace(tour.Description) && tour.Description.Length > 1000)
            {
                errors.Add("Mô tả tour không được quá 1000 ký tự");
            }

            // Price validation
            if (tour.AdultPrice <= 0)
            {
                errors.Add("Giá người lớn phải lớn hơn 0");
            }

            if (tour.ChildPrice <= 0)
            {
                errors.Add("Giá trẻ em phải lớn hơn 0");
            }

            if (tour.ChildPrice > tour.AdultPrice)
            {
                errors.Add("Giá trẻ em không được cao hơn giá người lớn");
            }

            // Date validation
            if (tour.StartDate.HasValue && tour.EndDate.HasValue)
            {
                if (tour.StartDate >= tour.EndDate)
                {
                    errors.Add("Ngày bắt đầu phải trước ngày kết thúc");
                }
            }

            if (tour.StartDate.HasValue && tour.StartDate < DateTime.Today)
            {
                errors.Add("Ngày bắt đầu không được trong quá khứ");
            }

            // Seats validation
            if (tour.MaxSeats.HasValue && tour.MaxSeats <= 0)
            {
                errors.Add("Số chỗ ngồi tối đa phải lớn hơn 0");
            }

            if (tour.AvailableSeats.HasValue && tour.AvailableSeats < 0)
            {
                errors.Add("Số chỗ ngồi có sẵn không được âm");
            }

            if (tour.MaxSeats.HasValue && tour.AvailableSeats.HasValue && tour.AvailableSeats > tour.MaxSeats)
            {
                errors.Add("Số chỗ ngồi có sẵn không được vượt quá số chỗ ngồi tối đa");
            }

            // Destination and Category validation
            if (tour.DestinationID.HasValue && tour.DestinationID <= 0)
            {
                errors.Add("ID điểm đến không hợp lệ");
            }

            if (tour.CategoryID.HasValue && tour.CategoryID <= 0)
            {
                errors.Add("ID danh mục không hợp lệ");
            }

            return (errors.Count == 0, errors);
        }
        public static (bool IsValid, List<string> Errors) ValidateDestination(Destination destination, bool isUpdate = false)
        {
            var errors = new List<string>();

            // DestinationName validation
            if (string.IsNullOrWhiteSpace(destination.DestinationName))
            {
                errors.Add("Tên điểm đến không được để trống");
            }
            else if (destination.DestinationName.Length < 3)
            {
                errors.Add("Tên điểm đến phải có ít nhất 3 ký tự");
            }
            else if (destination.DestinationName.Length > 100)
            {
                errors.Add("Tên điểm đến không được quá 100 ký tự");
            }

            // Description validation
            if (!string.IsNullOrWhiteSpace(destination.Description) && destination.Description.Length > 1000)
            {
                errors.Add("Mô tả điểm đến không được quá 1000 ký tự");
            }

            // Location validation
            if (!string.IsNullOrWhiteSpace(destination.Location) && destination.Location.Length > 200)
            {
                errors.Add("Địa điểm không được quá 200 ký tự");
            }

            return (errors.Count == 0, errors);
        }

    }
} 