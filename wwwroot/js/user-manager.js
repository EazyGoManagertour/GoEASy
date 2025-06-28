// Load roles khi trang load
document.addEventListener('DOMContentLoaded', function() {
    loadRoles();
});

// Load danh sách roles
async function loadRoles() {
    try {
        console.log('Loading roles...');
        const response = await fetch('/admin/user-manager/roles');
        console.log('Response status:', response.status);
        
        if (!response.ok) {
            console.error('Failed to load roles:', response.statusText);
            return;
        }
        
        const roles = await response.json();
        console.log('Roles data:', roles);
        
        const addRoleSelect = document.getElementById('addRole');
        const editRoleSelect = document.getElementById('editRole');
        
        // Clear existing options
        addRoleSelect.innerHTML = '';
        editRoleSelect.innerHTML = '';
        
        // Add options
        if (roles && Array.isArray(roles)) {
            roles.forEach(role => {
                console.log('Processing role:', role);
                addRoleSelect.innerHTML += `<option value="${role.roleId || role.RoleId}">${role.roleName || role.RoleName || 'Unknown'}</option>`;
                editRoleSelect.innerHTML += `<option value="${role.roleId || role.RoleId}">${role.roleName || role.RoleName || 'Unknown'}</option>`;
            });
        } else {
            console.warn('Roles data is not an array:', roles);
        }
    } catch (error) {
        console.error('Error loading roles:', error);
    }
}

// Hiển thị modal edit với thông tin user
async function showEditModal(userId) {
    try {
        const response = await fetch(`/admin/user-manager/get/${userId}`);
        if (!response.ok) {
            alert('Không thể lấy thông tin user!');
            return;
        }
        
        const user = await response.json();
        
        // Fill form với thông tin user
        document.getElementById('editUserId').value = user.userId;
        document.getElementById('editUsername').value = user.username;
        document.getElementById('editFullName').value = user.fullName;
        document.getElementById('editEmail').value = user.email || '';
        document.getElementById('editPhone').value = user.phone || '';
        document.getElementById('editAddress').value = user.address || '';
        document.getElementById('editIsVip').value = user.isVip ? 'true' : 'false';
        document.getElementById('editVippoints').value = user.vippoints || 0;
        document.getElementById('editStatus').value = user.status ? 'true' : 'false';
        
        // Clear password field
        document.getElementById('editPassword').value = '';
        
        // Set role
        const editRoleSelect = document.getElementById('editRole');
        if (user.roleId) {
            editRoleSelect.value = user.roleId;
        }
        
        // Show modal
        const editModal = new bootstrap.Modal(document.getElementById('editModal'));
        editModal.show();
        
    } catch (error) {
        console.error('Error loading user data:', error);
        alert('Có lỗi khi tải thông tin user!');
    }
}

// Xử lý form thêm user
document.getElementById('addUserForm').addEventListener('submit', async function(e) {
    e.preventDefault();
    
    // Validation
    const roleId = document.getElementById('addRole').value;
    if (!roleId) {
        alert('Vui lòng chọn Role!');
        return;
    }
    
    const formData = new FormData(this);
    const userData = Object.fromEntries(formData.entries());
    
    try {
        const response = await fetch('/admin/user-manager/add', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userData)
        });
        
        const result = await response.json();
        
        if (result.success) {
            alert(result.message);
            location.reload(); // Reload trang để hiển thị user mới
        } else {
            alert(result.message || 'Có lỗi xảy ra!');
        }
    } catch (error) {
        console.error('Error adding user:', error);
        alert('Có lỗi khi thêm user!');
    }
});

// Xử lý form cập nhật user
document.getElementById('editUserForm').addEventListener('submit', async function(e) {
    e.preventDefault();
    
    // Validation
    const roleId = document.getElementById('editRole').value;
    if (!roleId) {
        alert('Vui lòng chọn Role!');
        return;
    }

    const username = document.getElementById('editUsername').value;
    if (!username.trim()) {
        alert('Username không được để trống!');
        return;
    }
    
    const formData = new FormData(this);
    const userData = {};
    
    // Convert FormData to object with proper handling
    for (let entry of formData.entries()) {
        let key = entry[0];
        let value = entry[1];
        
        // Bỏ qua password field nếu trống
        if (key === 'Password' && value === '') {
            continue;
        }
        
        if (value === '') {
            userData[key] = null; // Convert empty string to null
        } else {
            userData[key] = value;
        }
    }

    // Convert string values to proper types
    if (userData.IsVip !== null) {
        userData.IsVip = userData.IsVip === 'true';
    }
    if (userData.Status !== null) {
        userData.Status = userData.Status === 'true';
    }
    if (userData.Vippoints !== null) {
        userData.Vippoints = parseInt(userData.Vippoints) || 0;
    }
    if (userData.RoleId !== null) {
        userData.RoleId = parseInt(userData.RoleId);
    }

    const userId = userData.UserId;
    
    try {
        console.log('Sending user data:', userData);
        
        const response = await fetch(`/admin/user-manager/update/${userId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userData)
        });
        
        const result = await response.json();
        console.log('Response:', result);
        
        if (result.success) {
            alert(result.message);
            location.reload(); // Reload trang để hiển thị thay đổi
        } else {
            alert(result.message || 'Có lỗi xảy ra!');
        }
    } catch (error) {
        console.error('Error updating user:', error);
        alert('Có lỗi khi cập nhật user!');
    }
});

// Xóa user
async function deleteUser(userId) {
    if (!confirm('Bạn có chắc chắn muốn xóa user này?')) {
        return;
    }
    
    try {
        const response = await fetch(`/admin/user-manager/delete/${userId}`, {
            method: 'DELETE'
        });
        
        const result = await response.json();
        
        if (result.success) {
            alert(result.message);
            location.reload();
        } else {
            alert(result.message || 'Có lỗi xảy ra!');
        }
    } catch (error) {
        console.error('Error deleting user:', error);
        alert('Có lỗi khi xóa user!');
    }
}

// Toggle status user
async function toggleStatus(userId) {
    try {
        const response = await fetch(`/admin/user-manager/toggle-status/${userId}`, {
            method: 'POST'
        });
        
        const result = await response.json();
        
        if (result.success) {
            alert(result.message);
            location.reload();
        } else {
            alert(result.message || 'Có lỗi xảy ra!');
        }
    } catch (error) {
        console.error('Error toggling status:', error);
        alert('Có lỗi khi thay đổi trạng thái!');
    }
}

// Client-side validation functions
function validateEmail(email) {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailRegex.test(email);
}

function validatePhone(phone) {
    if (!phone) return true; // Phone có thể trống
    const cleanPhone = phone.replace(/[^\d]/g, '');
    const phoneRegex = /^(0|\+84)(3[2-9]|5[689]|7[06-9]|8[1-689]|9[0-46-9])[0-9]{7}$/;
    return phoneRegex.test(cleanPhone);
}

function validateName(name) {
    if (!name) return false;
    const words = name.split(' ').filter(word => word.length > 0);
    return words.every(word => word.charAt(0) === word.charAt(0).toUpperCase());
}

function validateUsername(username) {
    const usernameRegex = /^[a-zA-Z0-9_]{3,20}$/;
    return usernameRegex.test(username);
}

function validatePassword(password) {
    if (!password) return true; // Password có thể trống khi update
    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/;
    return passwordRegex.test(password);
}

function formatName(name) {
    if (!name) return name;
    const words = name.split(' ');
    const formattedWords = words.map(word => {
        if (word.length > 0) {
            return word.charAt(0).toUpperCase() + word.slice(1).toLowerCase();
        }
        return word;
    });
    return formattedWords.join(' ');
}

function formatPhone(phone) {
    if (!phone) return phone;
    const cleanPhone = phone.replace(/[^\d]/g, '');
    if (cleanPhone.startsWith('84')) {
        return '0' + cleanPhone.substring(2);
    }
    return cleanPhone;
} 