using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UsuariosAPI_ViceriSeidor.src.Models;
using UsuariosAPI_ViceriSeidor.src.Data;
using UsuariosAPI_ViceriSeidor.src.Services;
using UsuariosAPI_ViceriSeidor.src.Dtos;
using Microsoft.AspNetCore.Http.Extensions;

namespace UsuariosAPI_ViceriSeidor.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
            
        }

        // Método de cadastro de usuário
        [HttpPost]
        public async Task<ActionResult<AdicionarUsuarioResponse>> Post([FromBody] AdicionarUsuarioRequest request)
        {
            var result = await _usuarioService.CadastrarUsuario(request);

            if (result.IsError)
            {
                if (result.FirstError.Type == ErrorOr.ErrorType.Validation)
                {
                    return BadRequest(result.Errors);
                }
                else if (result.FirstError == Errors.Usuario.DuplicateEmailOrCpf)
                {
                    return Conflict(new { message = "Usuário com este e-mail ou CPF já existe." });
                }
                else
                {
                    return StatusCode(500, result.Errors);
                }
            }

            var uri = $"{Request.GetEncodedUrl()}/{result.Value.Id}"; 
            return Created(uri, result.Value);
        }


        // Método de listagem de usuários
        [HttpGet]
        public async Task<IEnumerable<ListarUsuarioResponse>> Get()
        {
            return await _usuarioService.ListarUsuarios();
        }

        // Método de listagem de usuário por id
        [HttpGet("{id}")]
        public async Task<ActionResult<ListarUsuarioResponse>> GetId(int id)
        {
            var result = await _usuarioService.ListarUsuarioPorId(id);

            if (result.IsError)
            {
                if (result.FirstError == Errors.Usuario.NotFound)
                {
                    return NotFound(new { message = "Usuário não encontrado" });
                }
                else {
                    return StatusCode(500, result.Errors);
                }
            }

            return Ok(result.Value);
        }

        // Método de atualização de usuário
        [HttpPut("{id}")]
        public async Task<ActionResult<AtualizarUsuarioResponse>> Put(int id, [FromBody] AtualizarUsuarioRequest request)
        {
            var result = await _usuarioService.AtualizarUsuario(id, request);

            if (result.IsError)
            {
                if (result.FirstError.Type == ErrorOr.ErrorType.Validation)
                {
                    return BadRequest(result.Errors);
                }
                else if (result.FirstError == Errors.Usuario.DuplicateEmailOrCpf)
                {
                    return Conflict(new { message = "Usuário com este e-mail ou CPF já existe." });
                }
                else {
                    return StatusCode(500, result.Errors);
                }
            }

            return Ok(result.Value);
        }

        // Método de remoção de usuário por id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _usuarioService.RemoverUsuarioPorId(id);

            if (result.IsError)
            {
                if (result.FirstError == Errors.Usuario.NotFound)
                {
                    return NotFound(new { message = "Usuário não encontrado" });
                }
                else {
                    return BadRequest(result.Errors);
                }
            }

            return Ok(new { message = "Usuário excluído com sucesso" });
        }
    }
}
