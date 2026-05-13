using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionUsuarios.Capas.Transversal.Fabricas
{
    public class FabricaConexionSqlServer : IFabricaConexionSql
    {
        private readonly IConfiguration _configuracion;

        public FabricaConexionSqlServer(IConfiguration configuracion)
        {
            _configuracion = configuracion;
        }

        public IDbConnection ConexionLecturaEscritura =>
            new SqlConnection(_configuracion["BaseDeDatos:CadenaConexion"]);
    }
}
