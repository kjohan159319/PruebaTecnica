using GestionUsuarios.Capas.Aplicacion.Dto;
using GestionUsuarios.Capas.Transversal.Comun;

namespace GestionUsuarios.Capas.Aplicacion.Interfaz
{
    public interface IUsuarioAplicacion
    {
        Task<RespuestaBase> ListarUsuariosAsync();
        Task<RespuestaBase> ObtenerUsuarioPorIdAsync(int idUsuario);
        Task<RespuestaBase> InsertarUsuarioAsync(UsuarioDto dto);
        Task<RespuestaBase> ActualizarUsuarioAsync(UsuarioDto dto);
        Task<RespuestaBase> EliminarUsuarioAsync(int idUsuario);
    }
}
