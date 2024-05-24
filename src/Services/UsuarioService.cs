using System.Collections.Generic;
using System.Threading.Tasks;
using UsuariosAPI_ViceriSeidor.src.Data;
using UsuariosAPI_ViceriSeidor.src.Dtos;
using UsuariosAPI_ViceriSeidor.src.Models;
using UsuariosAPI_ViceriSeidor.src.Extensions;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using ErrorOr;


namespace UsuariosAPI_ViceriSeidor.src.Services
{
    public interface IUsuarioService
    {
        Task<ErrorOr<AdicionarUsuarioResponse>> CadastrarUsuario(AdicionarUsuarioRequest request);
        Task<List<Usuario>> ListarUsuarios();
        Task<Usuario?> ListarUsuarioPorId(int id);
        Task<ErrorOr<Usuario>> AtualizarUsuario(int id, AdicionarUsuarioRequest request);
        // Task<ErrorOr<None>> RemoverUsuarioPorId(int id);
    }

    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;
        private readonly IValidator<AdicionarUsuarioRequest> _adicionarUsuarioRequestValidator;

        public UsuarioService(AppDbContext context, IValidator<AdicionarUsuarioRequest> adicionarUsuarioRequestValidator)
        {
            _context = context;
            _adicionarUsuarioRequestValidator = adicionarUsuarioRequestValidator;
        }

        public async Task<ErrorOr<AdicionarUsuarioResponse>> CadastrarUsuario(AdicionarUsuarioRequest request)
        {
            var validationResult = _adicionarUsuarioRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return validationResult.Errors.ToValidation();
            }

            if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email || u.CPF == request.CPF))
            {
                return Errors.Usuario.DuplicateEmailOrCpf;
            }

            var usuario = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                CPF = request.CPF,
                DataNasc = request.DataNasc,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha)
            };

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return new AdicionarUsuarioResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                CPF = usuario.CPF,
                DataNasc = usuario.DataNasc
            };
        }

        public async Task<List<Usuario>> ListarUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario?> ListarUsuarioPorId(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<ErrorOr<Usuario>> AtualizarUsuario(int id, AdicionarUsuarioRequest request)
        {
            var validationResult = _adicionarUsuarioRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return validationResult.Errors.ToValidation();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return Errors.Usuario.NotFound;
            }

            if (await _context.Usuarios.AnyAsync(u => u.Id != id && (u.Email == request.Email || u.CPF == request.CPF)))
            {
                return Errors.Usuario.DuplicateEmailOrCpf;
            }

            usuario.Nome = request.Nome;
            usuario.Email = request.Email;
            usuario.CPF = request.CPF;
            usuario.DataNasc = request.DataNasc;
            if (!string.IsNullOrEmpty(request.Senha))
            {
                usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha);
            }

            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return usuario;
        }
        /*
        public async Task<ErrorOr<None>> RemoverUsuarioPorId(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return Errors.Usuario.NotFound;
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Result.Success;
        }*/
    }
}
