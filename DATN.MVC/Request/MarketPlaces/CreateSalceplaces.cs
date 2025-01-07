using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DATN.MVC.Request.MarketPlaces
{
    public class CreateSalceplaces
    {
        [Required(ErrorMessage = "Tên gian hàng là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên gian hàng không được vượt quá 100 ký tự")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Hình ảnh là bắt buộc")]
        public IEnumerable<IFormFile> ImageFile { get; set; }

    }
}
