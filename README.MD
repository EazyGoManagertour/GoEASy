# Cài EF tool nếu chưa có
dotnet tool install --global dotnet-ef

# Khôi phục packages (nếu cần)
dotnet restore

# Tạo database và schema
dotnet ef database update
