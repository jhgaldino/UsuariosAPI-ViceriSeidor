using FluentValidation;
using UsuariosAPI_ViceriSeidor.src.Dtos;
namespace UsuariosAPI_ViceriSeidor.src.Validations
{
    public class AtualizarUsuarioRequestValidator : AbstractValidator<AtualizarUsuarioRequest>
    {
        public AtualizarUsuarioRequestValidator()
        {
            // Validações para atualizar um usuário
            RuleFor(x => x.Nome)
                .MaximumLength(100)
                .WithMessage("Nome deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Email)
                .MaximumLength(100)
                .WithMessage("Email deve ter no máximo 100 caracteres.")
                .EmailAddress()
                .WithMessage("Email inválido.")
                .When(x => !string.IsNullOrEmpty(x.Email));
        }
        // Como CPF é unico, não é possível alterar o CPF de um usuário
    }



}
