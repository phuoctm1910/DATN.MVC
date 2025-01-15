namespace DATN.MVC.Areas.Admin.Models.AdminCategory.Request
{
    public class ParentCategories_Req
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsActived { get; set; }
        public IEnumerable<IFormFile>? ImageFile { get; set; }
    }
}
