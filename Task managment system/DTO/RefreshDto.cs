using System.ComponentModel.DataAnnotations;

namespace Task_managment_system.DTO
{
    public class RefreshDto
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}
