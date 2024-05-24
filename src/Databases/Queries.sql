-- Insercao
INSERT INTO Usuarios (Nome, Email, Senha, DataNasc) VALUES ('Joao', 'joao@gmail.com', '123456', '1990-01-01');

-- Selecao
SELECT * FROM Usuarios;

-- Selecao por ID
SELECT * FROM Usuarios WHERE ID = 1;

-- Atualizacao
UPDATE Usuarios SET Nome = 'Joao Silva' WHERE ID = 1;

-- Delecao
DELETE FROM Usuarios WHERE ID = 1;