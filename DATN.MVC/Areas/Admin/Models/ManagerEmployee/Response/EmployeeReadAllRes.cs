namespace DATN.MVC.Areas.Admin.Models.ManagerEmployee.Response
{
    public class EmployeeReadAllRes
    {
        public int Id { get; set; }
        public string UserName { get; set; }  // Chú ý là UserName
        public string PasswordHash { get; set; }  // PasswordHash
        public int CreatedDate { get; set; }
        public bool IsActived { get; set; }
    }
}
