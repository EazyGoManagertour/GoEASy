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
    
    // Load ảnh theo tour
    loadTourImages(tourId);
}

// Load ảnh theo tour
async function loadTourImages(tourId) {
    try {
        const response = await fetch(`/admin/tour-admin/get-images-by-tour/${tourId}`);
        const data = await response.json();
        
        if (data.success) {
            const container = document.getElementById('editAvailableImagesContainer');
            container.innerHTML = '';
            
            data.images.forEach(imagePath => {
                const imageDiv = document.createElement('div');
                imageDiv.className = 'col-md-3 mb-2';
                imageDiv.innerHTML = `
                    <div class="image-select-item" onclick="selectEditImage(this, '${imagePath}')">
                        <img src="${imagePath}" alt="Tour Image" class="img-thumbnail" style="width: 80px; height: 80px; object-fit: cover;">
                        <div class="image-overlay">
                            <i class="fas fa-check"></i>
                        </div>
                    </div>
                `;
                container.appendChild(imageDiv);
            });
        }
    } catch (error) {
        console.error('Error loading tour images:', error);
    }
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
        if (img) {
            const src = img.src;
            if (src.includes('/image/Tourimg/')) {
                return src.split('/image/Tourimg/')[1];
            } else if (src.includes('/assets/tours/')) {
                return src.split('/assets/tours/')[1];
            }
        }
        return '';
    }).filter(path => path);
    
    document.getElementById('selectedImages').value = selectedPaths.join(',');
}

function updateEditSelectedImages() {
    const selectedElements = document.querySelectorAll('#editAvailableImagesContainer .image-select-item.selected');
    const selectedPaths = Array.from(selectedElements).map(el => {
        const img = el.querySelector('img');
        if (img) {
            const src = img.src;
            if (src.includes('/image/Tourimg/')) {
                return src.split('/image/Tourimg/')[1];
            } else if (src.includes('/assets/tours/')) {
                return src.split('/assets/tours/')[1];
            }
        }
        return '';
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