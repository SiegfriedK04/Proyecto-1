
CREATE DATABASE Calculadora;

USE Calculadora;

CREATE TABLE Operaciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Operacion VARCHAR(50) NOT NULL,
    Valor1 DECIMAL(18, 2) NOT NULL,
    Valor2 DECIMAL(18, 2) NULL,
    Resultado DECIMAL(18, 2) NOT NULL
);
