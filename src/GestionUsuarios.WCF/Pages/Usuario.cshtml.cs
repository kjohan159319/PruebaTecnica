using GestionUsuarios.Capas.Aplicacion.Dto;
using GestionUsuarios.Capas.Aplicacion.Interfaz;
using GestionUsuarios.Capas.Transversal.Comun;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionUsuarios.WCF.Pages
{
    public class UsuarioModel : PageModel
    {
        private readonly IUsuarioAplicacion _usuarioAplicacion;

        public UsuarioModel(IUsuarioAplicacion usuarioAplicacion)
        {
            _usuarioAplicacion = usuarioAplicacion;
        }

        [BindProperty]
        public int IdUsuario { get; set; }

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string FechaNacimiento { get; set; } = string.Empty;

        [BindProperty]
        public string Sexo { get; set; } = string.Empty;

        public string Mensaje { get; private set; } = string.Empty;
        public bool Exito { get; private set; }
        public bool ModoEdicion { get; private set; }

        public async Task OnGetAsync(int? id)
        {
            if (id.HasValue && id.Value > 0)
            {
                RespuestaBase respuesta = await _usuarioAplicacion.ObtenerUsuarioPorIdAsync(id.Value);

                if (respuesta.Codigo == 200 && respuesta.Datos is UsuarioDto dto)
                {
                    IdUsuario = dto.IdUsuario;
                    Nombre = dto.Nombre;
                    FechaNacimiento = dto.FechaNacimiento.ToString("yyyy-MM-dd");
                    Sexo = dto.Sexo;
                    ModoEdicion = true;
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!DateTime.TryParse(FechaNacimiento, out DateTime fecha))
            {
                Mensaje = "La fecha de nacimiento no es válida.";
                return Page();
            }

            UsuarioDto dto = new UsuarioDto
            {
                IdUsuario = IdUsuario,
                Nombre = Nombre,
                FechaNacimiento = fecha,
                Sexo = Sexo
            };

            RespuestaBase respuesta;

            if (IdUsuario > 0)
            {
                respuesta = await _usuarioAplicacion.ActualizarUsuarioAsync(dto);
                Mensaje = respuesta.Mensaje;
                Exito = respuesta.Codigo == 200;
                ModoEdicion = true;

                if (Exito)
                {
                    // Redirigir al dashboard después de actualizar exitosamente
                    return RedirectToPage("/UsuarioConsulta", new { mensaje = Mensaje, exito = true });
                }
            }
            else
            {
                respuesta = await _usuarioAplicacion.InsertarUsuarioAsync(dto);
                Mensaje = respuesta.Mensaje;
                Exito = respuesta.Codigo == 201;

                if (Exito)
                {
                    // Limpiar formulario y mostrar vista colapsada (formulario vacío listo para nuevo registro)
                    Nombre = string.Empty;
                    FechaNacimiento = string.Empty;
                    Sexo = string.Empty;
                    IdUsuario = 0;
                    ModoEdicion = false;
                    // Continuar para mostrar mensaje de éxito y formulario colapsado
                }
            }

            return Page();
        }
    }
}
