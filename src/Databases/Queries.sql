-- Como estou usando Entity Framework, faremos consultas T-SQL equivalentes aos metodos do CRUD.

-- Criar a tabela Usuarios
CREATE TABLE Usuarios (
	ID INT PRIMARY KEY IDENTITY(1,1),
	Nome NVARCHAR(100) NOT NULL,
	Email NVARCHAR(100) NOT NULL,
	Senha NVARCHAR(100) NOT NULL,
	CPF NVARCHAR(11) NOT NULL,
	DataNasc DATE NOT NULL
);
-- Insercao
INSERT INTO Usuarios (Nome, Email, Senha, CPF, DataNasc) VALUES ('Joao', 'joao@gmail.com', '123456', '12345678901', '1990-01-01');
-- Melhor fazer a conversao de data no client-side, para evitar problemas de timezone.
-- Selecao
SELECT * FROM Usuarios;

-- Selecao por ID
SELECT * FROM Usuarios WHERE ID = 1;

-- Atualizacao
UPDATE Usuarios SET Nome = 'Joao Silva' WHERE ID = 1;

-- Delecao
DELETE FROM Usuarios WHERE ID = 1;