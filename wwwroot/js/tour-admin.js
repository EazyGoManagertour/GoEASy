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
    document.getElementById('editDestinationID').value = destinationId;
    document.getElementById('editCategoryID').value = categoryId;

    // Load ảnh theo tour
    loadTourImages(tourId);
    
    // Load thêm dữ liệu chi tiết
    loadTourDetails(tourId);
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

// Load chi tiết tour (Included, Excluded, Activities, Itinerary, FAQ)
async function loadTourDetails(tourId) {
    try {
        const response = await fetch(`/admin/tour-admin/get-tour-details/${tourId}`);
        const data = await response.json();

        if (data.success) {
            // Điền Included
            if (data.included) {
                document.getElementById('editIncluded').value = data.included.join('\n');
            }
            
            // Điền Excluded
            if (data.excluded) {
                document.getElementById('editExcluded').value = data.excluded.join('\n');
            }
            
            // Điền Activities
            if (data.activities) {
                document.getElementById('editActivities').value = data.activities.join('\n');
            }
            
            // Điền Itinerary
            if (data.itinerary && data.itinerary.length > 0) {
                const itineraryContainer = document.getElementById('editItineraryList');
                itineraryContainer.innerHTML = ''; // Xóa các dòng cũ
                
                data.itinerary.forEach(item => {
                    addItineraryRow('edit');
                    const rows = itineraryContainer.querySelectorAll('.itinerary-row');
                    const lastRow = rows[rows.length - 1];
                    
                    lastRow.querySelector('input[name="editItineraryDay[]"]').value = item.dayNumber || '';
                    lastRow.querySelector('input[name="editItineraryTitle[]"]').value = item.title || '';
                    lastRow.querySelector('input[name="editItineraryDescription[]"]').value = item.description || '';
                    lastRow.querySelector('input[name="editItineraryAccommodation[]"]').value = item.accommodation || '';
                    lastRow.querySelector('input[name="editItineraryMeals[]"]').value = item.meals || '';
                });
            }
            
            // Điền FAQ
            if (data.faqs && data.faqs.length > 0) {
                const faqContainer = document.getElementById('editFAQList');
                faqContainer.innerHTML = ''; // Xóa các dòng cũ
                
                data.faqs.forEach(item => {
                    addFAQRow('edit');
                    const rows = faqContainer.querySelectorAll('.faq-row');
                    const lastRow = rows[rows.length - 1];
                    
                    lastRow.querySelector('input[name="editFAQQuestion[]"]').value = item.question || '';
                    lastRow.querySelector('input[name="editFAQAnswer[]"]').value = item.answer || '';
                });
            }
        }
    } catch (error) {
        console.error('Error loading tour details:', error);
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

// ========== ITINERARY & FAQ DYNAMIC FIELDS ========== //
function addItineraryRow(type) {
    const listId = type === 'add' ? 'addItineraryList' : 'editItineraryList';
    const container = document.getElementById(listId);
    const idx = container.querySelectorAll('.itinerary-row').length + 1;
    const row = document.createElement('div');
    row.className = 'row itinerary-row mb-2';
    row.innerHTML = `
        <div class="col-md-2">
            <input type="number" class="form-control" name="${type}ItineraryDay[]" placeholder="Ngày" value="${idx}" min="1" required />
        </div>
        <div class="col-md-2">
            <input type="text" class="form-control" name="${type}ItineraryTitle[]" placeholder="Tiêu đề" required />
        </div>
        <div class="col-md-4">
            <input type="text" class="form-control" name="${type}ItineraryDescription[]" placeholder="Mô tả" required />
        </div>
        <div class="col-md-2">
            <input type="text" class="form-control" name="${type}ItineraryAccommodation[]" placeholder="Nơi ở" />
        </div>
        <div class="col-md-1">
            <input type="text" class="form-control" name="${type}ItineraryMeals[]" placeholder="Bữa ăn" />
        </div>
        <div class="col-md-1 d-flex align-items-center">
            <button type="button" class="btn btn-danger btn-sm" onclick="removeRow(this)">-</button>
        </div>
    `;
    container.appendChild(row);
}

function addFAQRow(type) {
    const listId = type === 'add' ? 'addFAQList' : 'editFAQList';
    const container = document.getElementById(listId);
    const row = document.createElement('div');
    row.className = 'row faq-row mb-2';
    row.innerHTML = `
        <div class="col-md-5">
            <input type="text" class="form-control" name="${type}FAQQuestion[]" placeholder="Câu hỏi" required />
        </div>
        <div class="col-md-6">
            <input type="text" class="form-control" name="${type}FAQAnswer[]" placeholder="Trả lời" required />
        </div>
        <div class="col-md-1 d-flex align-items-center">
            <button type="button" class="btn btn-danger btn-sm" onclick="removeRow(this)">-</button>
        </div>
    `;
    container.appendChild(row);
}

function removeRow(btn) {
    btn.closest('.row').remove();
}

// ========== AI CONTENT GENERATION ========== //
async function generateDescription(type) {
    const tourNameInput = document.getElementById(type === 'add' ? 'addTourName' : 'editTourName');
    const descriptionTextarea = document.getElementById(type === 'add' ? 'addDescription' : 'editDescription');
    const includedTextarea = document.getElementById(type === 'add' ? 'addIncluded' : 'editIncluded');
    const excludedTextarea = document.getElementById(type === 'add' ? 'addExcluded' : 'editExcluded');
    const activitiesTextarea = document.getElementById(type === 'add' ? 'addActivities' : 'editActivities');
    const itineraryList = document.getElementById(type === 'add' ? 'addItineraryList' : 'editItineraryList');

    const tourName = tourNameInput.value;
    const included = includedTextarea ? includedTextarea.value : '';
    const excluded = excludedTextarea ? excludedTextarea.value : '';
    const activities = activitiesTextarea ? activitiesTextarea.value : '';

    // Lấy danh sách itinerary (dạng text)
    let itinerary = [];
    if (itineraryList) {
        const rows = itineraryList.querySelectorAll('.itinerary-row');
        rows.forEach(row => {
            const day = row.querySelector('input[name$="ItineraryDay[]"]').value;
            const title = row.querySelector('input[name$="ItineraryTitle[]"]').value;
            const desc = row.querySelector('input[name$="ItineraryDescription[]"]').value;
            itinerary.push(`Ngày ${day}: ${title} - ${desc}`);
        });
    }

    // Debug log
    console.log({
        included,
        excluded,
        activities,
        itinerary
    });

    if (!tourName) {
        alert('Vui lòng nhập tên tour trước khi tạo mô tả.');
        return;
    }

    const button = tourNameInput.closest('.modal-body').querySelector(`button[onclick="generateDescription('${type}')"]`);
    const originalButtonHtml = button.innerHTML;
    button.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Generating...';
    button.disabled = true;

    try {
        const response = await fetch('/admin/tour-admin/generate-description', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                tourName: tourName,
                included: included,
                excluded: excluded,
                activities: activities,
                itinerary: itinerary
            })
        });

        if (response.ok) {
            const data = await response.json();
            descriptionTextarea.value = data.description;
        } else {
            alert('Không thể tạo mô tả. Vui lòng thử lại.');
        }
    } catch (error) {
        console.error('Error generating description:', error);
        alert('Đã có lỗi xảy ra.');
    } finally {
        button.innerHTML = originalButtonHtml;
        button.disabled = false;
    }
}

// Reset image selection when modal is closed
document.addEventListener('DOMContentLoaded', function () {
    const addModal = document.getElementById('addModal');
    const editModal = document.getElementById('editModal');

    if (addModal) {
        addModal.addEventListener('hidden.bs.modal', function () {
            document.querySelectorAll('#availableImagesContainer .image-select-item.selected').forEach(el => {
                el.classList.remove('selected');
            });
            document.getElementById('selectedImages').value = '';
        });
    }

    if (editModal) {
        editModal.addEventListener('hidden.bs.modal', function () {
            document.querySelectorAll('#editAvailableImagesContainer .image-select-item.selected').forEach(el => {
                el.classList.remove('selected');
            });
            document.getElementById('editSelectedImages').value = '';
        });
    }
}); 