-- Crear base de datos
CREATE DATABASE [Pokemon]
GO

USE [Pokemon];
GO

-- Crear tabla Usuarios
CREATE TABLE [Usuarios](
    [UsuarioID] INT IDENTITY(1,1) PRIMARY KEY,
    [NombreUsuario] NVARCHAR(50) NOT NULL,
    [Contraseña] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL
);
GO

-- Crear tabla Cartas
CREATE TABLE [Cartas](
    [ID] INT IDENTITY(1,1) PRIMARY KEY,
    [Pokedex] INT,
    [Nombre] NVARCHAR(100) NOT NULL,
    [Tipo] NVARCHAR(50) NOT NULL,
    [Region] NVARCHAR(50),
    [Rareza] NVARCHAR(50) NOT NULL,
    [HP] INT,
    [Debilidad] NVARCHAR(20),
    [ImagenURL] NVARCHAR(MAX) NOT NULL,
    [Habilidad] NVARCHAR(20),
    [Categoria] NVARCHAR(50)
);
GO

-- Crear tabla Colecciones
CREATE TABLE [Colecciones](
    [ColeccionID] INT IDENTITY(1,1) PRIMARY KEY,
    [UsuarioID] INT NOT NULL,
    [CartaID] INT NOT NULL,
    [Cantidad] INT NOT NULL
);
GO

-- Insertar datos iniciales en Cartas
INSERT INTO [Cartas] ([Pokedex], [Nombre], [Tipo], [Region], [Rareza], [HP], [Debilidad], [ImagenURL], [Habilidad], [Categoria]) 
VALUES 
(1, 'Bulbasaur', 'Planta', 'Kanto', '1 diamante', 70, 'Fuego', 'https://i.imgur.com/g0eBLzI.png', 'N', 'Pokemon'),
(2, 'Ivysaur', 'Planta', 'Kanto', '2 diamantes', 90, 'Fuego', 'https://i.imgur.com/dUZ8Ptn.png', 'N', 'Pokemon'),
(3, 'Venusaur', 'Planta', 'Kanto', '3 diamantes', 160, 'Fuego', 'https://i.imgur.com/04EBvpM.png', 'N', 'Pokemon'),
(4, 'Venusaur EX', 'Planta', 'Kanto', '4 diamantes', 190, 'Fuego', 'https://imgur.com/RllguFR.png', 'N', 'Pokemon'),
(5, 'Caterpie', 'Planta', 'Kanto', '1 diamante', 50, 'Fuego', 'https://imgur.com/1aNZEPk.png', 'N', 'Pokemon');
GO