-- 01_C_usuarios.Usuarios.sql

/******************************************************************************
* Proposito: Crea el esquema usuarios (si no existe) y la tabla Usuarios con
*            IdUsuario, Nombre, FechaNacimiento y Sexo.
*
* Desarrollador: Johan Yepes
*
* Control de Cambios (Mantener siempre un único registro actualizado)
* =============================================================================
* Fecha       Autor   Version  Descripcion
* 2026-05-12  JY  1.0.0.0  Creación del script.
******************************************************************************/

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'usuarios')
    EXEC('CREATE SCHEMA [usuarios] AUTHORIZATION [dbo]');
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.tables AS t
    INNER JOIN sys.schemas AS s ON t.schema_id = s.schema_id
    WHERE t.name = N'Usuarios' AND s.name = N'usuarios'
)
BEGIN
    CREATE TABLE [usuarios].[Usuarios] (
        [IdUsuario]        INT           NOT NULL IDENTITY(1, 1),
        [Nombre]           VARCHAR(100)  NOT NULL,
        [FechaNacimiento]  DATE          NOT NULL,
        [Sexo]             VARCHAR(20)   NOT NULL,
        CONSTRAINT [PK_Usuarios_IdUsuario] PRIMARY KEY CLUSTERED ([IdUsuario] ASC)
    );
END
GO
