using DATN.MVC.Models;

namespace DATN.MVC.Helpers
{
    public class FileHelper
    {
        public static IFormFile CreateFormFile(FileMetaData metadata)
        {
            if (string.IsNullOrEmpty(metadata.Data))
            {
                Console.WriteLine("Dữ liệu file metadata rỗng.");
                return null;
            }

            try
            {
                // Kiểm tra dữ liệu Base64
                var splitData = metadata.Data.Split(',');
                if (splitData.Length != 2)
                {
                    Console.WriteLine("Dữ liệu Base64 không hợp lệ.");
                    return null;
                }

                // Decode Base64
                var base64Data = splitData[1];
                var fileBytes = Convert.FromBase64String(base64Data);

                // Tạo MemoryStream
                var stream = new MemoryStream(fileBytes);

                return new FormFile(stream, 0, fileBytes.Length, metadata.Name, metadata.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = metadata.ContentType
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tạo FormFile từ Base64: {ex.Message}");
                return null;
            }
        }
    }
}
