namespace UsuariosAPI_ViceriSeidor.src.Dtos
{
    #pragma warning disable CS8618
    public class ListarUsuarioResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public DateTime DataNasc { get; set; }
    }
    #pragma warning restore CS8618
}
