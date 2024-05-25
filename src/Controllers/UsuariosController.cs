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

        /// <summary>
        /// Cadastro de usuário. Possui validador de CPF e e-mail. 
        /// Aponta erro caso tente cadastro de usuário com CPF ou e-mail já existente ou inválido.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        /// {
        ///     "Nome": "Joao",
        ///     "Email": "email2@teste.com",
        ///     "Senha": "123456",
        ///     "CPF":"90007533098",(OBS : Use um CPF real ou um gerado aleatoriamente)
        ///     "DataNasc":"2009-01-01"
        /// }
        /// </remarks>
        /// <response code="201">Retorna os dados do usuário cadastrado</response>
        /// <response code="400">Retorna erro de validação de CPF ou email</response>
        /// <response code="409">Retorna conflito de dados de CPF ou email</response>
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
        /// <summary>
        /// Lista todos os Usuários e seus dados, exceto a senha.
        /// é possível retornar somente dados nao sensíveis, porem não foi solicitado.
        /// </summary>
        /// <response code="200">Retorna os usuários cadastrados</response>
        /// <response code="404">Retorna que nenhum usuário não foi encontrado</response>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListarUsuarioResponse>>> Get()
        {
            var usuarios = await _usuarioService.ListarUsuarios();

            if (!usuarios.Any())
            {
                return NotFound(new { message = "Nenhum usuário encontrado" });
            }

            return Ok(usuarios);
        }


        // Método de listagem de usuário por id
        /// <summary>
        /// Exibe um usuário com o seu id passado.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///         /api/usuarios/1
        /// </remarks>
        /// <response code="200">Retorna os dados do usuário cadastrado</response>
        /// <response code="400">Retorna erro de validação de CPF ou email</response>
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
        /// <summary>
        ///  Atualiza um usuário por id.
        ///  Nao é possível alterar a senha do usuário. Poderia ser implementado, mas não foi solicitado.
        /// </summary>
        /// Exemplo de requisição:
        ///       /api/usuarios/1
        /// {
        ///     "Nome": "Joao",
        ///     "Email": "email2@teste.com",
        ///     "Senha": "123456",
        ///     "CPF":"90007533098",(OBS : Use um CPF real ou um gerado aleatoriamente)
        ///     "DataNasc":"2009-01-01"
        /// }
        /// <response code="200">Retorna os dados do usuário atualizado</response>
        /// <response code="400">Retorna erro de validação de CPF ou email</response>
        /// <response code="404">Retorna que o usuário não foi encontrado</response>
        /// <response code="409">Retorna conflito de dados de CPF ou email</response>
        /// 
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
        /// <summary>
        /// Remove o usuário por id.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///         /api/usuarios/1
        /// 
        /// 
        /// </remarks>
        /// <response code="200">Retorna mensagem de sucesso.</response>
        /// <response code="404">Retorna que o usuário não foi encontrado</response>
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
