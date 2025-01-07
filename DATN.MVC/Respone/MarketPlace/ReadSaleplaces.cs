using System.ComponentModel.DataAnnotations;

namespace DATN.MVC.Respone.MarketPlace
{
    public class ReadSaleplaces
    {
        [Required]
        public string Name { get; set; }

        public string Image { get; set; } // Lưu tên hoặc đường dẫn tệp hình ảnh

        public int UserId { get; set; }
      
    }
}
