# API de Usu�rios

Esta API foi desenvolvida para gerenciar usu�rios. Ela permite criar, listar, listar por Id, atualizar e excluir usu�rios.

## Come�ando

Essas instru��es far�o com que voc� tenha uma c�pia do projeto em execu��o na sua m�quina local para fins de desenvolvimento e teste.

### Pr�-requisitos

Para executar este projeto, voc� precisar� ter o .NET Core 3.1 ou superior instalado na sua m�quina.

### Instala��o

1. Clone o reposit�rio para a sua m�quina local.
2. Navegue at� a pasta do projeto no terminal.
3. Execute o comando `dotnet restore` para restaurar as depend�ncias do projeto.
4. Execute o comando `dotnet run` para iniciar a aplica��o.

## Recursos

### Rotas Ass�ncronas

Implementei rotas ass�ncronas para essa API. Isso significa que elas podem lidar com v�rias solicita��es ao mesmo tempo sem bloquear o thread principal.

### DTOs

Utilizei Data Transfer Objects (DTOs) para enviar e receber dados. Isso ajuda a garantir que apenas os dados necess�rios sejam transmitidos e que os dados recebidos estejam no formato correto.

### Services

Encapsulei a l�gica de neg�cios dessa API em servi�os. Isso mant�m os controladores finos e focados apenas no tratamento de solicita��es HTTP, enquanto a l�gica de neg�cios � mantida separada e pode ser reutilizada.

### Validations

Utilizei a biblioteca FluentValidation para tornar o c�digo de valida��o mais f�cil de ler e manter.
Tamb�m implementei regras de cria��o de usu�rio, como valida��o de CPF e e-mail e atualiza��o de usu�rio.


## Constru�do com

* [.NET Core](https://dotnet.microsoft.com/download) - O framework web usado
* [Entity Framework Core](https://docs.microsoft.com/pt-br/ef/core/) - ORM para acesso a dados
* [Swagger](https://swagger.io/) - Usado para documenta��o da API

## Autores

* **Jonathan Henrique Galdino** - (https://github.com/jhgaldino)


