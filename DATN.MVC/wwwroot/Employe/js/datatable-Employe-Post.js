let dataTable;

$(document).ready(function () {
    // Khởi tạo DataTable
    dataTable = $('#datatablesPost').DataTable({
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
                    columns: [0, 1, 2, 3, 4] // Chỉ xuất các cột ID, Tên Người Dùng, Nội Dung, Hình Ảnh, Đối Tượng, Trạng Thái, Ngày Tạo, Ngày Cập Nhật
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
            search: "Tìm kiếm:",  // Thay đổi text của nút tìm kiếm
            lengthMenu: "Hiển thị _MENU_ bản ghi",
            info: "Hiển thị _START_ đến _END_ của _TOTAL_ bản ghi",
            infoEmpty: "Không có bản ghi nào",
            infoFiltered: "(lọc từ _MAX_ bản ghi)",
            zeroRecords: "Không tìm thấy kết quả"
        },
        initComplete: function () {
            // Tùy chỉnh tìm kiếm chỉ áp dụng cho cột "Tên Người Dùng"
            let column = this.api().column(1); // Lấy cột thứ 2 (Tên Người Dùng)
            $('input[type="search"]').off('keyup').on('keyup', function () {
                column.search(this.value).draw(); // Tìm kiếm chỉ trong cột "Tên Người Dùng"
            });
        }
    });

    // Xử lý nút Chỉnh sửa
    $(document).on('click', '.edit-button', function () {
        const id = $(this).data('id');
        const isActived = $(this).data('isactived');

        // Điền giá trị vào các trường trong modal
        $('#modalPostId').val(id);

        // Điều kiện cho trạng thái, chọn đúng option trong dropdown
        if (isActived === true || isActived === 'True') {
            $('#modalIsActived').val('true'); // Sử dụng
        } else {
            $('#modalIsActived').val('false'); // Không sử dụng
        }
    });

    // Lưu thay đổi khi nhấn nút "Lưu thay đổi"
    $('#saveChangesButton').click(function () {
        const postId = $('#modalPostId').val();
        const isActived = $('#modalIsActived').val();



        const PostData = {
            PostId: postId,
            IsActived: isActived // Kiểm tra trạng thái đã chọn
        };


        console.log(PostData)
        $.ajax({
            url: 'https://localhost:7260/Employee/EmployePost/UpdatePostState',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(PostData),
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
});


// Hàm xử lý nút "Chỉnh sửa"
function handleEditButtonClick() {
    $(document).on('click', '.edit-button', function () {
        const id = $(this).data('id');
        const status = $(this).data('isactived');

        // Hiển thị modal và gán giá trị
        $('#modalPostId').val(id);
        $('#modalIsActived').val(status ? 'true' : 'false');
        $('#editPostModal').modal('show');
    });
}

// Hàm xử lý nút "Lưu thay đổi"
function handleSaveChangesClick() {
    $('#saveChangesButton').on('click', function () {
        const data = {
            PostId: parseInt($('#modalPostId').val()),
            IsActived: $('#modalIsActived').val() === 'true'
        };

        $.ajax({
            url: '@Url.Action("UpdatePostState", "EmployeePost", new { area = "Employee" })',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    dataTable.ajax.reload(); // Tải lại bảng
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr) {
                console.error(xhr);
                alert('Đã xảy ra lỗi khi cập nhật bài đăng.');
            }
        });
    });
}