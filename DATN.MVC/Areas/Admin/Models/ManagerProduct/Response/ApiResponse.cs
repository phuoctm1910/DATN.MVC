namespace DATN.MVC.Areas.Admin.Models.ManagerProduct.Response
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public List<T> Data { get; set; }
    }
}
