namespace DATN.MVC.Models.Request
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
