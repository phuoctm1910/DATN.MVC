namespace DATN.MVC.Areas.Admin.Models.AdminCategory.Response
{
    public class ParentCategoriesReadAll_Res
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public bool IsActived { get; set; }
    }
}
