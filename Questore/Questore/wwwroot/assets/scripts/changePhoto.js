const openPhotoModalButton = document.getElementById('change-photo-button');
const backdropPhoto = document.getElementById('backdrop');
const changePhotoModal = document.getElementById('change-photo-modal');
const changePhotoButton = document.getElementById('change-photo');
const cancelChangePhotoButton = document.getElementById('cancel-photo');

const inputFile = document.getElementById('file');
const inputPreviewContainer = document.getElementById('image-preview');
const inputPreviewImage = document.getElementById('image-preview-img');
const inputPreviewText = document.getElementById('image-preview-text');


const showPhotoModal = () => {
    changePhotoModal.classList.add('visible');
    togglePhotoBackdrop();
}

const togglePhotoBackdrop = () => {
    backdropPhoto.classList.toggle('visible');
}

const closePhotoModal = () => {
    changePhotoModal.classList.remove('visible');
    closePhotoBackdrop();
}

const closePhotoBackdrop = () => {
    backdropPhoto.classList.remove('visible');
}

openPhotoModalButton.addEventListener('click', showPhotoModal);
cancelChangePhotoButton.addEventListener('click', closePhotoModal);
backdropPhoto.addEventListener('click', closePhotoModal);

inputFile.addEventListener('change', function() {
    const file = this.files[0];

    if (file) {
        const reader = new FileReader();

        inputPreviewText.style.display = "none";
        inputPreviewImage.style.display = 'block';

        reader.addEventListener("load", function() {
            inputPreviewImage.setAttribute("src", this.result); 
        });

        reader.readAsDataURL(file);
    }
 });