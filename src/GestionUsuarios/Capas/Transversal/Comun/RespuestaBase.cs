namespace GestionUsuarios.Capas.Transversal.Comun
{
    public class RespuestaBase
    {
        public int Codigo { get; set; }
        public string Resultado { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public object? Datos { get; set; }
    }
}
