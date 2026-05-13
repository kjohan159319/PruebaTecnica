using GestionUsuarios.Capas.Transversal.Comun;

namespace GestionUsuariosTest.Compartida.Respuestas
{
    public class RespuestasUsuarioCompartidas
    {
        public RespuestaBase Exitosa200 { get; private set; }
        public RespuestaBase Exitosa201 { get; private set; }
        public RespuestaBase Error400 { get; private set; }
        public RespuestaBase Error500 { get; private set; }

        public RespuestasUsuarioCompartidas()
        {
            Exitosa200 = new RespuestaBase
            {
                Codigo = ConstantesCompartidas.Codigo200,
                Resultado = ConstantesCompartidas.ResultadoOK,
                Mensaje = "Operación exitosa"
            };

            Exitosa201 = new RespuestaBase
            {
                Codigo = ConstantesCompartidas.Codigo201,
                Resultado = ConstantesCompartidas.ResultadoOK,
                Mensaje = "Usuario creado exitosamente"
            };

            Error400 = new RespuestaBase
            {
                Codigo = ConstantesCompartidas.Codigo400,
                Resultado = ConstantesCompartidas.ResultadoError,
                Mensaje = "Error de validación"
            };

            Error500 = new RespuestaBase
            {
                Codigo = ConstantesCompartidas.Codigo500,
                Resultado = ConstantesCompartidas.ResultadoError,
                Mensaje = "Error interno del servidor"
            };
        }
    }
}
