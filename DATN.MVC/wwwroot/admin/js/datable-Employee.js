let dataTable;
$(document).ready(function () {
    // Khởi tạo DataTable cho nhân viên
    const dataTable = $('.table').DataTable({
        responsive: true,
        paging: true,
        searching: true,
        ordering: true,
        info: true,
        lengthChange: true,
        pageLength: 5,
        lengthMenu: [5, 10, 15, 20],
        //buttons: [
        //    { extend: 'copy', text: 'Copy', exportOptions: { columns: [0, 1, 2, 3] } },
        //    { extend: 'csv', text: 'CSV', exportOptions: { columns: [0, 1, 2, 3] } },
        //    { extend: 'excel', text: 'Excel', exportOptions: { columns: [0, 1, 2, 3] } },
        //    { extend: 'pdf', text: 'PDF', exportOptions: { columns: [0, 1, 2, 3] } }
        //] ,
        dom: '<"d-flex justify-content-between"<"d-flex align-items-center"<"lengthMenu"l><"buttons ms-2"B>><"search"f>>rt<"d-flex justify-content-between align-items-center"<"info"i><"pagination"p>>',
        language: {
            search: "Tìm kiếm nhân viên:",
            lengthMenu: "Hiển thị _MENU_ bản ghi",
            info: "Hiển thị _START_ đến _END_ của _TOTAL_ bản ghi",
            infoEmpty: "Không có bản ghi nào",
            infoFiltered: "(lọc từ _MAX_ bản ghi)",
            zeroRecords: "Không tìm thấy kết quả"
        }
    });

    // Thêm Nhân Viên
    $('#createEmployeeForm').submit(function (e) {
        e.preventDefault(); // Ngừng hành động submit mặc định

        var userName = $('#UserName').val();
        var passwordHash = $('#PasswordHash').val();
        var isActived = $('#IsActived').val() === 'true'; // Chuyển trạng thái thành boolean

        // Kiểm tra nếu tên đăng nhập và mật khẩu không rỗng
        if (!userName || !passwordHash) {
            alert('Vui lòng nhập đầy đủ thông tin.');
            return;
        }

        // Gửi yêu cầu POST để thêm nhân viên
        $.ajax({
            url: 'https://localhost:7296/api/employee/Create', // URL API Thêm
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ UserName: userName, PasswordHash: passwordHash, IsActived: isActived }),
            success: function (response) {
                alert('Thêm nhân viên thành công!');
                location.reload(); // Tải lại trang để cập nhật danh sách nhân viên
                $('#createEmployeeModal').modal('hide'); // Đóng modal sau khi thêm thành công
            },
            error: function () {
                alert('Có lỗi xảy ra khi thêm nhân viên.');
            }
        });
    });

    // Chỉnh sửa Trạng Thái
    window.openEditModal = function (id, currentStatus) {
        $('#EditId').val(id);
        $('#EditIsActived').val(currentStatus.toString());
        $('#editEmployeeModal').modal('show');
    };

    $('#editEmployeeForm').submit(function (e) {
        e.preventDefault(); // Ngừng hành động submit mặc định

        var employeeId = $('#EditId').val(); // Lấy ID nhân viên
        var isActive = $('#EditIsActived').val() === 'true'; // Lấy trạng thái IsActive và chuyển thành boolean

        // Kiểm tra xem ID nhân viên có hợp lệ không
        if (!employeeId || employeeId <= 0) {
            alert('ID nhân viên không hợp lệ.');
            return;
        }

        // Gửi yêu cầu PUT để cập nhật trạng thái nhân viên
        $.ajax({
            url: 'https://localhost:7296/api/employee/EditStatus', // URL API sửa lại không có ID trong URL
            type: 'PUT',
            contentType: 'application/json', // Đảm bảo gửi dữ liệu dưới dạng JSON
            data: JSON.stringify({
                Id: employeeId, // Gửi ID nhân viên trong body
                IsActived: isActive // Gửi trạng thái IsActived
            }),
            success: function (response) {
                alert('Cập nhật trạng thái nhân viên thành công.');
                location.reload(); // Reload lại trang sau khi cập nhật thành công
            },
            error: function (xhr) {
                alert('Có lỗi xảy ra khi cập nhật.');
            }
        });
    });

    // Xóa Nhân Viên
    window.deleteEmployee = function (employeeId) {
        if (confirm('Bạn có chắc muốn xóa nhân viên này?')) {
            $.ajax({
                url: `https://localhost:7296/api/employee/Delete/${employeeId}`, // URL API Xóa
                type: 'DELETE',
                success: function (response) {
                    alert('Xóa nhân viên thành công.');
                    location.reload(); // Tải lại trang sau khi xóa
                },
                error: function () {
                    alert('Có lỗi xảy ra khi xóa nhân viên.');
                }
            });
        }
    };
});
