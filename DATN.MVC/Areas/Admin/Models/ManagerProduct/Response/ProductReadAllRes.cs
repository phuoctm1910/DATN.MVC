namespace DATN.MVC.Areas.Admin.Models.ManagerProduct.Response
{
    public class ProductReadAllRes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool IsActived { get; set; }
    
        public int CreatedDate { get; set; }
        public int? UpdatedDate { get; set; }

        public int CategoryId { get; set; }
        public int SalePlaceId { get; set; }
    }

}