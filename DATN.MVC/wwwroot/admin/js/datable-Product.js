let dataTable;
$(document).ready(function () {
    // Khởi tạo DataTable cho sản phẩm
    const dataTable = $('#datatablesProduct').DataTable({
        responsive: true,
        paging: true,
        searching: true,
        ordering: true,
        info: true,
        lengthChange: true,
        pageLength: 5,
        lengthMenu: [5, 10, 15, 20],
        //buttons: [
        //    { extend: 'copy', text: 'Copy', exportOptions: { columns: [0, 1, 2, 3, 4] } },
        //    { extend: 'csv', text: 'CSV', exportOptions: { columns: [0, 1, 2, 3, 4] } },
        //    { extend: 'excel', text: 'Excel', exportOptions: { columns: [0, 1, 2, 3, 4] } },
        //    { extend: 'pdf', text: 'PDF', exportOptions: { columns: [0, 1, 2, 3, 4] } }
        //],
        dom: '<"d-flex justify-content-between"<"d-flex align-items-center"<"lengthMenu"l><"buttons ms-2"B>><"search"f>>rt<"d-flex justify-content-between align-items-center"<"info"i><"pagination"p>>',
        language: {
            search: "Tìm kiếm sản phẩm:",
            lengthMenu: "Hiển thị _MENU_ bản ghi",
            info: "Hiển thị _START_ đến _END_ của _TOTAL_ bản ghi",
            infoEmpty: "Không có bản ghi nào",
            infoFiltered: "(lọc từ _MAX_ bản ghi)",
            zeroRecords: "Không tìm thấy kết quả"
        }
    });

    // Hàm gửi yêu cầu cập nhật trạng thái sản phẩm
    $('#editProductForm').submit(function (e) {
        e.preventDefault(); // Ngừng hành động submit mặc định

        var productId = $('#ProductId').val(); // Lấy ID sản phẩm
        var isActive = $('#IsActive').val() === 'true'; // Lấy trạng thái IsActive và chuyển thành boolean

        // Kiểm tra xem ID sản phẩm có hợp lệ không
        if (!productId || productId <= 0) {
            alert('ID sản phẩm không hợp lệ.');
            return;
        }

        // Gửi yêu cầu PUT để cập nhật trạng thái sản phẩm
        $.ajax({
            url: `https://localhost:7296/api/product/EditStatus`, // URL API chỉnh sửa trạng thái sản phẩm
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify({
                Id: productId,  // Gửi ID sản phẩm trong body
                IsActived: isActive  // Gửi trạng thái hoạt động
            }),
            success: function (response) {
                if (response.success) {
                    alert('Cập nhật trạng thái sản phẩm thành công.');
                    location.reload();  // Reload lại trang sau khi cập nhật thành công
                } else {
                    alert('Không thể cập nhật trạng thái sản phẩm.');
                }
            },
            error: function (xhr) {
                alert('Có lỗi xảy ra khi cập nhật.');
            }
        });
    });

    // Hàm xóa sản phẩm
    window.deleteProduct = function (productId) {
        if (confirm('Bạn có chắc muốn xóa sản phẩm?')) {
            $.ajax({
                url: `https://localhost:7296/api/product/delete/${productId}`,  // URL API xóa sản phẩm
                type: 'DELETE',
                success: function (response) {
                    if (response.success) {
                        alert('Xóa sản phẩm thành công.');
                        location.reload(); // Reload lại trang để cập nhật lại danh sách sản phẩm
                    } else {
                        alert('Xóa thất bại: ' + response.message);
                    }
                },
                error: function () {
                    alert('Có lỗi xảy ra khi xóa.');
                }
            });
        }
    };

    // Mở modal chỉnh sửa trạng thái sản phẩm
    window.openEditModal = function (id, isActive) {
        $('#ProductId').val(id); // Đặt ID sản phẩm vào hidden field
        $('#IsActive').val(isActive.toString()); // Đặt trạng thái IsActive vào dropdown
        $('#editProductModal').modal('show'); // Mở modal chỉnh sửa
    };
});
