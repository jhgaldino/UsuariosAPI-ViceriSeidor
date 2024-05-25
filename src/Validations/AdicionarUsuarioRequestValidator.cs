using FluentValidation;
using UsuariosAPI_ViceriSeidor.src.Dtos;

namespace UsuariosAPI_ViceriSeidor.src.Validations
{
    public class AdicionarUsuarioRequestValidator : AbstractValidator<AdicionarUsuarioRequest>
    {
        public AdicionarUsuarioRequestValidator()
        {
            // Regras de validação para o cadastro de um novo usuário
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Nome é obrigatório");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email é obrigatório")
                .EmailAddress()
                .WithMessage("Email inválido");

            RuleFor(x => x.Senha)
                .NotEmpty()
                .WithMessage("Senha é obrigatória")
                .MinimumLength(6)
                .WithMessage("Senha deve ter no mínimo 6 caracteres"); // Exemplo para facilitar os testes, pode ser alterado depois

            RuleFor(x => x.CPF)
                .NotEmpty()
                .WithMessage("CPF é obrigatório")
                .Must(CPFValido)
                .WithMessage("CPF inválido");

            RuleFor(x => x.DataNasc)
                .NotEmpty()
                .WithMessage("Data de nascimento é obrigatória");
        }

        // Validação de CPF, retirado de https://gist.github.com/rdakar/dba890b5e2cbdeb7c62c0dee9f627a7f
        private static bool CPFValido(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}
