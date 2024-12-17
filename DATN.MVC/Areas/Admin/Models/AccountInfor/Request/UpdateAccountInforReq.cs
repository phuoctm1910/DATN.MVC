using System.ComponentModel.DataAnnotations;

namespace DATN.MVC.Areas.Admin.Models.AccountInfor.Request
{
    public class UpdateAccountInforReq
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Trạng thái người dùng là bắt buộc.")]
        public bool IsActived { get; set; }
    
    }
}
