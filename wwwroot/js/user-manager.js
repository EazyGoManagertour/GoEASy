// Set delete user ID for delete modal
function setDeleteUserId(id) {
    document.getElementById("deleteUserId").value = id;
}

// Fill edit modal with user data
function fillEditModal(id, username, password, fullname, email, phone, address, sex, role, isvip, vippoints, status) {
    document.getElementById("editUserID").value = id;
    document.getElementById("editUsername").value = username;
    document.getElementById("editPassword").value = ""; // ❗ Bảo mật: để trống!
    document.getElementById("editName").value = fullname;
    document.getElementById("editEmail").value = email;
    document.getElementById("editPhone").value = phone;
    document.getElementById("editAddress").value = address;
    document.getElementById("editSex").value = sex;
    document.getElementById("editRole").value = role;
    document.getElementById("editIsVip").value = isvip;
    document.getElementById("editVippoints").value = vippoints;
    document.getElementById("editStatus").value = status;
} 