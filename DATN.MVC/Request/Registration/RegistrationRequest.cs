using DATN.MVC.Helpers;
using Newtonsoft.Json;

namespace DATN.MVC.Request.Registration
{
    public class RegistrationRequest
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }

        public DateTime Birth { get; set; }

        public bool Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public long CreatedDateUnix { get; set; }

        public string BirthFormatted
        {
            get => Birth.ToString("yyyy-MM-dd");
        }
    }
}
