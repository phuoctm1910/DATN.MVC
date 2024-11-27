let dataTable;

$(document).ready(function () {
    // Khởi tạo DataTable
    dataTable = $('#datatablesProduct').DataTable({
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

});


// Bắt sự kiện nút chỉnh sửa
$('.edit-button').on('click', function () {
    const id = $(this).data('id');
    const status = $(this).data('isactived');

    // Ghi log để kiểm tra dữ liệu (nếu cần)
    console.log('Product ID:', id, 'IsActived:', status);


    // Điền giá trị vào các trường trong modal
    $('#modalProductId').val(id);
    console.log(status)
    if (status === true || status === 'True') {
        $('#modalIsActived').val('true'); // Sử dụng
    } else {
        $('#modalIsActived').val('false'); // Không sử dụng
    }
});

// Lưu thay đổi khi nhấn nút "Lưu thay đổi"
$('#saveChangesButton').on('click', function () {


    const ProductData = {
        Id: $('#modalProductId').val(),
        IsActived: $('#modalIsActived').val() // Kiểm tra trạng thái đã chọn
    };

    console.log(ProductData)
    // Gửi AJAX để lưu thay đổi
    $.ajax({
        url: 'https://localhost:7260/Employee/EmployeProduct/UpdateProductState',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(ProductData),
        success: function (response) {
            if (response.success) {
                alert(response.message); // Hiển thị thông báo thành công
                location.reload(); // Tải lại trang để cập nhật dữ liệu
            } else {
                alert(response.message); // Hiển thị lỗi nếu có
            }
        },
        error: function (xhr) {
            console.error(xhr); // Ghi log lỗi chi tiết
            alert('Đã xảy ra lỗi khi cập nhật sản phẩm.');
        }
    });
});