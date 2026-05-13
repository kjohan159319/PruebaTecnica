using CoreWCF;
using GestionUsuarios.Capas.Transversal.Comun;

namespace GestionUsuarios.WCF.Servicios
{
    [ServiceContract]
    public interface IUsuarioServicioWCF
    {
        [OperationContract]
        Task<RespuestaBase> ListarUsuariosAsync();

        [OperationContract]
        Task<RespuestaBase> ObtenerUsuarioPorIdAsync(int idUsuario);

        [OperationContract]
        Task<RespuestaBase> InsertarUsuarioAsync(string nombre, DateTime fechaNacimiento, string sexo);

        [OperationContract]
        Task<RespuestaBase> ActualizarUsuarioAsync(int idUsuario, string nombre, DateTime fechaNacimiento, string sexo);

        [OperationContract]
        Task<RespuestaBase> EliminarUsuarioAsync(int idUsuario);
    }
}
