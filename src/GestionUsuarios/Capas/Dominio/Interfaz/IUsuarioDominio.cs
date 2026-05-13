using GestionUsuarios.Capas.Transversal.Comun;

namespace GestionUsuarios.Capas.Dominio.Interfaz
{
    public interface IUsuarioDominio
    {
        Task<RespuestaBase> ListarUsuariosAsync();
        Task<RespuestaBase> ObtenerUsuarioPorIdAsync(int idUsuario);
        Task<RespuestaBase> InsertarUsuarioAsync(string nombre, DateTime fechaNacimiento, string sexo);
        Task<RespuestaBase> ActualizarUsuarioAsync(int idUsuario, string nombre, DateTime fechaNacimiento, string sexo);
        Task<RespuestaBase> EliminarUsuarioAsync(int idUsuario);
    }
}
