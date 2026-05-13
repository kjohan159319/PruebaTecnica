using AutoMapper;
using GestionUsuarios.Capas.Aplicacion.Dto;
using GestionUsuarios.Capas.Dominio.Entidades;

namespace GestionUsuarios.Capas.Aplicacion.Mapeadores
{
    public class UsuarioPerfil : Profile
    {
        public UsuarioPerfil()
        {
            CreateMap<Usuario, UsuarioDto>();
        }
    }
}
