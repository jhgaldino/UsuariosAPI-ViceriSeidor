using System.ComponentModel.DataAnnotations;

namespace UsuariosAPI_ViceriSeidor.src.Dtos
{
    // desabilitando erro de falta de inicialização dos campos
    #pragma warning disable CS8618
    public class AdicionarUsuarioRequest
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [MinLength(6)]
        public string Senha { get; set; }
        
        [Required]
        public string CPF { get; set; }
        
        [Required]
        public DateTime DataNasc { get; set; }
    }
        #pragma warning restore CS8618

}
