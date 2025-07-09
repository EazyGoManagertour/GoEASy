function setDeleteTourId(id) {
    document.getElementById("deleteTourId").value = id;
}

function fillEditModal(tourId, tourName, description, adultPrice, childPrice, startDate, endDate, maxSeats, availableSeats, destinationId, categoryId) {
    document.getElementById('editTourID').value = tourId;
    document.getElementById('editTourName').value = tourName;
    document.getElementById('editDescription').value = description;
    document.getElementById('editAdultPrice').value = adultPrice;
    document.getElementById('editChildPrice').value = childPrice;
    document.getElementById('editStartDate').value = startDate;
    document.getElementById('editEndDate').value = endDate;
    document.getElementById('editMaxSeats').value = maxSeats;
    document.getElementById('editAvailableSeats').value = availableSeats;
    document.getElementById('editDestinationId').value = destinationId;
    document.getElementById('editCategoryId').value = categoryId;
}

// Image selection functions
function selectImage(element, imagePath) {
    element.classList.toggle('selected');
    updateSelectedImages();
}

function selectEditImage(element, imagePath) {
    element.classList.toggle('selected');
    updateEditSelectedImages();
}

function updateSelectedImages() {
    const selectedElements = document.querySelectorAll('#availableImagesContainer .image-select-item.selected');
    const selectedPaths = Array.from(selectedElements).map(el => {
        const img = el.querySelector('img');
        return img ? img.src.split('/image/Tourimg/')[1] : '';
    }).filter(path => path);
    
    document.getElementById('selectedImages').value = selectedPaths.join(',');
}

function updateEditSelectedImages() {
    const selectedElements = document.querySelectorAll('#editAvailableImagesContainer .image-select-item.selected');
    const selectedPaths = Array.from(selectedElements).map(el => {
        const img = el.querySelector('img');
        return img ? img.src.split('/image/Tourimg/')[1] : '';
    }).filter(path => path);
    
    document.getElementById('editSelectedImages').value = selectedPaths.join(',');
}

// Reset image selection when modal is closed
document.addEventListener('DOMContentLoaded', function() {
    const addModal = document.getElementById('addModal');
    const editModal = document.getElementById('editModal');
    
    if (addModal) {
        addModal.addEventListener('hidden.bs.modal', function() {
            document.querySelectorAll('#availableImagesContainer .image-select-item.selected').forEach(el => {
                el.classList.remove('selected');
            });
            document.getElementById('selectedImages').value = '';
        });
    }
    
    if (editModal) {
        editModal.addEventListener('hidden.bs.modal', function() {
            document.querySelectorAll('#editAvailableImagesContainer .image-select-item.selected').forEach(el => {
                el.classList.remove('selected');
            });
            document.getElementById('editSelectedImages').value = '';
        });
    }
}); 