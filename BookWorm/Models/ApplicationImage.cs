namespace BookWorm.Models
{
    public class ApplicationImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadedOn { get; set; } = DateTime.Now;
        

        //todo: remember to apply the same logic to add the uploaded files to the wwwroot/uploads/images folder
        public string FilePath =>
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images", Id.ToString());
    }
}

