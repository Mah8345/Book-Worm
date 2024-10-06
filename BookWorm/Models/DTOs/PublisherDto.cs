using System.ComponentModel.DataAnnotations;

namespace BookWorm.Models.DTOs
{
    public class PublisherDto
    {
        [MaxLength(50)]
        public required string Name { get; set; }
        public string? About { get; set; }
        public IFormFile? PhotoFile { get; set; }
    }
}
