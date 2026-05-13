-- 02_C_usuarios.InsertarUsuario.sql

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************
* Proposito: Inserta un nuevo registro en la tabla Usuarios.
*
* Desarrollador: Johan Yepes
*
* Control de Cambios (Mantener siempre un único registro actualizado)
* =============================================================================
* Fecha       Autor   Version  Descripcion
* 2026-05-12  JY  1.0.0.0  Creación o última modificación del procedimiento.
******************************************************************************/
CREATE OR ALTER PROCEDURE [usuarios].[InsertarUsuario]
(
    @Nombre VARCHAR(100),
    @FechaNacimiento DATE,
    @Sexo VARCHAR(20),
    @IdGenerado INT OUT,
    @CodigoRespuesta INT OUT,
    @MensajeRespuesta VARCHAR(255) OUT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO [usuarios].[Usuarios] ([Nombre], [FechaNacimiento], [Sexo])
        VALUES (@Nombre, @FechaNacimiento, @Sexo);

        SET @IdGenerado = CAST(SCOPE_IDENTITY() AS INT);
        SET @CodigoRespuesta = 201; -- Created
        SET @MensajeRespuesta = 'Usuario creado con éxito.';
    END TRY
    BEGIN CATCH
        SET @CodigoRespuesta = 500; -- Internal Server Error
        SET @MensajeRespuesta = ERROR_MESSAGE();
    END CATCH
END
GO
