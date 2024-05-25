# API de Usuários

Esta API foi desenvolvida para gerenciar usuários. Ela permite criar, listar, listar por Id, atualizar e excluir usuários.

## Começando

Essas instruções farão com que você tenha uma cópia do projeto em execução na sua máquina local para fins de desenvolvimento e teste.

### Pré-requisitos

Para executar este projeto, você precisará ter o .NET Core 3.1 ou superior instalado na sua máquina.

### Instalação

1. Clone o repositório para a sua máquina local.
2. Navegue até a pasta do projeto no terminal.
3. Execute o comando `dotnet restore` para restaurar as dependências do projeto.
4. Execute o comando `dotnet run` para iniciar a aplicação.

## Recursos

### Rotas Assíncronas

Implementei rotas assíncronas para essa API. Isso significa que elas podem lidar com várias solicitações ao mesmo tempo sem bloquear o thread principal.

### DTOs

Utilizei Data Transfer Objects (DTOs) para enviar e receber dados. Isso ajuda a garantir que apenas os dados necessários sejam transmitidos e que os dados recebidos estejam no formato correto.

### Services

Encapsulei a lógica de negócios dessa API em serviços. Isso mantém os controladores finos e focados apenas no tratamento de solicitações HTTP, enquanto a lógica de negócios é mantida separada e pode ser reutilizada.

### Validations

Utilizei a biblioteca FluentValidation para tornar o código de validação mais fácil de ler e manter.
Também implementei regras de criação de usuário, como validação de CPF e e-mail e atualização de usuário.


## Construído com

* [.NET Core](https://dotnet.microsoft.com/download) - O framework web usado
* [Entity Framework Core](https://docs.microsoft.com/pt-br/ef/core/) - ORM para acesso a dados
* [Swagger](https://swagger.io/) - Usado para documentação da API

## Autores

* **Jonathan Henrique Galdino** - (https://github.com/jhgaldino)


