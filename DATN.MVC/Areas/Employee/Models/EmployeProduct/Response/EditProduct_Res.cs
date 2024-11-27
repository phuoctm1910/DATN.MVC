namespace DATN.MVC.Areas.Employee.Models.EmployeProduct.Response
{
    public class EditProduct_Res
    {
        public int Id { get; set; } // ID của sản phẩm
        public string Name { get; set; } // Tên sản phẩm
        public bool IsActived { get; set; } // Trạng thái mới sau khi thay đổi
    }
}
