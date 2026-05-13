using GestionUsuarios.Capas.Aplicacion.Dto;
using GestionUsuarios.Capas.Aplicacion.Interfaz;
using GestionUsuarios.Capas.Transversal.Comun;

namespace GestionUsuarios.WCF.Servicios
{
    public class UsuarioServicioWCF : IUsuarioServicioWCF
    {
        private readonly IUsuarioAplicacion _usuarioAplicacion;

        public UsuarioServicioWCF(IUsuarioAplicacion usuarioAplicacion)
        {
            _usuarioAplicacion = usuarioAplicacion;
        }

        public async Task<RespuestaBase> ListarUsuariosAsync()
        {
            return await _usuarioAplicacion.ListarUsuariosAsync();
        }

        public async Task<RespuestaBase> ObtenerUsuarioPorIdAsync(int idUsuario)
        {
            return await _usuarioAplicacion.ObtenerUsuarioPorIdAsync(idUsuario);
        }

        public async Task<RespuestaBase> InsertarUsuarioAsync(string nombre, DateTime fechaNacimiento, string sexo)
        {
            UsuarioDto dto = new UsuarioDto
            {
                Nombre = nombre,
                FechaNacimiento = fechaNacimiento,
                Sexo = sexo
            };

            return await _usuarioAplicacion.InsertarUsuarioAsync(dto);
        }

        public async Task<RespuestaBase> ActualizarUsuarioAsync(int idUsuario, string nombre, DateTime fechaNacimiento, string sexo)
        {
            UsuarioDto dto = new UsuarioDto
            {
                IdUsuario = idUsuario,
                Nombre = nombre,
                FechaNacimiento = fechaNacimiento,
                Sexo = sexo
            };

            return await _usuarioAplicacion.ActualizarUsuarioAsync(dto);
        }

        public async Task<RespuestaBase> EliminarUsuarioAsync(int idUsuario)
        {
            return await _usuarioAplicacion.EliminarUsuarioAsync(idUsuario);
        }
    }
}
