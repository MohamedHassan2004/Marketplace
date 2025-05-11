using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.BLL.Helper
{
    public static class ImageProcessing
    {

        public static async Task<string> UploadImageAsync(IFormFile image, string folderName, IWebHostEnvironment env)
        {
            if (image == null || image.Length == 0)
                throw new ArgumentException("Image is required");

            var fileName = GenerateFileName(image.FileName);
            var filePath = GetFilePath(fileName, folderName, env);

            await SaveImageToFileSystemAsync(image, filePath);

            return $"/images/{folderName}/{fileName}";
        }

        public static void DeleteImage(string imgUrl, IWebHostEnvironment env)
        {
            if (!string.IsNullOrEmpty(imgUrl))
            {
                var imagePath = Path.Combine(env.WebRootPath, imgUrl.TrimStart('/'));
                if (File.Exists(imagePath))
                    File.Delete(imagePath);
            }
        }
        public static async Task<string> UpdateImageAsync(string oldImageUrl, IFormFile newImage, string folderName, IWebHostEnvironment env)
        {
            // Delete old image if it exists
            DeleteImage(oldImageUrl, env);

            // Save new image
            return await UploadImageAsync(newImage, folderName, env);
        }


        private static string GenerateFileName(string originalFileName)
        {
            return Guid.NewGuid().ToString() + Path.GetExtension(originalFileName);
        }

        private static string GetFilePath(string fileName, string folderName, IWebHostEnvironment env)
        {
            var folderPath = Path.Combine(env.WebRootPath, $"images/{folderName}");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            return Path.Combine(folderPath, fileName);
        }

        private static async Task SaveImageToFileSystemAsync(IFormFile image, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
        }
    }
}
