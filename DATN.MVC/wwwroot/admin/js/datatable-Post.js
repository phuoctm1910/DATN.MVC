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
                    columns: [0, 1, 3] // Chỉ xuất các cột ID, Người tạo, Trạng thái
                }
            },
            {
                extend: 'csv',
                text: 'CSV',
                exportOptions: {
                    columns: [0, 1, 3]
                }
            },
            {
                extend: 'excel',
                text: 'Excel',
                exportOptions: {
                    columns: [0, 1, 3]
                }
            },
            {
                extend: 'pdf',
                text: 'PDF',
                exportOptions: {
                    columns: [0, 1, 3]
                }
            }
        ],
        dom: '<"d-flex justify-content-between align-items-center"<"d-flex align-items-center"<"lengthMenu"l><"buttons ms-2"B>><"search"f>>rt<"d-flex justify-content-between align-items-center"<"info"i><"pagination"p>>',
        language: {
            search: "Tìm kiếm theo người tạo:",  // Thay đổi text của nút tìm kiếm
            lengthMenu: "Hiển thị _MENU_ bản ghi",
            info: "Hiển thị _START_ đến _END_ của _TOTAL_ bản ghi",
            infoEmpty: "Không có bản ghi nào",
            infoFiltered: "(lọc từ _MAX_ bản ghi)",
            zeroRecords: "Không tìm thấy kết quả"
        },
        initComplete: function () {
            // Tùy chỉnh tìm kiếm chỉ áp dụng cho cột "Người tạo"
            let column = this.api().column(1); // Lấy cột thứ 2 (Người tạo)
            $('input[type="search"]').off('keyup').on('keyup', function () {
                column.search(this.value).draw(); // Tìm kiếm chỉ trong cột "Người tạo"
            });
        }
    });
});

// Hàm xóa bài đăng
function deletePost(id) {
    if (confirm("Bạn có chắc chắn muốn xóa danh mục này không?")) {
        $.ajax({
            url: 'https://localhost:7260/Admin/AdminPost/deletePost/' + id,
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
