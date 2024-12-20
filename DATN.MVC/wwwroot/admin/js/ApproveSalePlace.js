let dataTable;

$(document).ready(function () {
    // Khởi tạo DataTable
    dataTable = $('#datatablesApproveSalePlace').DataTable({
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
                    columns: [0, 1, 2, 3, 4] // Chỉ xuất các cột ID, Tên gian hàng, Người tạo, Trạng thái, Ngày cập nhật
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
            // Tùy chỉnh tìm kiếm chỉ áp dụng cho cột "Tên gian hàng"
            this.api().columns(0).every(function () { // `columns(0)` refers to the first column
                var that = this;

                $('input[type="search"]', this.header()).on('keyup', function () {
                    that.search(this.value).draw();
                });
            });
        }
    });

    //// Xử lý nút Chỉnh sửa
    //$(document).on('click', '.edit-saleplace-btn', function () {
    //    const id = $(this).data('id');
    //    const name = $(this).data('name');
    //    const userId = $(this).data('userid');
    //    const status = $(this).data('status');

    //    // Điền giá trị vào các trường trong modal
    //    $('#editId').val(id);
    //    $('#editName').val(name);
    //    $('#editUserId').val(userId);
    //    console.log(status)
    //    // Điều kiện cho trạng thái, chọn đúng option trong dropdown
    //    if (status === true || status === 'True') {
    //        $('#editIsActived').val('true'); // Sử dụng
    //    } else {
    //        $('#editIsActived').val('false'); // Không sử dụng
    //    }
    //});
});
