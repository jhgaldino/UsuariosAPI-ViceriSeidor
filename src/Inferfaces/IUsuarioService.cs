using ErrorOr;
using UsuariosAPI_ViceriSeidor.src.Dtos;

namespace UsuariosAPI_ViceriSeidor.src.Inferfaces
{
    public interface IUsuarioService
    {
        Task<ErrorOr<AdicionarUsuarioResponse>> CadastrarUsuario(AdicionarUsuarioRequest request);
        Task<IEnumerable<ListarUsuarioResponse>> ListarUsuarios();
        Task<ErrorOr<ListarUsuarioResponse>> ListarUsuarioPorId(int id);
        Task<ErrorOr<AtualizarUsuarioResponse>> AtualizarUsuario(int id, AtualizarUsuarioRequest request);
        Task<ErrorOr<bool>> RemoverUsuarioPorId(int id);
    }
}
