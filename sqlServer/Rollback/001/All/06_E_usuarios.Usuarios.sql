-- 06_E_usuarios.Usuarios.sql

/******************************************************************************
* Proposito: Elimina la tabla usuarios.Usuarios y el esquema usuarios si queda
*            sin objetos dependientes.
*
* Desarrollador: Johan Yepes
*
* Control de Cambios (Mantener siempre un único registro actualizado)
* =============================================================================
* Fecha       Autor   Version  Descripcion
* 2026-05-12  JY  1.0.0.0  Creación del script de rollback.
******************************************************************************/

IF EXISTS (
    SELECT 1
    FROM sys.tables AS t
    INNER JOIN sys.schemas AS s ON t.schema_id = s.schema_id
    WHERE t.name = N'Usuarios' AND s.name = N'usuarios'
)
    DROP TABLE [usuarios].[Usuarios];
GO

IF SCHEMA_ID(N'usuarios') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.objects WHERE schema_id = SCHEMA_ID(N'usuarios'))
BEGIN
    DROP SCHEMA [usuarios];
END
GO
