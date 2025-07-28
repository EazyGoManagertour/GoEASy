-- Script để cập nhật lại dữ liệu policy với encoding UTF-8 đúng
-- Xóa dữ liệu cũ trước
DELETE FROM TourPolicies;

-- Thêm lại dữ liệu với encoding UTF-8 đúng
-- Sử dụng N prefix để đảm bảo Unicode
INSERT INTO TourPolicies (TourID, PolicyType, PolicyName, PolicyDescription, PolicyValue, IsActive, CreatedAt)
VALUES
-- Tour 1: Hạ Long 2N1D
(1, N'ChildPolicy', N'Chính sách trẻ em', N'Phải có ít nhất 1 người lớn cho mỗi 3 trẻ em',
N'{"MinAdultsPerChild": 1, "MaxChildrenPerAdult": 3, "ChildAgeLimit": 18, "Message": "Phải có ít nhất 1 người lớn cho mỗi 3 trẻ em"}',
1, GETDATE()),
(1, N'CancellationPolicy', N'Chính sách hủy tour', N'Hủy trước 5 ngày được hoàn 100% tiền',
N'{"RefundDays": 5, "RefundPercentage": 100, "Message": "Hủy trước 5 ngày được hoàn 100% tiền"}',
1, GETDATE()),

-- Tour 2: Hà Nội
(2, N'ChildPolicy', N'Chính sách trẻ em', N'Phải có ít nhất 1 người lớn cho mỗi 3 trẻ em',
N'{"MinAdultsPerChild": 1, "MaxChildrenPerAdult": 3, "ChildAgeLimit": 18, "Message": "Phải có ít nhất 1 người lớn cho mỗi 3 trẻ em"}',
1, GETDATE()),
(2, N'CancellationPolicy', N'Chính sách hủy tour', N'Hủy trước 5 ngày được hoàn 100% tiền',
N'{"RefundDays": 5, "RefundPercentage": 100, "Message": "Hủy trước 5 ngày được hoàn 100% tiền"}',
1, GETDATE()),

-- Tour 3: Khám phá Đà Nẵng 3N2D
(3, N'ChildPolicy', N'Chính sách trẻ em', N'Phải có ít nhất 1 người lớn cho mỗi 3 trẻ em',
N'{"MinAdultsPerChild": 1, "MaxChildrenPerAdult": 3, "ChildAgeLimit": 18, "Message": "Phải có ít nhất 1 người lớn cho mỗi 3 trẻ em"}',
1, GETDATE()),
(3, N'CancellationPolicy', N'Chính sách hủy tour', N'Hủy trước 5 ngày được hoàn 100% tiền',
N'{"RefundDays": 5, "RefundPercentage": 100, "Message": "Hủy trước 5 ngày được hoàn 100% tiền"}',
1, GETDATE()),

-- Tour 4: Nghỉ dưỡng Phú Quốc 5N4D
(4, N'ChildPolicy', N'Chính sách trẻ em', N'Phải có ít nhất 1 người lớn cho mỗi 3 trẻ em',
N'{"MinAdultsPerChild": 1, "MaxChildrenPerAdult": 3, "ChildAgeLimit": 18, "Message": "Phải có ít nhất 1 người lớn cho mỗi 3 trẻ em"}',
1, GETDATE()),
(4, N'CancellationPolicy', N'Chính sách hủy tour', N'Hủy trước 5 ngày được hoàn 100% tiền',
N'{"RefundDays": 5, "RefundPercentage": 100, "Message": "Hủy trước 5 ngày được hoàn 100% tiền"}',
1, GETDATE()),

-- Tour 5: Tour Hạ Long 3 ngày 2 đêm
(5, N'ChildPolicy', N'Chính sách trẻ em', N'Phải có ít nhất 1 người lớn cho mỗi 3 trẻ em',
N'{"MinAdultsPerChild": 1, "MaxChildrenPerAdult": 3, "ChildAgeLimit": 18, "Message": "Phải có ít nhất 1 người lớn cho mỗi 3 trẻ em"}',
1, GETDATE()),
(5, N'CancellationPolicy', N'Chính sách hủy tour', N'Hủy trước 5 ngày được hoàn 100% tiền',
N'{"RefundDays": 5, "RefundPercentage": 100, "Message": "Hủy trước 5 ngày được hoàn 100% tiền"}',
1, GETDATE()),

-- Tour 6: Trekking Sa Pa 4N3D (chính sách khác biệt)
(6, N'ChildPolicy', N'Chính sách trẻ em', N'Phải có ít nhất 2 người lớn cho mỗi 5 trẻ em (trekking khó)',
N'{"MinAdultsPerChild": 2, "MaxChildrenPerAdult": 2.5, "ChildAgeLimit": 18, "Message": "Phải có ít nhất 2 người lớn cho mỗi 5 trẻ em (trekking khó)"}',
1, GETDATE()),
(6, N'CancellationPolicy', N'Chính sách hủy tour', N'Hủy trước 7 ngày được hoàn 100%, trước 3 ngày được hoàn 50%',
N'{"RefundDays": 7, "RefundPercentage": 100, "Message": "Hủy trước 7 ngày được hoàn 100%, trước 3 ngày được hoàn 50%"}',
1, GETDATE());

-- Kiểm tra dữ liệu sau khi insert
SELECT TourID, PolicyType, PolicyDescription, PolicyValue FROM TourPolicies ORDER BY TourID, PolicyType; 