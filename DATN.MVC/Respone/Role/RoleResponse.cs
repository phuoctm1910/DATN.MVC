using System.Text.Json.Serialization;

namespace DATN.MVC.Respone.Role
{
    public class RoleResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActived { get; set; }
        
    }
}
