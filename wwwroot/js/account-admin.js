
        function setDeleteAdminId(id) {
        document.getElementById("deleteAdminId").value = id;
        }
  

  
        function fillEditModal(id, username, password, fullname, email, phone, address, role, status) {
        document.getElementById("editAdminID").value = id;
        document.getElementById("editUsername").value = username;
        document.getElementById("editPassword").value = ""; // ❗ Bảo mật: để trống!
        document.getElementById("editName").value = fullname;
        document.getElementById("editEmail").value = email;
        document.getElementById("editPhone").value = phone;
        document.getElementById("editAddress").value = address;
        document.getElementById("editRole").value = role;
        document.getElementById("editStatus").value = status;
        }
    