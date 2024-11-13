namespace DATN.MVC.Areas.Admin.Models.AdminChildCategory.Response
{
    public class CategoriesChild_Res
    {
        public int Id { get; set; } // ID của danh mục con vừa được tạo
        public string Name { get; set; } // Tên danh mục con
        public int? ParentCategoryId { get; set; } // ID của danh mục cha
        public int CreatedDate { get; set; } // Ngày tạo
        public int UpdatedDate { get; set; } // Ngày cập nhật
    }
}
