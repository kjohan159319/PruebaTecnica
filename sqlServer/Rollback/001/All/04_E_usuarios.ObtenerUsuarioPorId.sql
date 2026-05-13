-- 04_E_usuarios.ObtenerUsuarioPorId.sql

/******************************************************************************
* Proposito: Revierte la creación del procedimiento usuarios.ObtenerUsuarioPorId.
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
    FROM sys.objects AS o
    INNER JOIN sys.schemas AS s ON o.schema_id = s.schema_id
    WHERE o.type = N'P' AND o.name = N'ObtenerUsuarioPorId' AND s.name = N'usuarios'
)
    DROP PROCEDURE [usuarios].[ObtenerUsuarioPorId];
GO
