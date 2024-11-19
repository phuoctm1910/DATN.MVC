namespace DATN.MVC.Areas.Admin.Models
{
    public class UserRegistration
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime Birth { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? RoleName { get; set; }
        
    }

}
