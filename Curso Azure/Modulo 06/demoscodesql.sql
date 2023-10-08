IF OBJECT_ID('dbo.Empleados', 'U') IS NOT NULL
DROP TABLE dbo.Empleados
GO

CREATE TABLE dbo.Empleados
(
IdEmpleado INT NOT NULL PRIMARY KEY,
Nombre [NVARCHAR](50) NOT NULL,
Localizacion [NVARCHAR](50) NOT NULL,
);
GO

INSERT INTO Empleados
([Idempleado], [Nombre], [Localizacion])
VALUES
(1, N'Jared', N'Australia'),
(2, N'Nikita', N'India'),
(3, N'Ton', N'Germany'),
(4, N'Jake', N'United States')
GO
