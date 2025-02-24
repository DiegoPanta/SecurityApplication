using System.ComponentModel.DataAnnotations;

namespace Presentation.Model.Clientes
{
    public class Cliente
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(200, ErrorMessage = "O Nome não pode ter mais de 200 caracteres.")]
        public required string Name { get; set; }

        [Required, MaxLength(100, ErrorMessage = "O E-mail não pode ter mais de 100 caracteres")]
        [EmailAddress(ErrorMessage = "Por favor, forneça um email válido.")]
        public required string Email { get; set; }

        [Required, MaxLength(15, ErrorMessage = "O E-mail não pode ter mais de 15 caracteres")]
        [Phone(ErrorMessage = "Por favor, forneça um número de telefone válido.")]
        public required string Phone { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DataType(DataType.Date, ErrorMessage = "A data fornecida é inválida.")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "O campo 'Ativo' é obrigatório.")]
        public bool Active { get; set; }
    }
}
