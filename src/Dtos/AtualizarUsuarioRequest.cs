using System.ComponentModel.DataAnnotations;

namespace UsuariosAPI_ViceriSeidor.src.Dtos
{
    public class AtualizarUsuarioRequest
    {
        #pragma warning disable CS8618
        [Required]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string CPF { get; set; }
        
        [Required]
        public DateTime DataNasc { get; set; }
        #pragma warning restore CS8618
    }
}
