function fillEditModal(id, name, description, status) {
    document.getElementById("editCategoryId").value = id;
    document.getElementById("editCategoryName").value = name;
    document.getElementById("editDescription").value = description;
    document.getElementById("editStatus").value = status;
}

function setDeleteCategoryId(id) {
    document.getElementById("deleteCategoryId").value = id;
}