namespace DATN.MVC.Areas.Employee.Models.EmployeProduct.Request
{
    public class EditProductState_Req
    {
        public int Id { get; set; } // ID của sản phẩm cần thay đổi
        public bool IsActived { get; set; } // Trạng thái mới của sản phẩm
    }
}
