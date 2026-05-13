using GestionUsuarios.Capas.Aplicacion.Dto;
using GestionUsuarios.Capas.Aplicacion.Interfaz;
using GestionUsuarios.Capas.Transversal.Comun;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionUsuarios.WCF.Pages
{
    public class UsuarioConsultaModel : PageModel
    {
        private readonly IUsuarioAplicacion _usuarioAplicacion;

        public UsuarioConsultaModel(IUsuarioAplicacion usuarioAplicacion)
        {
            _usuarioAplicacion = usuarioAplicacion;
        }

        public List<UsuarioDto> Usuarios { get; private set; } = new List<UsuarioDto>();
        public string Mensaje { get; private set; } = string.Empty;
        public bool Exito { get; private set; }

        public async Task OnGetAsync(string? mensaje, bool? exito)
        {
            await CargarUsuariosAsync();

            // Mostrar mensaje de éxito si viene del redirect después de editar
            if (!string.IsNullOrEmpty(mensaje))
            {
                Mensaje = mensaje;
                Exito = exito ?? false;
            }
        }

        public async Task<IActionResult> OnPostEliminarAsync(int idUsuario)
        {
            RespuestaBase respuesta = await _usuarioAplicacion.EliminarUsuarioAsync(idUsuario);

            Mensaje = respuesta.Mensaje;
            Exito = respuesta.Codigo == 200;

            await CargarUsuariosAsync();
            return Page();
        }

        private async Task CargarUsuariosAsync()
        {
            RespuestaBase respuesta = await _usuarioAplicacion.ListarUsuariosAsync();

            if (respuesta.Codigo == 200 && respuesta.Datos is IEnumerable<UsuarioDto> lista)
            {
                Usuarios = lista.ToList();
            }
        }
    }
}
