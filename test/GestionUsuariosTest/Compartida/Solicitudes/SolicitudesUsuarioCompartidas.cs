using System;
using GestionUsuarios.Capas.Aplicacion.Dto;

namespace GestionUsuariosTest.Compartida.Solicitudes
{
    public class SolicitudesUsuarioCompartidas
    {
        public UsuarioDto DtoValido { get; private set; }
        public UsuarioDto DtoConNombreVacio { get; private set; }
        public UsuarioDto DtoConNombreLargo { get; private set; }
        public UsuarioDto DtoConFechaFutura { get; private set; }
        public UsuarioDto DtoConSexoInvalido { get; private set; }

        public SolicitudesUsuarioCompartidas()
        {
            DtoValido = new UsuarioDto
            {
                IdUsuario = ConstantesCompartidas.IdUsuarioValido,
                Nombre = ConstantesCompartidas.NombreValido,
                FechaNacimiento = DateTime.Today.AddYears(-25),
                Sexo = ConstantesCompartidas.SexoValido
            };

            DtoConNombreVacio = new UsuarioDto
            {
                IdUsuario = ConstantesCompartidas.IdUsuarioValido,
                Nombre = string.Empty,
                FechaNacimiento = DateTime.Today.AddYears(-25),
                Sexo = ConstantesCompartidas.SexoValido
            };

            DtoConNombreLargo = new UsuarioDto
            {
                IdUsuario = ConstantesCompartidas.IdUsuarioValido,
                Nombre = new string('A', 31),
                FechaNacimiento = DateTime.Today.AddYears(-25),
                Sexo = ConstantesCompartidas.SexoValido
            };

            DtoConFechaFutura = new UsuarioDto
            {
                IdUsuario = ConstantesCompartidas.IdUsuarioValido,
                Nombre = ConstantesCompartidas.NombreValido,
                FechaNacimiento = DateTime.Today.AddDays(1),
                Sexo = ConstantesCompartidas.SexoValido
            };

            DtoConSexoInvalido = new UsuarioDto
            {
                IdUsuario = ConstantesCompartidas.IdUsuarioValido,
                Nombre = ConstantesCompartidas.NombreValido,
                FechaNacimiento = DateTime.Today.AddYears(-25),
                Sexo = "Invalido"
            };
        }
    }
}
