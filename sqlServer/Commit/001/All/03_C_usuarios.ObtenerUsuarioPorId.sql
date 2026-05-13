-- 03_C_usuarios.ObtenerUsuarioPorId.sql

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************
* Proposito: Obtiene un registro de Usuarios por su IdUsuario.
*
* Desarrollador: Johan Yepes
*
* Control de Cambios (Mantener siempre un único registro actualizado)
* =============================================================================
* Fecha       Autor   Version  Descripcion
* 2026-05-12  JY  1.0.0.0  Creación o última modificación del procedimiento.
******************************************************************************/
CREATE OR ALTER PROCEDURE [usuarios].[ObtenerUsuarioPorId]
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
        SET @MensajeRespuesta = 'El Usuario especificado no existe.';
        SELECT [IdUsuario], [Nombre], [FechaNacimiento], [Sexo]
        FROM [usuarios].[Usuarios]
        WHERE 1 = 0; -- Retorna estructura vacía
        RETURN;
    END

    SELECT [IdUsuario], [Nombre], [FechaNacimiento], [Sexo]
    FROM [usuarios].[Usuarios]
    WHERE [IdUsuario] = @IdUsuario;

    SET @CodigoRespuesta = 200; -- OK
    SET @MensajeRespuesta = 'Consulta exitosa.';
END
GO
