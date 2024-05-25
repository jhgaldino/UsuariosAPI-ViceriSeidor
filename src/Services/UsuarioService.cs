using FluentValidation;
using ErrorOr;
using UsuariosAPI_ViceriSeidor.src.Data;
using UsuariosAPI_ViceriSeidor.src.Dtos;
using UsuariosAPI_ViceriSeidor.src.Models;
using UsuariosAPI_ViceriSeidor.src.Extensions;
using Microsoft.EntityFrameworkCore;

namespace UsuariosAPI_ViceriSeidor.src.Services
{
    // Interface de serviço de usuário
    public interface IUsuarioService
    {
        Task<ErrorOr<AdicionarUsuarioResponse>> CadastrarUsuario(AdicionarUsuarioRequest request);
        Task<IEnumerable<ListarUsuarioResponse>> ListarUsuarios();
        Task<ErrorOr<ListarUsuarioResponse>> ListarUsuarioPorId(int id);
        Task<ErrorOr<AtualizarUsuarioResponse>> AtualizarUsuario(int id, AtualizarUsuarioRequest request);
        Task<ErrorOr<bool>> RemoverUsuarioPorId(int id);
    }


    // Implementação do serviço de usuário
    public class UsuarioService : IUsuarioService
    {
        private readonly UsuariosContext _context;
        private readonly IValidator<AdicionarUsuarioRequest> _adicionarUsuarioRequestValidator;
        private readonly IValidator<AtualizarUsuarioRequest> _atualizarUsuarioRequestValidator;

        public UsuarioService(UsuariosContext context, IValidator<AdicionarUsuarioRequest> adicionarUsuarioRequestValidator, IValidator<AtualizarUsuarioRequest> atualizarUsuarioRequestValidator)
        {
            _context = context;
            _adicionarUsuarioRequestValidator = adicionarUsuarioRequestValidator;
            _atualizarUsuarioRequestValidator = atualizarUsuarioRequestValidator;
        }

        public async Task<ErrorOr<AdicionarUsuarioResponse>> CadastrarUsuario(AdicionarUsuarioRequest request)
        {
            // Valida as entradas da requisição
            var validationResult = _adicionarUsuarioRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return validationResult.Errors.ToValidation();
            }

            // Valida se o email ou o CPF já existe na base
            if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email || u.CPF == request.CPF))
            {
                return Errors.Usuario.DuplicateEmailOrCpf;
            }

            // Insere o usuário na base
            var usuario = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                CPF = request.CPF,
                DataNasc = request.DataNasc,
                // Senha é criptografada em hash antes de ser salva no banco
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha)
            };

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            // Mapeamento do objeto de resposta
            return new AdicionarUsuarioResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                CPF = usuario.CPF,
                DataNasc = usuario.DataNasc
            };
        }

        public async Task<IEnumerable<ListarUsuarioResponse>> ListarUsuarios()
        {
            // Retorna os dados não confidenciais do usuário
            return (await _context.Usuarios.ToListAsync())
                .Select(u => new ListarUsuarioResponse {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    CPF = u.CPF,
                    DataNasc = u.DataNasc
                });
        }

        public async Task<ErrorOr<ListarUsuarioResponse>> ListarUsuarioPorId(int id)
        {
            // Valida que o usuário existe
            var result = await _context.Usuarios.FindAsync(id);
            if (result == null)
            {
                return Errors.Usuario.NotFound;
            }

            // Retorna os dados não confidenciais do usuário
            return new ListarUsuarioResponse {
                Id = result.Id,
                Nome = result.Nome,
                Email = result.Email,
                CPF = result.CPF,
                DataNasc = result.DataNasc
            };
        }

        public async Task<ErrorOr<AtualizarUsuarioResponse>> AtualizarUsuario(int id, AtualizarUsuarioRequest request)
        {
            // Valida as entradas da requisição
            var validationResult = _atualizarUsuarioRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return validationResult.Errors.ToValidation();
            }

            // Valida que o usuário existe
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return Errors.Usuario.NotFound;
            }

            // Valida se o email ou o CPF já existe na base
            if (await _context.Usuarios.AnyAsync(u => u.Id != id && (u.Email == request.Email || u.CPF == request.CPF)))
            {
                return Errors.Usuario.DuplicateEmailOrCpf;
            }

            // Atualiza os dados
            usuario.Nome = request.Nome;
            usuario.Email = request.Email;
            usuario.CPF = request.CPF;
            usuario.DataNasc = request.DataNasc;

            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return new AtualizarUsuarioResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                CPF = usuario.CPF,
                DataNasc = usuario.DataNasc
            };
        }

        public async Task<ErrorOr<bool>> RemoverUsuarioPorId(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return Errors.Usuario.NotFound;
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
