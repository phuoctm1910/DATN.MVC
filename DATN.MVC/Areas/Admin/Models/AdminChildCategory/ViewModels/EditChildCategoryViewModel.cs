namespace DATN.MVC.Areas.Admin.Models.AdminChildCategory.ViewModels
{
    public class EditChildCategoryViewModel
    {
        public Response.CategoriesChild_Res ChildCategory { get; set; } // Thông tin danh mục con
        public List<Response.ParentCategoryRespone> ParentCategories { get; set; } // Danh sách danh mục cha (sửa chính tả)
    }
}
