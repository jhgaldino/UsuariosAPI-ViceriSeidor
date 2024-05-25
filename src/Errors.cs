using ErrorOr;

namespace UsuariosAPI_ViceriSeidor.src
{
    public static class Errors
    {
        public static class Usuario
        {
            public static readonly Error DuplicateEmailOrCpf = Error.Conflict(
                code: "Usuario.DuplicateEmailOrCpf",
                description: "Usuário com este e-mail ou CPF já existe.");

            public static readonly Error NotFound = Error.NotFound(
                code: "Usuario.NotFound",
                description: "Usuário não encontrado.");
        }

        public class None
        {
            public static readonly Error NotFound = Error.NotFound(
                code: "None.NotFound",
                description: "Nenhum registro encontrado.");
        }
    }
}

