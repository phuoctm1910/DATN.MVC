using System.ComponentModel.DataAnnotations;

namespace DATN.MVC.Areas.Admin.Models.ManagerProduct.Request
{
    public class ProductReq
    {
        // Kiểm tra Id bắt buộc (Thêm điều kiện cho Id > 0)
        [Required(ErrorMessage = "Mã sản phẩm là bắt buộc.")]
        public int Id { get; set; }

        // Kiểm tra tên sản phẩm bắt buộc
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự.")]
        public string Name { get; set; }

        // Kiểm tra giá phải lớn hơn 0
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0.")]
        public decimal Price { get; set; }

        // Kiểm tra số lượng phải lớn hơn 0
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; }

        // Kiểm tra kích thước hợp lệ (nếu cần thêm các validation cho Size, có thể bổ sung)
        [StringLength(50, ErrorMessage = "Kích thước không được vượt quá 50 ký tự.")]
        public string Size { get; set; }

        // Kiểm tra mô tả không vượt quá 500 ký tự
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        public string Description { get; set; }

        // Kiểm tra hình ảnh hợp lệ (Có thể dùng URL nếu cần)
        [StringLength(255, ErrorMessage = "Đường dẫn hình ảnh không được vượt quá 255 ký tự.")]
        public string Image { get; set; }

        // Kiểm tra trạng thái hoạt động của sản phẩm (True/False)
        public bool IsActived { get; set; }

       

        // Kiểm tra Id danh mục sản phẩm
        [Required(ErrorMessage = "Danh mục sản phẩm là bắt buộc.")]
        public int CategoryId { get; set; }

        // Kiểm tra Id nơi bán (SalePlaceId)
        [Required(ErrorMessage = "Nơi bán là bắt buộc.")]
        public int SalePlaceId { get; set; }
    }
}
