namespace DATN.MVC.Respone.MarketPlace
{
    public class GetALLProductAndCategory
    {
        public List<CaterorisRes> Categories { get; set; } = new List<CaterorisRes>();
        public List<GetAllProductRes> Products { get; set; } = new List<GetAllProductRes>();
        public List<SalePlaceRes> salePlace {  get; set; } = new List<SalePlaceRes>();
    }
}
