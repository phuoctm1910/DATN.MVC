using System.ComponentModel.DataAnnotations;

namespace DATN.MVC.Areas.Admin.Models.ManagerEmployee.Request
{
    public class EmployeeReq
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên nhân viên là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên nhân viên không được quá 100 ký tự.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Trạng thái nhân viên là bắt buộc.")]
        public bool IsActived { get; set; }

        // Các trường khác nếu có thêm (ví dụ CreatedDate)
    }
}
