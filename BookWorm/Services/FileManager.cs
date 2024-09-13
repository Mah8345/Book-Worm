using BookWorm.Models;

namespace BookWorm.Services
{
    public static class FileManager
    {
        public static async Task SavePhotoAsync(Guid photoId, IFormFile photoFile)
        {
            var imageFormat = Path.GetExtension(photoFile.FileName);
            var fileName = $"{photoId}{imageFormat}";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
            Console.WriteLine(path);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await photoFile.CopyToAsync(stream);
            }
        }
    }
}
