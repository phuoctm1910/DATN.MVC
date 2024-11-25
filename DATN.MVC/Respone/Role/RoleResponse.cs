using System.Text.Json.Serialization;

namespace DATN.MVC.Respone.Role
{
    public class RoleResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        public bool IsActived { get; set; }
        
    }
}
