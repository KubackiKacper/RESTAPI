using System.ComponentModel.DataAnnotations;

namespace RESTAPI_Backend.DTOs
{
    public class SaveToDoDTO
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime ExpiryDateTime { get; set; }
        [Required]
        [Range(0, 100,
        ErrorMessage = "PercentComplete must be between 0 and 100.")]
        public int PercentComplete { get; set; }
        [Required]
        public bool IsDone { get; set; }
    }
}
