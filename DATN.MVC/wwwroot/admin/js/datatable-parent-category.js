let dataTable;

$(document).ready(function () {
    // Khởi tạo DataTable
    dataTable = $('#datatablesParentCategory').DataTable({
        responsive: true,
        paging: true,
        searching: true,  // Tính năng tìm kiếm
        ordering: true,
        info: true,       // Hiển thị thông tin dưới bảng
        lengthChange: true,
        pageLength: 5,  // Số lượng bản ghi mặc định
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
            search: "Tìm kiếm theo tên danh mục:",  // Thay đổi text của nút tìm kiếm
            lengthMenu: "Hiển thị _MENU_ bản ghi",
            info: "Hiển thị _START_ đến _END_ của _TOTAL_ bản ghi",
            infoEmpty: "Không có bản ghi nào",
            infoFiltered: "(lọc từ _MAX_ bản ghi)",
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

    // Xử lý Form Thêm danh mục
    $('#createCategoryForm').submit(function (e) {
        e.preventDefault();

        // Lấy giá trị của Name
        const name = $('#Name').val().trim();  // Dùng .trim() để loại bỏ khoảng trắng ở đầu và cuối chuỗi

        // Kiểm tra nếu tên rỗng
        if (name === "") {
            $('#NameError').text("Tên danh mục không được để trống.");
            return; // Dừng lại nếu tên rỗng
        }

        // Biểu thức chính quy để kiểm tra ký tự hợp lệ (chỉ cho phép chữ cái, số, dấu cách và các ký tự tiếng Việt)
        const validNamePattern = /^[a-zA-Z0-9À-ỹ\s]+$/;

        // Kiểm tra nếu tên không hợp lệ
        if (!validNamePattern.test(name)) {
            // Nếu tên không hợp lệ, hiển thị thông báo lỗi và dừng form submit
            $('#NameError').text("Tên danh mục không được chứa ký tự đặc biệt.");
            return; // Dừng lại nếu dữ liệu không hợp lệ
        } else {
            $('#NameError').text(""); // Xóa thông báo lỗi nếu tên hợp lệ
        }

        // Tạo đối tượng dữ liệu
        const categoryData = {
            Name: name,
            IsActived: $('#IsActived').val()
        };

        // Gửi AJAX
        $.ajax({
            url: 'https://localhost:7260/Admin/AdminCategory/CreateParentCategory',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(categoryData),
            success: function (response) {
                // Kiểm tra thành công hay không
                if (response.success) {
                    alert(response.message); // Thông báo thành công
                    $('#createParentCategoryModal').modal('hide');
                    location.reload();
                } else {
                    alert(response.message); // Thông báo lỗi
                }
            },
            error: function (xhr, status, error) {
                // Nếu có lỗi xảy ra khi gọi API
                console.error(xhr);
                alert('Có lỗi xảy ra khi thêm danh mục. Vui lòng thử lại.');
            }
        });
    });

    // Sửa danh mục
    $('#editCategoryForm').submit(function (e) {
        e.preventDefault();

        const categoryData = {
            Id: $('#editId').val(),
            Name: $('#editName').val(),
            IsActived: $('#editIsActived').val()
        };
        console.log(categoryData);

        $.ajax({
            url: 'https://localhost:7260/Admin/AdminCategory/UpdateParentCategory',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(categoryData),
            success: function (response) {
                if (response.success) {
                    // Nếu thành công, thông báo và đóng modal
                    alert(response.message);
                    $('#editParentCategoryModal').modal('hide');
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr) {
                alert('Có lỗi xảy ra khi cập nhật danh mục');
                console.error(xhr);
            }
        });
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
