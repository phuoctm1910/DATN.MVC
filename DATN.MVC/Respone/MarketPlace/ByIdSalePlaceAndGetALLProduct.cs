namespace DATN.MVC.Respone.MarketPlace
{
    public class ByIdSalePlaceAndGetALLProduct
    {
        public SalePlaceRes saleplace { get; set; } = new SalePlaceRes();
        public List<GetAllProductRes> Products { get; set; } = new List<GetAllProductRes>();
    }
}
