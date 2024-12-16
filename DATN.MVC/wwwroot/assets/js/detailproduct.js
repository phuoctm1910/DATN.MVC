// Script for Thumbnail Click and Timer
document.querySelectorAll('.thumbnail').forEach(thumbnail => {
    thumbnail.addEventListener('click', function() {
        document.getElementById('main-image').src = this.src;
    });
});


const thumbnailsContainer = document.querySelector('.thumbnail-container');
const leftButton = document.getElementById('angle-left');
const rightButton = document.getElementById('angle-right');

leftButton.addEventListener('click', () => {
    thumbnailsContainer.scrollBy({
        left: -150,
        behavior: 'smooth',
    });
});

rightButton.addEventListener('click', () => {
    thumbnailsContainer.scrollBy({
        left: 150,
        behavior: 'smooth',
    });
});


const input = document.getElementById('quantity');
const btnIncrease = document.querySelector('.btn-increase');
const btnDecrease = document.querySelector('.btn-decrease');

btnIncrease.addEventListener('click', () => {
    input.value = parseInt(input.value) + 1;
});

btnDecrease.addEventListener('click', () => {
    if (parseInt(input.value) > parseInt(input.min)) {
        input.value = parseInt(input.value) - 1;
    }
});