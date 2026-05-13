using System;
using System.Threading.Tasks;
using FluentAssertions;
using GestionUsuarios.Capas.Dominio.Core;
using GestionUsuarios.Capas.Infraestructura.Interfaz;
using GestionUsuarios.Capas.Transversal.Comun;
using GestionUsuariosTest.Compartida;
using GestionUsuariosTest.Compartida.Respuestas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GestionUsuariosTest.Capas.Dominio.Core
{
    [TestClass]
    public class UsuarioDominioTest
    {
        private Mock<IUsuarioRepositorio> _mockRepositorio = null!;
        private UsuarioDominio _dominio = null!;
        private RespuestasUsuarioCompartidas _respuestas = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockRepositorio = new Mock<IUsuarioRepositorio>();
            _respuestas = new RespuestasUsuarioCompartidas();
            _dominio = new UsuarioDominio(_mockRepositorio.Object);
        }

        // ── ListarUsuariosAsync ───────────────────────────────────────────────

        [TestMethod]
        public async Task ListarUsuariosAsync_DebeRetornarRespuestaDelRepositorio()
        {
            _mockRepositorio
                .Setup(r => r.ListarUsuariosAsync())
                .ReturnsAsync(_respuestas.Exitosa200);

            var resultado = await _dominio.ListarUsuariosAsync();

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo200);
            resultado.Resultado.Should().Be(ConstantesCompartidas.ResultadoOK);
            _mockRepositorio.Verify(r => r.ListarUsuariosAsync(), Times.Once);
        }

        [TestMethod]
        public async Task ListarUsuariosAsync_CuandoRepositorioRetornaError_DebePropagar()
        {
            _mockRepositorio
                .Setup(r => r.ListarUsuariosAsync())
                .ReturnsAsync(_respuestas.Error500);

            var resultado = await _dominio.ListarUsuariosAsync();

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo500);
            resultado.Resultado.Should().Be(ConstantesCompartidas.ResultadoError);
        }

        // ── ObtenerUsuarioPorIdAsync ──────────────────────────────────────────

        [TestMethod]
        public async Task ObtenerUsuarioPorIdAsync_DebeRetornarRespuestaDelRepositorio()
        {
            _mockRepositorio
                .Setup(r => r.ObtenerUsuarioPorIdAsync(ConstantesCompartidas.IdUsuarioValido))
                .ReturnsAsync(_respuestas.Exitosa200);

            var resultado = await _dominio.ObtenerUsuarioPorIdAsync(ConstantesCompartidas.IdUsuarioValido);

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo200);
            resultado.Resultado.Should().Be(ConstantesCompartidas.ResultadoOK);
            _mockRepositorio.Verify(r => r.ObtenerUsuarioPorIdAsync(ConstantesCompartidas.IdUsuarioValido), Times.Once);
        }

        [TestMethod]
        public async Task ObtenerUsuarioPorIdAsync_CuandoRepositorioRetornaError_DebePropagar()
        {
            _mockRepositorio
                .Setup(r => r.ObtenerUsuarioPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_respuestas.Error500);

            var resultado = await _dominio.ObtenerUsuarioPorIdAsync(999);

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo500);
        }

        // ── InsertarUsuarioAsync ──────────────────────────────────────────────

        [TestMethod]
        public async Task InsertarUsuarioAsync_DebeRetornarRespuestaDelRepositorioYVerificarParametros()
        {
            var fecha = DateTime.Today.AddYears(-25);

            _mockRepositorio
                .Setup(r => r.InsertarUsuarioAsync(
                    ConstantesCompartidas.NombreValido,
                    fecha,
                    ConstantesCompartidas.SexoValido))
                .ReturnsAsync(_respuestas.Exitosa201);

            var resultado = await _dominio.InsertarUsuarioAsync(
                ConstantesCompartidas.NombreValido, fecha, ConstantesCompartidas.SexoValido);

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo201);
            resultado.Resultado.Should().Be(ConstantesCompartidas.ResultadoOK);
            _mockRepositorio.Verify(r => r.InsertarUsuarioAsync(
                ConstantesCompartidas.NombreValido,
                fecha,
                ConstantesCompartidas.SexoValido), Times.Once);
        }

        [TestMethod]
        public async Task InsertarUsuarioAsync_CuandoRepositorioRetornaError_DebePropagar()
        {
            _mockRepositorio
                .Setup(r => r.InsertarUsuarioAsync(
                    It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .ReturnsAsync(_respuestas.Error500);

            var resultado = await _dominio.InsertarUsuarioAsync(
                ConstantesCompartidas.NombreValido,
                DateTime.Today.AddYears(-25),
                ConstantesCompartidas.SexoValido);

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo500);
        }

        // ── ActualizarUsuarioAsync ────────────────────────────────────────────

        [TestMethod]
        public async Task ActualizarUsuarioAsync_DebeRetornarRespuestaDelRepositorioYVerificarParametros()
        {
            var fecha = DateTime.Today.AddYears(-25);

            _mockRepositorio
                .Setup(r => r.ActualizarUsuarioAsync(
                    ConstantesCompartidas.IdUsuarioValido,
                    ConstantesCompartidas.NombreValido,
                    fecha,
                    ConstantesCompartidas.SexoValido))
                .ReturnsAsync(_respuestas.Exitosa200);

            var resultado = await _dominio.ActualizarUsuarioAsync(
                ConstantesCompartidas.IdUsuarioValido,
                ConstantesCompartidas.NombreValido,
                fecha,
                ConstantesCompartidas.SexoValido);

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo200);
            resultado.Resultado.Should().Be(ConstantesCompartidas.ResultadoOK);
            _mockRepositorio.Verify(r => r.ActualizarUsuarioAsync(
                ConstantesCompartidas.IdUsuarioValido,
                ConstantesCompartidas.NombreValido,
                fecha,
                ConstantesCompartidas.SexoValido), Times.Once);
        }

        [TestMethod]
        public async Task ActualizarUsuarioAsync_CuandoRepositorioRetornaError_DebePropagar()
        {
            _mockRepositorio
                .Setup(r => r.ActualizarUsuarioAsync(
                    It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .ReturnsAsync(_respuestas.Error500);

            var resultado = await _dominio.ActualizarUsuarioAsync(
                999, ConstantesCompartidas.NombreValido, DateTime.Today.AddYears(-25), ConstantesCompartidas.SexoValido);

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo500);
        }

        // ── EliminarUsuarioAsync ──────────────────────────────────────────────

        [TestMethod]
        public async Task EliminarUsuarioAsync_DebeRetornarRespuestaDelRepositorioYVerificarId()
        {
            _mockRepositorio
                .Setup(r => r.EliminarUsuarioAsync(ConstantesCompartidas.IdUsuarioValido))
                .ReturnsAsync(_respuestas.Exitosa200);

            var resultado = await _dominio.EliminarUsuarioAsync(ConstantesCompartidas.IdUsuarioValido);

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo200);
            resultado.Resultado.Should().Be(ConstantesCompartidas.ResultadoOK);
            _mockRepositorio.Verify(r => r.EliminarUsuarioAsync(ConstantesCompartidas.IdUsuarioValido), Times.Once);
        }

        [TestMethod]
        public async Task EliminarUsuarioAsync_CuandoRepositorioRetornaError_DebePropagar()
        {
            _mockRepositorio
                .Setup(r => r.EliminarUsuarioAsync(It.IsAny<int>()))
                .ReturnsAsync(_respuestas.Error500);

            var resultado = await _dominio.EliminarUsuarioAsync(999);

            resultado.Codigo.Should().Be(ConstantesCompartidas.Codigo500);
        }
    }
}
