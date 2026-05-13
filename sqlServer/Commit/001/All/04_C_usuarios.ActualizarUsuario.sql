-- 04_C_usuarios.ActualizarUsuario.sql

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************
* Proposito: Actualiza un registro existente en la tabla Usuarios.
*
* Desarrollador: Johan Yepes
*
* Control de Cambios (Mantener siempre un único registro actualizado)
* =============================================================================
* Fecha       Autor   Version  Descripcion
* 2026-05-12  JY  1.0.0.0  Creación o última modificación del procedimiento.
******************************************************************************/
CREATE OR ALTER PROCEDURE [usuarios].[ActualizarUsuario]
(
    @IdUsuario INT,
    @Nombre VARCHAR(100),
    @FechaNacimiento DATE,
    @Sexo VARCHAR(20),
    @CodigoRespuesta INT OUT,
    @MensajeRespuesta VARCHAR(255) OUT
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM [usuarios].[Usuarios] WHERE [IdUsuario] = @IdUsuario)
    BEGIN
        SET @CodigoRespuesta = 404; -- Not Found
        SET @MensajeRespuesta = 'El Usuario a actualizar no existe.';
        RETURN;
    END

    BEGIN TRY
        UPDATE [usuarios].[Usuarios]
        SET [Nombre] = @Nombre,
            [FechaNacimiento] = @FechaNacimiento,
            [Sexo] = @Sexo
        WHERE [IdUsuario] = @IdUsuario;

        SET @CodigoRespuesta = 200; -- OK
        SET @MensajeRespuesta = 'Usuario actualizado con éxito.';
    END TRY
    BEGIN CATCH
        SET @CodigoRespuesta = 500; -- Internal Server Error
        SET @MensajeRespuesta = ERROR_MESSAGE();
    END CATCH
END
GO
