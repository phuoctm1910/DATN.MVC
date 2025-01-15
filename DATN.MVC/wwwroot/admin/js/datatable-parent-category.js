let dataTable;

$(document).ready(function () {
    // Khởi tạo DataTable
    dataTable = $('#datatablesParentCategory').DataTable({
        responsive: true,
        paging: true,
        searching: true, // Tính năng tìm kiếm
        ordering: true,
        info: true, // Hiển thị thông tin dưới bảng
        lengthChange: true,
        pageLength: 5, // Số lượng bản ghi mặc định
        lengthMenu: [5, 10, 15, 20], // Các lựa chọn cho dropdown hiển thị số bản ghi
        buttons: [
            {
                extend: 'copy',
                text: 'Copy',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4] // Chỉ xuất các cột ID, Tên danh mục, Trạng thái, Ngày tạo, Ngày cập nhật
                }
            },
            {
                extend: 'csv',
                text: 'CSV',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            },
            {
                extend: 'excel',
                text: 'Excel',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            },
            {
                extend: 'pdf',
                text: 'PDF',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            }
        ],
        dom: '<"d-flex justify-content-between align-items-center"<"d-flex align-items-center"<"lengthMenu"l><"buttons ms-2"B>><"search"f>>rt<"d-flex justify-content-between align-items-center"<"info"i><"pagination"p>>',
        language: {
            search: "Tìm kiếm theo tên danh mục:", // Thay đổi text của nút tìm kiếm
            lengthMenu: "Hiển thị MENU bản ghi",
            info: "Hiển thị START đến END của TOTAL bản ghi",
            infoEmpty: "Không có bản ghi nào",
            infoFiltered: "(lọc từ MAX bản ghi)",
            zeroRecords: "Không tìm thấy kết quả"
        },
        initComplete: function () {
            // Tùy chỉnh tìm kiếm chỉ áp dụng cho cột "Tên danh mục"
            let column = this.api().column(1); // Lấy cột thứ 2 (Tên danh mục)
            $('input[type="search"]').off('keyup').on('keyup', function () {
                column.search(this.value).draw(); // Tìm kiếm chỉ trong cột "Tên danh mục"
            });
        }
    });
    // Xử lý nút Chỉnh sửa
    $(document).on('click', '.edit-category-btn', function () {
        const id = $(this).data('id');
        const name = $(this).data('name');
        const status = $(this).data('status');

        // Điền giá trị vào các trường trong modal
        $('#editId').val(id);
        $('#editName').val(name);
        console.log(status)
        // Điều kiện cho trạng thái, chọn đúng option trong dropdown
        if (status === true || status === 'True') {
            $('#editIsActived').val('true'); // Sử dụng
        } else {
            $('#editIsActived').val('false'); // Không sử dụng
        }
    });
});
document.addEventListener("DOMContentLoaded", function () {
    var ctx = document.getElementById('myChart').getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
            datasets: [{
                label: '# of Votes',
                data: [12, 19, 3, 5, 2, 3],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
});
// Xử lý sự kiện chọn ảnh
$('#ImageFile').change(function () {
    validateAndSubmitCreateForm();
});

$('#Name').on('input', function () {
    validateAndSubmitCreateForm();
});

$('#IsActived').change(function () {
    validateAndSubmitCreateForm();
});

function validateAndSubmitCreateForm() {
    //const name = $('#Name').val().trim();
    //const imageFile = $('#ImageFile')[0].files;

    //if (name !== "" && imageFile.length > 0 && $('#IsActived').val() !== "") {
    //    $('#createCategoryForm').submit();
    //}
}
// Ảnh xem trước
$('#ImageFile').change(function () {
    const files = this.files;
    const preview = $('#imagePreview');
    preview.empty(); // Xóa nội dung cũ

    if (files.length > 0) {
        for (let i = 0; i < files.length; i++) {
            const file = files[i];
            const reader = new FileReader();

            reader.onload = function (e) {
                // Tạo thẻ img mới và thêm vào div xem trước
                const img = $('<img>').attr('src', e.target.result).css({
                    width: '100px',
                    height: '100px',
                    margin: '5px',
                    border: '1px solid #ccc',
                    borderRadius: '5px'
                });
                preview.append(img);
            };

            reader.readAsDataURL(file); // Đọc file dưới dạng URL
        }
    }
});
// Xử lý Form Thêm danh mục
$('#createCategoryForm').submit(function (e) {
    e.preventDefault();

    const name = $('#Name').val().trim();
    const imageFile = $('#ImageFile')[0].files;

    if (name === "") {
        $('#NameError').text("Tên danh mục không được để trống.");
        return;
    }

    if (imageFile.length === 0) {
        $('#ImageFileError').text("Vui lòng chọn ít nhất một hình ảnh.");
        return;
    }

    const formData = new FormData();
    formData.append("Name", name);
    formData.append("IsActived", $('#IsActived').val());
    formData.append("ImageFile", imageFile[0]);

    $.ajax({
        url: 'https://localhost:7260/Admin/AdminCategory/CreateParentCategory',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                alert(response.message);
                $('#createParentCategoryModal').modal('hide');
                location.reload();
            } else {
                alert(response.message);
            }
        },
        error: function (xhr) {
            console.error(xhr);
            alert('Có lỗi xảy ra khi thêm danh mục. Vui lòng thử lại.');
        }
    });
});
// Sửa danh mục
$('#editCategoryForm').submit(function (e) {
    e.preventDefault();
    // Lấy giá trị của Name
    const name = $('#editName').val().trim();  // Dùng .trim() để loại bỏ khoảng trắng ở đầu và cuối chuỗi

    // Kiểm tra nếu tên rỗng
    if (name === "") {
        $('#NameErrorEdit').text("Tên danh mục không được để trống.");
        return; // Dừng lại nếu tên rỗng
    }

    // Biểu thức chính quy để kiểm tra ký tự hợp lệ (chỉ cho phép chữ cái, số, dấu cách và các ký tự tiếng Việt)
    const validNamePattern = /^[a-zA-Z0-9À-ỹ\s]+$/;

    // Kiểm tra nếu tên không hợp lệ
    if (!validNamePattern.test(name)) {
        // Nếu tên không hợp lệ, hiển thị thông báo lỗi và dừng form submit
        $('#NameErrorEdit').text("Tên danh mục không được chứa ký tự đặc biệt.");
        return; // Dừng lại nếu dữ liệu không hợp lệ
    } else {
        $('#NameErrorEdit').text(""); // Xóa thông báo lỗi nếu tên hợp lệ
    }

    // Tạo đối tượng dữ liệu
    const categoryData = {
        Id: $('#editId').val(),
        Name: name,
        IsActived: $('#editIsActived').val()
    };

    // Gửi AJAX
    $.ajax({
        url: 'https://localhost:7260/Admin/AdminCategory/UpdateParentCategory',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(categoryData),
        success: function (response) {
            if (response.success) {
                // Nếu thành công, thông báo và đóng modal
                alert('Cập nhật thành công');
                $('#editParentCategoryModal').modal('hide');
                location.reload();
            } else {
                alert('Cập nhật thất bại: ' + response.message);
            }
        },
        error: function (xhr) {
            alert('Có lỗi xảy ra khi cập nhật danh mục');
            console.error(xhr);
        }
    });

});

// Hàm xóa danh mục
function deleteCategory(id) {
    if (confirm("Bạn có chắc chắn muốn xóa danh mục này không?")) {
        $.ajax({
            url: 'https://localhost:7260/Admin/AdminCategory/DeleteParentCategory/' + id,
            type: 'POST',
            success: function (response) {
                if (response.success) {
                    // Nếu thành công, thông báo và đóng modal
                    alert(response.message);
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr) {
                alert('Có lỗi xảy ra khi xóa danh mục');
                console.error(xhr);
            }
        });
    }
}
