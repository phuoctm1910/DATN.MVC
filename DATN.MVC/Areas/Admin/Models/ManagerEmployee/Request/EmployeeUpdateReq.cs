using System.ComponentModel.DataAnnotations;

namespace DATN.MVC.Areas.Admin.Models.ManagerEmployee.Request
{
    public class EmployeeUpdateReq
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Trạng thái nhân viên là bắt buộc.")]
        public bool IsActived { get; set; }

        // Các trường khác nếu có thêm (ví dụ CreatedDate)
       
    }
}
