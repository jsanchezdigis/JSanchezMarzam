CREATE DATABASE JSanchezMarzam
USE JSanchezMarzam
GO

CREATE TABLE Usuario(
	IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
	UserName VARCHAR(50),
	Password VARCHAR(50)
)
GO
INSERT INTO Usuario(UserName,Password)VALUES('jsanchez','12345')
CREATE TABLE Medicamento(
	IdMedicamento INT IDENTITY(1,1) PRIMARY KEY,
	Nombre VARCHAR(50),
	Precio DECIMAL(18,2),
	Imagen VARCHAR(MAX)
)
GO

ALTER PROCEDURE MedicamentoAdd --'Paracetamol',50.5,'gadfgs'
@Nombre VARCHAR(50),
@Precio DECIMAL(18,2),
@Imagen VARCHAR(MAX)
AS
INSERT INTO Medicamento(Nombre,Precio,Imagen)VALUES(@Nombre,@Precio,@Imagen)
GO

ALTER PROCEDURE MedicamentoUpdate
@IdMedicamento INT,
@Nombre VARCHAR(50),
@Precio DECIMAL(18,2),
@Imagen VARCHAR(MAX)
AS
UPDATE Medicamento
SET
Nombre = @Nombre,
Precio=@Precio,
Imagen=@Imagen
WHERE IdMedicamento=@IdMedicamento
GO

CREATE PROCEDURE MedicamentoDelete
@IdMedicamento INT
AS
DELETE FROM Medicamento WHERE IdMedicamento=@IdMedicamento
GO

ALTER PROCEDURE MedicamentoGetAll
AS
SELECT IdMedicamento,Nombre,Precio,Imagen FROM Medicamento
GO

ALTER PROCEDURE MedicamentoGetById 
@IdMedicamento INT
AS
SELECT IdMedicamento,Nombre,Precio,Imagen FROM Medicamento
WHERE IdMedicamento=@IdMedicamento
GO

CREATE PROCEDURE UsuarioGetByUserName
@UserName VARCHAR(50)
AS
SELECT IdUsuario,UserName,Password FROM Usuario
WHERE UserName=@UserName
GO