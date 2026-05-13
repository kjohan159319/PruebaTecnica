using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GestionUsuarios.Capas.Aplicacion.Dto;
using GestionUsuarios.Capas.Aplicacion.Interfaz;
using GestionUsuarios.Capas.Dominio.Entidades;
using GestionUsuarios.Capas.Dominio.Interfaz;
using GestionUsuarios.Capas.Transversal.Comun;

namespace GestionUsuarios.Capas.Aplicacion.Principal
{
    public class UsuarioAplicacion : IUsuarioAplicacion
    {
        private readonly IUsuarioDominio _usuarioDominio;
        private readonly IValidator<UsuarioDto> _validator;
        private readonly IMapper _mapper;

        public UsuarioAplicacion(IUsuarioDominio usuarioDominio, IValidator<UsuarioDto> validator, IMapper mapper)
        {
            _usuarioDominio = usuarioDominio;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<RespuestaBase> ListarUsuariosAsync()
        {
            RespuestaBase respuesta = await _usuarioDominio.ListarUsuariosAsync();

            if (respuesta.Codigo == 200 && respuesta.Datos is IEnumerable<Usuario> usuarios)
            {
                respuesta.Datos = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
            }

            return respuesta;
        }

        public async Task<RespuestaBase> ObtenerUsuarioPorIdAsync(int idUsuario)
        {
            RespuestaBase respuesta = await _usuarioDominio.ObtenerUsuarioPorIdAsync(idUsuario);

            if (respuesta.Codigo == 200 && respuesta.Datos is Usuario usuario)
            {
                respuesta.Datos = _mapper.Map<UsuarioDto>(usuario);
            }

            return respuesta;
        }

        public async Task<RespuestaBase> InsertarUsuarioAsync(UsuarioDto dto)
        {
            ValidationResult validacion = await _validator.ValidateAsync(dto);
            if (!validacion.IsValid)
            {
                return new RespuestaBase
                {
                    Codigo = 400,
                    Resultado = "Error",
                    Mensaje = string.Join(", ", validacion.Errors.Select(e => e.ErrorMessage))
                };
            }

            return await _usuarioDominio.InsertarUsuarioAsync(dto.Nombre, dto.FechaNacimiento, dto.Sexo);
        }

        public async Task<RespuestaBase> ActualizarUsuarioAsync(UsuarioDto dto)
        {
            ValidationResult validacion = await _validator.ValidateAsync(dto);
            if (!validacion.IsValid)
            {
                return new RespuestaBase
                {
                    Codigo = 400,
                    Resultado = "Error",
                    Mensaje = string.Join(", ", validacion.Errors.Select(e => e.ErrorMessage))
                };
            }

            return await _usuarioDominio.ActualizarUsuarioAsync(dto.IdUsuario, dto.Nombre, dto.FechaNacimiento, dto.Sexo);
        }

        public async Task<RespuestaBase> EliminarUsuarioAsync(int idUsuario)
        {
            return await _usuarioDominio.EliminarUsuarioAsync(idUsuario);
        }
    }
}
