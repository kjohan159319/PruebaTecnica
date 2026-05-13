using Dapper;
using GestionUsuarios.Capas.Aplicacion.Dto;
using GestionUsuarios.Capas.Dominio.Entidades;
using GestionUsuarios.Capas.Infraestructura.Interfaz;
using GestionUsuarios.Capas.Transversal.Comun;
using GestionUsuarios.Capas.Transversal.Fabricas;
using System.Data;

namespace GestionUsuarios.Capas.Infraestructura.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly IFabricaConexionSql _fabricaConexion;

        public UsuarioRepositorio(IFabricaConexionSql fabricaConexion)
        {
            _fabricaConexion = fabricaConexion;
        }

        public async Task<RespuestaBase> ListarUsuariosAsync()
        {
            try
            {
                using IDbConnection conexion = _fabricaConexion.ConexionLecturaEscritura;

                var parametros = new DynamicParameters();
                parametros.Add("@CodigoRespuesta", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parametros.Add("@MensajeRespuesta", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                IEnumerable<Usuario> datos = await conexion.QueryAsync<Usuario>(
                    "usuarios.ListarUsuarios",
                    parametros,
                    commandType: CommandType.StoredProcedure);

                int codigo = parametros.Get<int>("@CodigoRespuesta");
                string mensaje = parametros.Get<string>("@MensajeRespuesta");

                return new RespuestaBase
                {
                    Codigo = codigo,
                    Resultado = codigo == 200 ? "OK" : "Error",
                    Mensaje = mensaje,
                    Datos = datos
                };
            }
            catch (Exception ex)
            {
                return new RespuestaBase { Codigo = 500, Resultado = "Error", Mensaje = ex.Message };
            }
        }

        public async Task<RespuestaBase> ObtenerUsuarioPorIdAsync(int idUsuario)
        {
            try
            {
                using IDbConnection conexion = _fabricaConexion.ConexionLecturaEscritura;

                var parametros = new DynamicParameters();
                parametros.Add("@IdUsuario", idUsuario);
                parametros.Add("@CodigoRespuesta", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parametros.Add("@MensajeRespuesta", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                Usuario? dato = await conexion.QueryFirstOrDefaultAsync<Usuario>(
                    "usuarios.ObtenerUsuarioPorId",
                    parametros,
                    commandType: CommandType.StoredProcedure);

                int codigo = parametros.Get<int>("@CodigoRespuesta");
                string mensaje = parametros.Get<string>("@MensajeRespuesta");

                return new RespuestaBase
                {
                    Codigo = codigo,
                    Resultado = codigo == 200 ? "OK" : "Error",
                    Mensaje = mensaje,
                    Datos = dato
                };
            }
            catch (Exception ex)
            {
                return new RespuestaBase { Codigo = 500, Resultado = "Error", Mensaje = ex.Message };
            }
        }

        public async Task<RespuestaBase> InsertarUsuarioAsync(string nombre, DateTime fechaNacimiento, string sexo)
        {
            try
            {
                using IDbConnection conexion = _fabricaConexion.ConexionLecturaEscritura;

                var parametros = new DynamicParameters();
                parametros.Add("@Nombre", nombre);
                parametros.Add("@FechaNacimiento", fechaNacimiento.Date);
                parametros.Add("@Sexo", sexo);
                parametros.Add("@IdGenerado", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parametros.Add("@CodigoRespuesta", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parametros.Add("@MensajeRespuesta", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                await conexion.ExecuteAsync(
                    "usuarios.InsertarUsuario",
                    parametros,
                    commandType: CommandType.StoredProcedure);

                int codigo = parametros.Get<int>("@CodigoRespuesta");
                string mensaje = parametros.Get<string>("@MensajeRespuesta");
                int idGenerado = parametros.Get<int>("@IdGenerado");

                return new RespuestaBase
                {
                    Codigo = codigo,
                    Resultado = codigo == 201 ? "OK" : "Error",
                    Mensaje = mensaje,
                    Datos = new UsuarioInsertadoDto { IdGenerado = idGenerado }
                };
            }
            catch (Exception ex)
            {
                return new RespuestaBase { Codigo = 500, Resultado = "Error", Mensaje = ex.Message };
            }
        }

        public async Task<RespuestaBase> ActualizarUsuarioAsync(int idUsuario, string nombre, DateTime fechaNacimiento, string sexo)
        {
            try
            {
                using IDbConnection conexion = _fabricaConexion.ConexionLecturaEscritura;

                var parametros = new DynamicParameters();
                parametros.Add("@IdUsuario", idUsuario);
                parametros.Add("@Nombre", nombre);
                parametros.Add("@FechaNacimiento", fechaNacimiento.Date);
                parametros.Add("@Sexo", sexo);
                parametros.Add("@CodigoRespuesta", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parametros.Add("@MensajeRespuesta", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                await conexion.ExecuteAsync(
                    "usuarios.ActualizarUsuario",
                    parametros,
                    commandType: CommandType.StoredProcedure);

                int codigo = parametros.Get<int>("@CodigoRespuesta");
                string mensaje = parametros.Get<string>("@MensajeRespuesta");

                return new RespuestaBase
                {
                    Codigo = codigo,
                    Resultado = codigo == 200 ? "OK" : "Error",
                    Mensaje = mensaje
                };
            }
            catch (Exception ex)
            {
                return new RespuestaBase { Codigo = 500, Resultado = "Error", Mensaje = ex.Message };
            }
        }

        public async Task<RespuestaBase> EliminarUsuarioAsync(int idUsuario)
        {
            try
            {
                using IDbConnection conexion = _fabricaConexion.ConexionLecturaEscritura;

                var parametros = new DynamicParameters();
                parametros.Add("@IdUsuario", idUsuario);
                parametros.Add("@CodigoRespuesta", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parametros.Add("@MensajeRespuesta", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                await conexion.ExecuteAsync(
                    "usuarios.EliminarUsuario",
                    parametros,
                    commandType: CommandType.StoredProcedure);

                int codigo = parametros.Get<int>("@CodigoRespuesta");
                string mensaje = parametros.Get<string>("@MensajeRespuesta");

                return new RespuestaBase
                {
                    Codigo = codigo,
                    Resultado = codigo == 200 ? "OK" : "Error",
                    Mensaje = mensaje
                };
            }
            catch (Exception ex)
            {
                return new RespuestaBase { Codigo = 500, Resultado = "Error", Mensaje = ex.Message };
            }
        }
    }
}
