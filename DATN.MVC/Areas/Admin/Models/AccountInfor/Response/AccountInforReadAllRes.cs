namespace DATN.MVC.Areas.Admin.Models.AccountInfor.Response
{
    public class AccountInforReadAllRes
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }

        public bool IsActived { get; set; }
        public int CreatedDate { get; set; }
        public int UpdatedDate { get; set; }
    }
}
