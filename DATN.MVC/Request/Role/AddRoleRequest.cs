using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DATN.MVC.Request.Role
{
    public class AddRoleRequest
    {
        public string Name { get; set; }
        public bool IsActived { get; set; }
 
    }
}
