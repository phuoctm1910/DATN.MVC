namespace DATN.MVC.Areas.Admin.Models.AdminChildCategory.Response
{
    public class ParentCategoryRespone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public bool IsSelected { get; set; } // Thêm thuộc tính này
    }
}
