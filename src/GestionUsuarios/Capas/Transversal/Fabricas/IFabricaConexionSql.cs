using System.Data;

namespace GestionUsuarios.Capas.Transversal.Fabricas
{
    public interface IFabricaConexionSql
    {
        IDbConnection ConexionLecturaEscritura { get; }
    }
}
