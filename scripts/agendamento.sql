-- Tabela Cliente
CREATE TABLE Cliente (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Telefone VARCHAR(20),
    Email VARCHAR(100)
);

-- Tabela Funcionario
CREATE TABLE Funcionario (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Especialidade VARCHAR(100)
);

-- Tabela Servico
CREATE TABLE Servico (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Preco DECIMAL(10, 2) NOT NULL
);

-- Tabela Agendamento
CREATE TABLE Agendamento (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Data DATETIME NOT NULL,
    ClienteId INT,
    FuncionarioId INT,
    ServicoId INT,
    FOREIGN KEY (ClienteId) REFERENCES Cliente(Id),
    FOREIGN KEY (FuncionarioId) REFERENCES Funcionario(Id),
    FOREIGN KEY (ServicoId) REFERENCES Servico(Id)
);