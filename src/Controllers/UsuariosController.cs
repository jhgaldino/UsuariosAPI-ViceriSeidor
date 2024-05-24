using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UsuariosAPI_ViceriSeidor.src.Models;
using UsuariosAPI_ViceriSeidor.src.Data;
using UsuariosAPI_ViceriSeidor.src.Services;
using UsuariosAPI_ViceriSeidor.src.Dtos;

namespace UsuariosAPI_ViceriSeidor.src.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UsuarioService _usuarioService;

        public UsuariosController(AppDbContext context, UsuarioService usuarioService)
        {
            _context = context;
            _usuarioService = usuarioService;
            
        }

        // Método de cadastro de usuário
        [HttpPost]
        public async Task<ActionResult<Usuario>> Post([FromBody] AdicionarUsuarioRequest request)
        {
            var result = await _usuarioService.CadastrarUsuario(request);

            if (result.FirstError.Type == ErrorOr.ErrorType.Validation)
            {
                return BadRequest(result.Errors);
            }
            return CreatedAtRoute("GetUsuario", new { id = result.Value.Id }, result.Value);
        }


        // Método de listagem de usuários
        public async Task<List<Usuario>> ListarUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // Método de listagem de usuário por id
        public async Task<Usuario?> ListarUsuarioPorId(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        // Método de atualização de usuário
        public async Task AtualizarUsuarioPorId(int id)
        {
            _context.Entry(id).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Método de remoção de usuário por id
        public async Task RemoverUsuarioPorId(int id)
        {
            var usuario = await _usuarioService.ListarUsuarioPorId(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }
    }
}
