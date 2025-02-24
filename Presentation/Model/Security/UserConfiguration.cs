using System.ComponentModel.DataAnnotations;

namespace Presentation.Model.Security
{
    public class UserConfiguration
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required, MaxLength(200, ErrorMessage = "O E-mail não pode ter mais de 200 caracteres")]
        public required string Email { get; set; }

        [Required, MaxLength(100, ErrorMessage = "O Nome não pode ter mais de 100 caracteres")]
        public required string Name { get; set; }
    }
}
