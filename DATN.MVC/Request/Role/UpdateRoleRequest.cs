using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DATN.MVC.Request.Role
{
    public class UpdateRoleRequest
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        public bool IsActived { get; set; }
     
    }
}
