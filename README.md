# GestionUsuarios

Solución **.NET 9** para la gestión de usuarios que expone dos proyectos complementarios: una **Web API REST** y un **servicio SOAP (CoreWCF)** con interfaz web Razor Pages. Ambos proyectos comparten la misma lógica de negocio, acceso a datos e infraestructura mediante referencias de proyecto.

## Características

- CRUD completo de usuarios vía **REST** y **SOAP**
- Arquitectura en capas con separación clara de responsabilidades
- Validación de entrada con **FluentValidation**
- Mapeo de objetos con **AutoMapper**
- Acceso a datos con **Dapper** y stored procedures
- Documentación de API con **Swagger UI** en la raíz
- Interfaz web integrada en el host WCF (**Razor Pages**)
- Pruebas unitarias con **MSTest**, **Moq** y **FluentAssertions**
- Entorno completamente contenedorizado con **Docker Compose**

## Arquitectura

La solución sigue una **arquitectura en capas** dentro de cada proyecto:

```
Presentación  (UsuarioController / Razor Pages / UsuarioServicioWCF)
      ↓
Aplicación    (UsuarioAplicacion — orquestación, AutoMapper, FluentValidation)
      ↓
Dominio       (UsuarioDominio — reglas de negocio)
      ↓
Infraestructura (UsuarioRepositorio — Dapper + Stored Procedures)
      ↓
Transversal   (RespuestaBase, FabricaConexionSqlServer)
```

## Proyectos

| Proyecto | Tipo | Puerto local | Puerto Docker |
|---|---|---|---|
| `GestionUsuarios` | Web API REST | `5000` | `8080` |
| `GestionUsuarios.WCF` | CoreWCF + Razor Pages | `5002` | `8081` |
| `GestionUsuariosTest` | MSTest | — | — |

### GestionUsuarios — Web API REST

API REST con Swagger habilitado en la raíz (`/`).

| Método | Ruta | Descripción |
|--------|------|-------------|
| `GET` | `/api/usuario` | Listar todos los usuarios |
| `GET` | `/api/usuario/{id}` | Obtener usuario por ID |
| `POST` | `/api/usuario` | Insertar nuevo usuario |
| `PUT` | `/api/usuario/{id}` | Actualizar usuario existente |
| `DELETE` | `/api/usuario/{id}` | Eliminar usuario |

Todas las respuestas usan el envoltorio `RespuestaBase`:

```json
{
  "codigo": 200,
  "resultado": "OK",
  "mensaje": "...",
  "datos": { }
}
```

### GestionUsuarios.WCF — Servicio SOAP + Web

- **Endpoint SOAP**: `http://localhost:5002/UsuarioServicio` (BasicHttpBinding)
- **WSDL**: `http://localhost:5002/UsuarioServicio?wsdl`
- **Página `/Usuario`**: formulario de registro (Nombre, Fecha de nacimiento, Sexo)
- **Página `/UsuarioConsulta`**: grilla con opciones de modificar y eliminar

Operaciones disponibles: `ListarUsuariosAsync`, `ObtenerUsuarioPorIdAsync`, `InsertarUsuarioAsync`, `ActualizarUsuarioAsync`, `EliminarUsuarioAsync`.

## Base de datos

- **Motor**: SQL Server 2022
- **Base de datos**: `PruebaTecnica`
- **Esquema**: `usuarios`
- **Tabla**: `Usuarios` (`IdUsuario`, `Nombre`, `FechaNacimiento`, `Sexo`)

Los scripts DDL y SPs se encuentran en `sqlServer/Commit/001/All/` y los rollbacks en `sqlServer/Rollback/001/All/`.

**Stored Procedures:**

| Stored Procedure | Descripción |
|---|---|
| `usuarios.ListarUsuarios` | Devuelve todos los usuarios |
| `usuarios.ObtenerUsuarioPorId` | Devuelve un usuario por ID |
| `usuarios.InsertarUsuario` | Inserta un nuevo usuario |
| `usuarios.ActualizarUsuario` | Actualiza un usuario existente |
| `usuarios.EliminarUsuario` | Elimina un usuario por ID |

## Tecnologías

| Paquete | Versión | Propósito |
|---|---|---|
| [.NET 9](https://dotnet.microsoft.com/) | 9.0 | Runtime y SDK |
| [Dapper](https://github.com/DapperLib/Dapper) | 2.1.35 | Micro-ORM |
| [AutoMapper](https://automapper.org/) | 14.0.0 | Mapeo de objetos |
| [FluentValidation](https://docs.fluentvalidation.net/) | 11.11.0 | Validaciones |
| [CoreWCF](https://github.com/CoreWCF/CoreWCF) | 1.5.2 | Servicio SOAP sobre .NET moderno |
| [Swashbuckle / Swagger](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) | 6.9.0 | Documentación de API |
| [Moq](https://github.com/devlooped/moq) | 4.20.72 | Mocking en pruebas |
| [FluentAssertions](https://fluentassertions.com/) | 6.12.0 | Aserciones expresivas |

## Configuración

Cada proyecto tiene dos archivos de configuración:

| Archivo | Entorno |
|---|---|
| `appsettings.json` | Docker (host `sqlserver`) |
| `appsettings.Local.json` | Desarrollo local (host `localhost`) |

Ajusta la cadena de conexión según tu entorno en la clave `BaseDeDatos:CadenaConexion`:

```json
"BaseDeDatos": {
  "CadenaConexion": "Server=<host>;Database=PruebaTecnica;User Id=sa;Password=<password>;TrustServerCertificate=True;"
}
```

> [!WARNING]
> Las credenciales en `appsettings.json` y `docker-compose.yml` son exclusivamente para desarrollo local. No las uses en producción.

## Ejecución local

**Prerrequisitos:** [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0), SQL Server 2022 (o Docker).

```bash
# Aplicar scripts de base de datos en SQL Server local
# (ejecutar manualmente los scripts en sqlServer/Commit/001/All/)

# API REST — Swagger en http://localhost:5000
cd src/GestionUsuarios
dotnet run --launch-profile Local

# WCF + Web — http://localhost:5002
cd src/GestionUsuarios.WCF
dotnet run --launch-profile Local

# Pruebas unitarias
cd test/GestionUsuariosTest
dotnet test
```

## Ejecución con Docker

Levanta todos los servicios (SQL Server, tests, API y WCF) con un solo comando:

```bash
docker-compose up -d
```

| Servicio | URL |
|---|---|
| API REST + Swagger | http://localhost:8080 |
| WCF + Web | http://localhost:8081 |
| SQL Server | `localhost:1433` |

> [!NOTE]
> Los servicios `api` y `wcf` dependen de `sqlserver` y `tests`. Docker Compose espera a que los tests pasen antes de arrancar los servicios de aplicación.

Para detener y limpiar volúmenes:

```bash
docker-compose down -v
```

## Pruebas unitarias

El proyecto `GestionUsuariosTest` cubre las tres capas principales con **27 pruebas** en total:

| Archivo | Capa | Pruebas |
|---|---|---|
| `UsuarioDominioTest.cs` | Dominio | 10 |
| `UsuarioAplicacionTest.cs` | Aplicación | 10 |
| `UsuarioValidacionTest.cs` | Validación DTO | 7 |

## Estructura del repositorio

```
GestionUsuarios/
├── docker-compose.yml
├── GestionUsuarios.slnx
├── README.md
├── sqlServer/                        # Imagen Docker SQL Server + scripts
│   ├── Dockerfile
│   ├── init.sh
│   ├── Commit/001/All/               # Scripts de creación (DDL + SPs)
│   └── Rollback/001/All/             # Scripts de rollback
├── src/
│   ├── Dockerfile                    # Imagen API REST
│   ├── Dockerfile.wcf                # Imagen WCF + Razor
│   ├── GestionUsuarios/              # Web API REST
│   │   ├── Controllers/
│   │   └── Capas/                    # Aplicacion, Dominio, Infraestructura, Transversal
│   └── GestionUsuarios.WCF/          # CoreWCF + Razor Pages
│       ├── Servicios/
│       └── Pages/
└── test/
    └── GestionUsuariosTest/          # Pruebas unitarias (MSTest + Moq)
        └── Capas/                    # Tests por capa (Dominio, Aplicacion)
```
