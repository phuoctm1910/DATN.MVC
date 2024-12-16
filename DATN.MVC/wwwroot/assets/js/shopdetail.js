const row = document.querySelector('.list-product');
const btnLeft = document.getElementById('angle-left');
const btnRight = document.getElementById('angle-right');

let isDragging = false;
let startX;
let currentPosition = 0;
const itemWidth = 203;
const maxScroll = row.scrollWidth - row.clientWidth;

// Lắng nghe sự kiện mousedown (bắt đầu kéo)
row.addEventListener('mousedown', (e) => {
    isDragging = true;
    startX = e.pageX - row.offsetLeft; // Vị trí ban đầu của chuột
    row.style.cursor = 'grabbing'; // Đổi con trỏ thành "đang kéo"
});

// Lắng nghe sự kiện mousemove (đang kéo)
row.addEventListener('mousemove', (e) => {
    if (!isDragging) return; // Nếu không đang kéo thì không làm gì
    const moveX = e.pageX - row.offsetLeft; // Vị trí chuột mới
    const distance = startX - moveX; // Khoảng cách di chuyển của chuột

    // Điều chỉnh vị trí của danh sách sản phẩm
    currentPosition += distance;
    
    // Kiểm tra để không cuộn vượt quá các giới hạn
    if (currentPosition < 0) currentPosition = 0;
    if (currentPosition > maxScroll) currentPosition = maxScroll;

    row.style.transform = `translateX(-${currentPosition}px)`; // Di chuyển danh sách
    startX = moveX; // Cập nhật vị trí chuột ban đầu để tính toán tiếp
});

// Lắng nghe sự kiện mouseup (kết thúc kéo)
row.addEventListener('mouseup', () => {
    isDragging = false;
    row.style.cursor = 'grab'; // Đổi lại con trỏ chuột
});

// Lắng nghe sự kiện mouseleave (nếu chuột rời khỏi vùng danh sách sản phẩm)
row.addEventListener('mouseleave', () => {
    isDragging = false;
    row.style.cursor = 'grab'; // Đổi lại con trỏ chuột nếu chuột rời khỏi
});

// Nút cuộn sang trái
btnLeft.addEventListener('click', () => {
    slideLeft();
});

// Nút cuộn sang phải
btnRight.addEventListener('click', () => {
    slideRight();
});

// Hàm cuộn sang trái 1 sản phẩm
function slideLeft() {
    currentPosition -= itemWidth; // Cuộn 1 sản phẩm
    if (currentPosition < 0) currentPosition = 0;
    row.style.transform = `translateX(-${currentPosition}px)`;
}

// Hàm cuộn sang phải 1 sản phẩm
function slideRight() {
    currentPosition += itemWidth; // Cuộn 1 sản phẩm
    if (currentPosition > maxScroll) currentPosition = maxScroll; // Dừng lại khi đạt đến cuối
    row.style.transform = `translateX(-${currentPosition}px)`;
}
