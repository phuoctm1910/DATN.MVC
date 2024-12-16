const categoryList = document.querySelector('.category-list');
const angleRight = document.getElementById('angle-right');
const angleLeft = document.getElementById('angle-left');
const itemWidth = 120;
const itemsPerRow = 10;
const totalItems = document.querySelectorAll('.category-item').length;
const itemsPerView = itemsPerRow * 2;
let currentPosition = 0;


angleRight.addEventListener('click', () => {
    if (Math.abs(currentPosition) >= itemWidth * (totalItems - itemsPerView)) {
        currentPosition = 0;
    } else {
        currentPosition -= itemWidth * itemsPerRow;
    }

    categoryList.style.transform = `translateX(${currentPosition}px)`;

    angleLeft.classList.add('active-category');
    angleRight.classList.add('noAcctive-category');
    angleLeft.classList.remove('noAcctive-category');
});

angleLeft.addEventListener('click', () => {
    if (currentPosition < 0) {
        currentPosition += itemWidth * itemsPerRow;
    } else {
        currentPosition = -(itemWidth * (totalItems - itemsPerView));
    }

    categoryList.style.transform = `translateX(${currentPosition}px)`;

    angleRight.classList.add('active-category');
    angleLeft.classList.add('noAcctive-category');
    angleRight.classList.remove('noAcctive-category');
});