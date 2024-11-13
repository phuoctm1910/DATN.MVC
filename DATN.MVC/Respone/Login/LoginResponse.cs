namespace DATN.MVC.Models.Response
{
    public class LoginResponse
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
        public string UserImage { get; set; }

        public string Token { get; set; }
    }
}
