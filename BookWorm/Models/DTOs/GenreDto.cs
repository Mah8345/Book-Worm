using System.ComponentModel.DataAnnotations;

namespace BookWorm.Models.DTOs
{
    public class GenreDto
    {
        [MaxLength(50)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required IFormFile PhotoFile { get; set; }
    }
}
