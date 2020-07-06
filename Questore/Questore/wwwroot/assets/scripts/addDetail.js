const openAddDetailButton = document.getElementById('add-button');
const backdrop = document.getElementById('backdrop');
const addDetailModal = document.getElementById('add-modal');
const addDetailButton = document.getElementById('add');
const cancelAddDetailButton = document.getElementById('cancel');
const nameInput = document.getElementById('name-input');
const descriptionInput = document.getElementById('description-input');

const showModal = () => {
    addDetailModal.classList.add('visible');
    toggleBackdrop();

}

const toggleBackdrop = () => {
    backdrop.classList.toggle('visible');
}

const closeModal = () => {
    addDetailModal.classList.remove('visible');
    closeBackdrop();
}

const closeBackdrop = () => {
    backdrop.classList.remove('visible');
}

if (nameInput.value !== '' && nameInput.value !== null){
    showModal();
}
else if (descriptionInput.value !== '' && descriptionInput.value !== null){
    showModal();
}


openAddDetailButton.addEventListener('click', showModal);
backdrop.addEventListener('click', closeModal);
cancelAddDetailButton.addEventListener('click', closeModal);