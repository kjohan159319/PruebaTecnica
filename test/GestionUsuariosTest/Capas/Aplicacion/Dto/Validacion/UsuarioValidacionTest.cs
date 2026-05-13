using System;
using System.Linq;
using FluentAssertions;
using GestionUsuarios.Capas.Aplicacion.Dto;
using GestionUsuarios.Capas.Aplicacion.Dto.Validaciones;
using GestionUsuariosTest.Compartida.Solicitudes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GestionUsuariosTest.Capas.Aplicacion.Dto.Validacion
{
    [TestClass]
    public class UsuarioValidacionTest
    {
        private UsuarioDtoValidacion _validator = null!;
        private SolicitudesUsuarioCompartidas _solicitudes = null!;

        [TestInitialize]
        public void Setup()
        {
            _validator = new UsuarioDtoValidacion();
            _solicitudes = new SolicitudesUsuarioCompartidas();
        }

        // ── Nombre ───────────────────────────────────────────────────────────

        [DataTestMethod]
        [DataRow("Juan Perez", true, null)]
        [DataRow("", false, "El nombre es obligatorio.")]
        [DataRow(null, false, "El nombre es obligatorio.")]
        [DataRow("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", false, "El nombre no puede superar 30 caracteres.")]
        public void ValidarNombre_DebeComportarseCorrectamente_SegunLosCasos(
            string? nombre, bool esValido, string? mensajeError)
        {
            var dto = new UsuarioDto
            {
                Nombre = nombre!,
                FechaNacimiento = DateTime.Today.AddYears(-25),
                Sexo = "Masculino"
            };

            var resultado = _validator.Validate(dto);

            if (esValido)
            {
                resultado.Errors.Should().NotContain(e => e.PropertyName == "Nombre");
            }
            else
            {
                resultado.Errors
                    .Where(e => e.PropertyName == "Nombre")
                    .Select(e => e.ErrorMessage)
                    .Should().Contain(mensajeError!);
            }
        }

        // ── FechaNacimiento ───────────────────────────────────────────────────

        [TestMethod]
        public void ValidarFechaNacimiento_CuandoEsPasada_DebeSerValida()
        {
            var dto = new UsuarioDto
            {
                Nombre = "Juan Perez",
                FechaNacimiento = DateTime.Today.AddYears(-25),
                Sexo = "Masculino"
            };

            var resultado = _validator.Validate(dto);

            resultado.Errors.Should().NotContain(e => e.PropertyName == "FechaNacimiento");
        }

        [TestMethod]
        public void ValidarFechaNacimiento_CuandoEsFutura_DebeSerInvalida()
        {
            var dto = new UsuarioDto
            {
                Nombre = "Juan Perez",
                FechaNacimiento = DateTime.Today.AddDays(1),
                Sexo = "Masculino"
            };

            var resultado = _validator.Validate(dto);

            resultado.Errors
                .Where(e => e.PropertyName == "FechaNacimiento")
                .Select(e => e.ErrorMessage)
                .Should().Contain("La fecha de nacimiento debe ser anterior a hoy.");
        }

        [TestMethod]
        public void ValidarFechaNacimiento_CuandoEsHoy_DebeSerInvalida()
        {
            var dto = new UsuarioDto
            {
                Nombre = "Juan Perez",
                FechaNacimiento = DateTime.Today,
                Sexo = "Masculino"
            };

            var resultado = _validator.Validate(dto);

            resultado.Errors
                .Where(e => e.PropertyName == "FechaNacimiento")
                .Select(e => e.ErrorMessage)
                .Should().Contain("La fecha de nacimiento debe ser anterior a hoy.");
        }

        [TestMethod]
        public void ValidarFechaNacimiento_CuandoEsDefault_DebeSerInvalida()
        {
            var dto = new UsuarioDto
            {
                Nombre = "Juan Perez",
                FechaNacimiento = default,
                Sexo = "Masculino"
            };

            var resultado = _validator.Validate(dto);

            resultado.Errors
                .Where(e => e.PropertyName == "FechaNacimiento")
                .Should().NotBeEmpty();
        }

        // ── Sexo ─────────────────────────────────────────────────────────────

        [DataTestMethod]
        [DataRow("Masculino", true, null)]
        [DataRow("Femenino", true, null)]
        [DataRow("Otro", true, null)]
        [DataRow("Invalido", false, "El sexo debe ser Masculino, Femenino u Otro.")]
        [DataRow("", false, "El sexo es obligatorio.")]
        [DataRow(null, false, "El sexo es obligatorio.")]
        public void ValidarSexo_DebeComportarseCorrectamente_SegunLosCasos(
            string? sexo, bool esValido, string? mensajeError)
        {
            var dto = new UsuarioDto
            {
                Nombre = "Juan Perez",
                FechaNacimiento = DateTime.Today.AddYears(-25),
                Sexo = sexo!
            };

            var resultado = _validator.Validate(dto);

            if (esValido)
            {
                resultado.Errors.Should().NotContain(e => e.PropertyName == "Sexo");
            }
            else
            {
                resultado.Errors
                    .Where(e => e.PropertyName == "Sexo")
                    .Select(e => e.ErrorMessage)
                    .Should().Contain(mensajeError!);
            }
        }

        // ── DTO completo válido ───────────────────────────────────────────────

        [TestMethod]
        public void ValidarDto_CuandoTodosLosCamposSonValidos_DebeSerValido()
        {
            var resultado = _validator.Validate(_solicitudes.DtoValido);

            resultado.IsValid.Should().BeTrue();
        }

        [TestMethod]
        public void ValidarDto_CuandoNombreEsVacio_DebeRetornar400ConMensaje()
        {
            var resultado = _validator.Validate(_solicitudes.DtoConNombreVacio);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Select(e => e.ErrorMessage)
                .Should().Contain("El nombre es obligatorio.");
        }

        [TestMethod]
        public void ValidarDto_CuandoSexoEsInvalido_DebeRetornarError()
        {
            var resultado = _validator.Validate(_solicitudes.DtoConSexoInvalido);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Select(e => e.ErrorMessage)
                .Should().Contain("El sexo debe ser Masculino, Femenino u Otro.");
        }
    }
}
