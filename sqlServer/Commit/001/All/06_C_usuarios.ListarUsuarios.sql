-- 06_C_usuarios.ListarUsuarios.sql

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************
* Proposito: Lista todos los registros de la tabla Usuarios.
*
* Desarrollador: Johan Yepes
*
* Control de Cambios (Mantener siempre un único registro actualizado)
* =============================================================================
* Fecha       Autor   Version  Descripcion
* 2026-05-12  JY  1.0.0.0  Creación o última modificación del procedimiento.
******************************************************************************/
CREATE OR ALTER PROCEDURE [usuarios].[ListarUsuarios]
(
    @CodigoRespuesta INT OUT,
    @MensajeRespuesta VARCHAR(255) OUT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [IdUsuario], [Nombre], [FechaNacimiento], [Sexo]
    FROM [usuarios].[Usuarios]
    ORDER BY [IdUsuario] ASC;

    SET @CodigoRespuesta = 200; -- OK
    SET @MensajeRespuesta = 'Consulta exitosa.';
END
GO
