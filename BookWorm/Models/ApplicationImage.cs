using System.ComponentModel.DataAnnotations;

namespace BookWorm.Models
{
    public class ApplicationImage
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string? FileName { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadedOn { get; } = DateTime.Now;
        

        //todo: remember to apply the same logic to add the uploaded files to the wwwroot/uploads/images folder
        public string FilePath =>
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images", Id.ToString());
    }
}

