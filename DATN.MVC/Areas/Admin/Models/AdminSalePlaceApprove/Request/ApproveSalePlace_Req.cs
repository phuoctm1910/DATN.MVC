namespace DATN.MVC.Request.SalePlaceApprove
{
	public class ApproveSalePlace_Req
	{
		public int Id { get; set; } // ID của SalePlace cần duyệt
		public bool IsApproved { get; set; } // Trạng thái duyệt: true (đồng ý), false (từ chối)
	}
}
