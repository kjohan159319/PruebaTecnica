using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using GestionUsuarios.Capas.Aplicacion.Dto;
using GestionUsuarios.Capas.Aplicacion.Principal;
using GestionUsuarios.Capas.Dominio.Entidades;
using GestionUsuarios.Capas.Dominio.Interfaz;
using GestionUsuarios.Capas.Transversal.Comun;
using GestionUsuariosTest.Compartida;
using GestionUsuariosTest.Compartida.Respuestas;
using GestionUsuariosTest.Compartida.Solicitudes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GestionUsuariosTest.Capas.Aplicacion.Principal
{
    [TestClass]
    public class UsuarioAplicacionTest
    {
        private Mock<IUsuarioDominio> _mockDominio = null!;
        private Mock<IValidator<UsuarioDto>> _mockValidator = null!;
        private Mock<IMapper> _mockMapper = null!;
        private UsuarioAplicacion _aplicacion = null!;
        private SolicitudesUsuarioCompartidas _solicitudes = null!;
        private RespuestasUsuarioCompartidas _respuestas = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockDominio = new Mock<IUsuarioDominio>();
            _mockValidator = new Mock<IValidator<UsuarioDto>>();
            _mockMapper = new Mock<IMapper>();
            _solicitudes = new SolicitudesUsuarioCompartidas();
            _respuestas = new RespuestasUsuarioCompartidas();

            _aplicacion = new UsuarioAplicacion(
                _mockDominio.Object,
                _mockValidator.Object,
                _mockMapper.Object);
        }

        // ── ListarUsuariosAsync ───────────────────────────────────────────────

        [TestMethod]
        public async Task ListarUsuariosAsync_CuandoDominioRetorna200_DebeMapearAListaDto()
        {
            var listaUsuarios = new List<Usuario>
            {
                new() { IdUsuario = 1, Nombre = ConstantesCompartidas.NombreValido, FechaNacimiento = DateTime.Today.AddYears(-25), Sexo = ConstantesCompartidas.SexoValido }
            };
            var listaDto = new List<UsuarioDto>
            {
                new() { IdUsuario = 1, Nombre = ConstantesCompartidas.NombreValido, FechaNacimiento = DateTime.Today.AddYears(-25), Sexo = ConstantesCompartidas.SexoValido }
            };
            var respuestaDominio = new RespuestaBase
            {
                Codigo = ConstantesCompartidas.Codigo200,
                Resultado = ConstantesCompartidas.ResultadoOK,
                Datos = listaUsuarios
            };

            _mockDominio.Setup(d => d.ListarUsuariosAsync()).ReturnsAsync(respuestaDominio);
            _mockMapper.Setup(m => m.Map<IEnumerable<UsuarioDto>>(It.IsAny<object>())).Returns(listaDto);

            var resultado = await _aplicacion.ListarUsuariosAsync();

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo200);
            ((object)resultado.Datos!).Should().NotBeNull();
            _mockMapper.Verify(m => m.Map<IEnumerable<UsuarioDto>>(It.IsAny<object>()), Times.Once);
        }

        [TestMethod]
        public async Task ListarUsuariosAsync_CuandoDominioRetornaError_DebePropagar()
        {
            _mockDominio.Setup(d => d.ListarUsuariosAsync()).ReturnsAsync(_respuestas.Error500);

            var resultado = await _aplicacion.ListarUsuariosAsync();

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo500);
            resultado.Resultado.Should().Be(ConstantesCompartidas.ResultadoError);
            _mockMapper.Verify(m => m.Map<IEnumerable<UsuarioDto>>(It.IsAny<object>()), Times.Never);
        }

        // ── ObtenerUsuarioPorIdAsync ──────────────────────────────────────────

        [TestMethod]
        public async Task ObtenerUsuarioPorIdAsync_CuandoDominioRetorna200_DebeMapearADto()
        {
            var usuario = new Usuario
            {
                IdUsuario = ConstantesCompartidas.IdUsuarioValido,
                Nombre = ConstantesCompartidas.NombreValido,
                FechaNacimiento = DateTime.Today.AddYears(-25),
                Sexo = ConstantesCompartidas.SexoValido
            };
            var usuarioDto = new UsuarioDto
            {
                IdUsuario = ConstantesCompartidas.IdUsuarioValido,
                Nombre = ConstantesCompartidas.NombreValido,
                FechaNacimiento = DateTime.Today.AddYears(-25),
                Sexo = ConstantesCompartidas.SexoValido
            };
            var respuestaDominio = new RespuestaBase
            {
                Codigo = ConstantesCompartidas.Codigo200,
                Resultado = ConstantesCompartidas.ResultadoOK,
                Datos = usuario
            };

            _mockDominio.Setup(d => d.ObtenerUsuarioPorIdAsync(It.IsAny<int>())).ReturnsAsync(respuestaDominio);
            _mockMapper.Setup(m => m.Map<UsuarioDto>(It.IsAny<object>())).Returns(usuarioDto);

            var resultado = await _aplicacion.ObtenerUsuarioPorIdAsync(ConstantesCompartidas.IdUsuarioValido);

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo200);
            ((object)resultado.Datos!).Should().NotBeNull();
            _mockMapper.Verify(m => m.Map<UsuarioDto>(It.IsAny<object>()), Times.Once);
        }

        [TestMethod]
        public async Task ObtenerUsuarioPorIdAsync_CuandoDominioRetornaError_DebePropagar()
        {
            _mockDominio.Setup(d => d.ObtenerUsuarioPorIdAsync(It.IsAny<int>())).ReturnsAsync(_respuestas.Error500);

            var resultado = await _aplicacion.ObtenerUsuarioPorIdAsync(999);

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo500);
            resultado.Resultado.Should().Be(ConstantesCompartidas.ResultadoError);
            _mockMapper.Verify(m => m.Map<UsuarioDto>(It.IsAny<object>()), Times.Never);
        }

        // ── InsertarUsuarioAsync ──────────────────────────────────────────────

        [TestMethod]
        public async Task InsertarUsuarioAsync_CuandoDtoEsInvalido_DebeRetornar400()
        {
            var errores = new List<ValidationFailure>
            {
                new("Nombre", "El nombre es obligatorio.")
            };
            _mockValidator
                .Setup(v => v.ValidateAsync(It.IsAny<UsuarioDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(errores));

            var resultado = await _aplicacion.InsertarUsuarioAsync(_solicitudes.DtoConNombreVacio);

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo400);
            resultado.Resultado.Should().Be(ConstantesCompartidas.ResultadoError);
            resultado.Mensaje.Should().Contain("El nombre es obligatorio.");
            _mockDominio.Verify(d => d.InsertarUsuarioAsync(
                It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task InsertarUsuarioAsync_CuandoDtoEsValido_DebeDelegarAlDominioYRetornar201()
        {
            _mockValidator
                .Setup(v => v.ValidateAsync(It.IsAny<UsuarioDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mockDominio
                .Setup(d => d.InsertarUsuarioAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .ReturnsAsync(_respuestas.Exitosa201);

            var resultado = await _aplicacion.InsertarUsuarioAsync(_solicitudes.DtoValido);

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo201);
            resultado.Resultado.Should().Be(ConstantesCompartidas.ResultadoOK);
            _mockDominio.Verify(d => d.InsertarUsuarioAsync(
                _solicitudes.DtoValido.Nombre,
                _solicitudes.DtoValido.FechaNacimiento,
                _solicitudes.DtoValido.Sexo), Times.Once);
        }

        // ── ActualizarUsuarioAsync ────────────────────────────────────────────

        [TestMethod]
        public async Task ActualizarUsuarioAsync_CuandoDtoEsInvalido_DebeRetornar400()
        {
            var errores = new List<ValidationFailure>
            {
                new("Sexo", "El sexo debe ser Masculino, Femenino u Otro.")
            };
            _mockValidator
                .Setup(v => v.ValidateAsync(It.IsAny<UsuarioDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(errores));

            var resultado = await _aplicacion.ActualizarUsuarioAsync(_solicitudes.DtoConSexoInvalido);

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo400);
            resultado.Resultado.Should().Be(ConstantesCompartidas.ResultadoError);
            resultado.Mensaje.Should().Contain("El sexo debe ser Masculino, Femenino u Otro.");
            _mockDominio.Verify(d => d.ActualizarUsuarioAsync(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task ActualizarUsuarioAsync_CuandoDtoEsValido_DebeDelegarAlDominioYRetornar200()
        {
            _mockValidator
                .Setup(v => v.ValidateAsync(It.IsAny<UsuarioDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mockDominio
                .Setup(d => d.ActualizarUsuarioAsync(
                    It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .ReturnsAsync(_respuestas.Exitosa200);

            var resultado = await _aplicacion.ActualizarUsuarioAsync(_solicitudes.DtoValido);

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo200);
            resultado.Resultado.Should().Be(ConstantesCompartidas.ResultadoOK);
            _mockDominio.Verify(d => d.ActualizarUsuarioAsync(
                _solicitudes.DtoValido.IdUsuario,
                _solicitudes.DtoValido.Nombre,
                _solicitudes.DtoValido.FechaNacimiento,
                _solicitudes.DtoValido.Sexo), Times.Once);
        }

        // ── EliminarUsuarioAsync ──────────────────────────────────────────────

        [TestMethod]
        public async Task EliminarUsuarioAsync_CuandoIdEsValido_DebeRetornarRespuestaDominio()
        {
            _mockDominio
                .Setup(d => d.EliminarUsuarioAsync(ConstantesCompartidas.IdUsuarioValido))
                .ReturnsAsync(_respuestas.Exitosa200);

            var resultado = await _aplicacion.EliminarUsuarioAsync(ConstantesCompartidas.IdUsuarioValido);

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo200);
            resultado.Resultado.Should().Be(ConstantesCompartidas.ResultadoOK);
            _mockDominio.Verify(d => d.EliminarUsuarioAsync(ConstantesCompartidas.IdUsuarioValido), Times.Once);
        }

        [TestMethod]
        public async Task EliminarUsuarioAsync_CuandoDominioRetornaError_DebePropagar()
        {
            _mockDominio
                .Setup(d => d.EliminarUsuarioAsync(It.IsAny<int>()))
                .ReturnsAsync(_respuestas.Error500);

            var resultado = await _aplicacion.EliminarUsuarioAsync(999);

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo500);
            resultado.Resultado.Should().Be(ConstantesCompartidas.ResultadoError);
        }
    }
}
