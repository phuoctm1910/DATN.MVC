let dataTable;
$(document).ready(function () {
    // Khởi tạo DataTable cho tài khoản
    dataTable = $('.table').DataTable({
        responsive: true,
        paging: true,
        searching: true,
        ordering: true,
        info: true,
        lengthChange: true,
        pageLength: 5,
        lengthMenu: [5, 10, 15, 20],
        dom: '<"d-flex justify-content-between"<"d-flex align-items-center"<"lengthMenu"l><"buttons ms-2"B>><"search"f>>rt<"d-flex justify-content-between align-items-center"<"info"i><"pagination"p>>',
        language: {
            search: "Tìm kiếm tài khoản:",
            lengthMenu: "Hiển thị _MENU_ bản ghi",
            info: "Hiển thị _START_ đến _END_ của _TOTAL_ bản ghi",
            infoEmpty: "Không có bản ghi nào",
            infoFiltered: "(lọc từ _MAX_ bản ghi)",
            zeroRecords: "Không tìm thấy kết quả"
        }
    });

    // Chỉnh sửa Trạng Thái
    window.openEditModal = function (id, currentStatus) {
        $('#EditId').val(id);
        $('#EditIsActived').val(currentStatus.toString());
        $('#editAccountModal').modal('show');
    };

    $('#editAccountForm').submit(function (e) {
        e.preventDefault(); // Ngừng hành động submit mặc định

        var accountId = $('#EditId').val(); // Lấy ID tài khoản
        var isActive = $('#EditIsActived').val() === 'true'; // Lấy trạng thái IsActive và chuyển thành boolean

        // Kiểm tra xem ID tài khoản có hợp lệ không
        if (!accountId || accountId <= 0) {
            alert('ID tài khoản không hợp lệ.');
            return;
        }

        // Gửi yêu cầu PUT để cập nhật trạng thái tài khoản
        $.ajax({
            url: 'https://localhost:7296/api/AccountInfor/EditStatus', // URL API sửa lại không có ID trong URL
            type: 'PUT',
            contentType: 'application/json', // Đảm bảo gửi dữ liệu dưới dạng JSON
            data: JSON.stringify({
                Id: accountId, // Gửi ID tài khoản trong body
                IsActived: isActive // Gửi trạng thái IsActived
            }),
            success: function (response) {
                alert('Cập nhật trạng thái tài khoản thành công.');
                location.reload(); // Reload lại trang sau khi cập nhật thành công
            },
            error: function (xhr) {
                alert('Có lỗi xảy ra khi cập nhật.');
            }
        });
    });

    // Xóa Tài Khoản
    window.deleteAccount = function (accountId) {
        if (confirm('Bạn có chắc muốn xóa tài khoản này?')) {
            $.ajax({
                url: `https://localhost:7296/api/AccountInfor/Delete/${accountId}`, // URL API Xóa
                type: 'DELETE',
                success: function (response) {
                    alert('Xóa tài khoản thành công.');
                    location.reload(); // Tải lại trang sau khi xóa
                },
                error: function () {
                    alert('Có lỗi xảy ra khi xóa tài khoản.');
                }
            });
        }
    };
});
