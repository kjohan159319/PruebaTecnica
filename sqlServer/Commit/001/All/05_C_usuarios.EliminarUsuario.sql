-- 05_C_usuarios.EliminarUsuario.sql

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************
* Proposito: Elimina un registro de la tabla Usuarios.
*
* Desarrollador: Johan Yepes
*
* Control de Cambios (Mantener siempre un único registro actualizado)
* =============================================================================
* Fecha       Autor   Version  Descripcion
* 2026-05-12  JY  1.0.0.0  Creación o última modificación del procedimiento.
******************************************************************************/
CREATE OR ALTER PROCEDURE [usuarios].[EliminarUsuario]
(
    @IdUsuario INT,
    @CodigoRespuesta INT OUT,
    @MensajeRespuesta VARCHAR(255) OUT
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM [usuarios].[Usuarios] WHERE [IdUsuario] = @IdUsuario)
    BEGIN
        SET @CodigoRespuesta = 404; -- Not Found
        SET @MensajeRespuesta = 'El Usuario a eliminar no existe.';
        RETURN;
    END

    BEGIN TRY
        DELETE FROM [usuarios].[Usuarios]
        WHERE [IdUsuario] = @IdUsuario; -- eliminacion logica

        SET @CodigoRespuesta = 200; -- OK
        SET @MensajeRespuesta = 'Usuario eliminado con éxito.';
    END TRY
    BEGIN CATCH
        SET @CodigoRespuesta = 500; -- Internal Server Error
        SET @MensajeRespuesta = ERROR_MESSAGE();
    END CATCH
END
GO
