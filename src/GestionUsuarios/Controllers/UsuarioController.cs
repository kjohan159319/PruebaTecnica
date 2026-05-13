using GestionUsuarios.Capas.Aplicacion.Dto;
using GestionUsuarios.Capas.Aplicacion.Interfaz;
using GestionUsuarios.Capas.Transversal.Comun;
using Microsoft.AspNetCore.Mvc;

namespace GestionUsuarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioAplicacion _usuarioAplicacion;

        public UsuarioController(IUsuarioAplicacion usuarioAplicacion)
        {
            _usuarioAplicacion = usuarioAplicacion;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            RespuestaBase respuesta = await _usuarioAplicacion.ListarUsuariosAsync();
            return StatusCode(respuesta.Codigo, respuesta);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            RespuestaBase respuesta = await _usuarioAplicacion.ObtenerUsuarioPorIdAsync(id);
            return StatusCode(respuesta.Codigo, respuesta);
        }

        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] UsuarioDto dto)
        {
            RespuestaBase respuesta = await _usuarioAplicacion.InsertarUsuarioAsync(dto);
            return StatusCode(respuesta.Codigo, respuesta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] UsuarioDto dto)
        {
            dto.IdUsuario = id;
            RespuestaBase respuesta = await _usuarioAplicacion.ActualizarUsuarioAsync(dto);
            return StatusCode(respuesta.Codigo, respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            RespuestaBase respuesta = await _usuarioAplicacion.EliminarUsuarioAsync(id);
            return StatusCode(respuesta.Codigo, respuesta);
        }
    }
}
