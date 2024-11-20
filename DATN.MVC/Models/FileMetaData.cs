namespace DATN.MVC.Models
{
    public class FileMetaData
    {
        public string Name { get; set; } // Tên file
        public string ContentType { get; set; } // Loại file (MIME type)
        public string Data { get; set; }
    }
}
