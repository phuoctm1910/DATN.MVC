let dataTable;

$(document).ready(function () {
    // Khởi tạo DataTable
    dataTable = $('#datatablesSalePlace').DataTable({
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
                    columns: [0, 1, 2, 3, 4] // Chỉ xuất các cột ID, Tên Gian Hàng, Người Tạo, Trạng Thái, Hoạt Động
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
            search: "Tìm kiếm theo tên gian hàng:",  // Thay đổi text của nút tìm kiếm
            lengthMenu: "Hiển thị _MENU_ bản ghi",
            info: "Hiển thị _START_ đến _END_ của _TOTAL_ bản ghi",
            infoEmpty: "Không có bản ghi nào",
            infoFiltered: "(lọc từ _MAX_ bản ghi)",
            zeroRecords: "Không tìm thấy kết quả"
        },
        initComplete: function () {
            // Tùy chỉnh tìm kiếm chỉ áp dụng cho cột "Tên Gian Hàng"
            let column = this.api().column(1); // Lấy cột thứ 2 (Tên Gian Hàng)
            $('input[type="search"]').off('keyup').on('keyup', function () {
                column.search(this.value).draw(); // Tìm kiếm chỉ trong cột "Tên Gian Hàng"
            });
        }
    });
});

// Hàm mở modal chỉnh sửa trạng thái
function openEditModal(id, status) {
    $('#SalePlaceId').val(id); // Gán ID gian hàng vào input
    $('#Status').val(status); // Gán trạng thái hiện tại vào dropdown
    $('#editSalePlaceModal').modal('show'); // Hiển thị modal
}

// Hàm xóa gian hàng
function deleteSalePlace(id) {
    if (confirm("Bạn có chắc chắn muốn xóa gian hàng này không?")) {
        $.ajax({
            url: '@Url.Action("DeleteSalePlace", "AdminManageSalePlace")/' + id,
            type: 'POST',
            success: function (response) {
                if (response.success) {
                    alert(response.message); // Thông báo thành công
                    dataTable.ajax.reload(); // Tải lại bảng sau khi xóa
                } else {
                    alert(response.message); // Thông báo lỗi
                }
            },
            error: function (xhr) {
                alert('Có lỗi xảy ra khi xóa gian hàng');
                console.error(xhr);
            }
        });
    }
}