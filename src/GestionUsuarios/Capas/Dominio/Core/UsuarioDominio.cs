using GestionUsuarios.Capas.Dominio.Interfaz;
using GestionUsuarios.Capas.Infraestructura.Interfaz;
using GestionUsuarios.Capas.Transversal.Comun;

namespace GestionUsuarios.Capas.Dominio.Core
{
    public class UsuarioDominio : IUsuarioDominio
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioDominio(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<RespuestaBase> ListarUsuariosAsync()
        {
            return await _usuarioRepositorio.ListarUsuariosAsync();
        }

        public async Task<RespuestaBase> ObtenerUsuarioPorIdAsync(int idUsuario)
        {
            return await _usuarioRepositorio.ObtenerUsuarioPorIdAsync(idUsuario);
        }

        public async Task<RespuestaBase> InsertarUsuarioAsync(string nombre, DateTime fechaNacimiento, string sexo)
        {
            return await _usuarioRepositorio.InsertarUsuarioAsync(nombre, fechaNacimiento, sexo);
        }

        public async Task<RespuestaBase> ActualizarUsuarioAsync(int idUsuario, string nombre, DateTime fechaNacimiento, string sexo)
        {
            return await _usuarioRepositorio.ActualizarUsuarioAsync(idUsuario, nombre, fechaNacimiento, sexo);
        }

        public async Task<RespuestaBase> EliminarUsuarioAsync(int idUsuario)
        {
            return await _usuarioRepositorio.EliminarUsuarioAsync(idUsuario);
        }
    }
}
