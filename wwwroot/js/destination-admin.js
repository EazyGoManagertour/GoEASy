// Destination Admin JavaScript
document.addEventListener('DOMContentLoaded', function() {
    // Toast notifications
    const successMsg = document.getElementById('toast-success-msg').value;
    const errorMsg = document.getElementById('toast-error-msg').value;

    if (successMsg) {
        showToast('success', successMsg);
    }
    if (errorMsg) {
        showToast('error', errorMsg);
    }

    // Initialize tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
});

function showToast(type, message) {
    const toastId = type === 'success' ? 'successToast' : 'errorToast';
    const toast = new bootstrap.Toast(document.getElementById(toastId));
    document.querySelector(`#${toastId} .toast-body`).textContent = message;
    toast.show();
}

function fillEditModal(id, name, description, location, status) {
    document.getElementById('editDestinationId').value = id;
    document.getElementById('editDestinationName').value = name;
    document.getElementById('editDescription').value = description;
    document.getElementById('editLocation').value = location;
    document.getElementById('editStatus').value = status;
}

function setDeleteDestinationId(id) {
    document.getElementById('deleteDestinationId').value = id;
}

// Toggle sidebar for mobile
function toggleSidebar() {
    const sidebar = document.querySelector('.sidebar');
    const overlay = document.querySelector('.overlay');
    
    if (sidebar.classList.contains('active')) {
        sidebar.classList.remove('active');
        overlay.style.display = 'none';
    } else {
        sidebar.classList.add('active');
        overlay.style.display = 'block';
    }
} 