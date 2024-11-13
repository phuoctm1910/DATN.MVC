namespace DATN.MVC.Areas.Admin.Models.AdminChildCategory.Request
{
    public class CategoriesChild_Req
    {
        public int? ParentCategoryId { get; set; } // ID của danh mục cha
        public string CategoryName { get; set; }  // Tên danh mục con
    }
}
