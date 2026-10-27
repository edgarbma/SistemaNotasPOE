using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using SistemaNotasPOE.Clases;
using RepoDb;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace SistemaNotasPOE.Repositorios
{
    public class EstudianteRepo
    {
        private readonly string _cadenaConexion;

        public EstudianteRepo()
        {
            GlobalConfiguration.Setup().UseSqlServer();
            _cadenaConexion = ConfigurationManager.ConnectionStrings["conexionBD"].ConnectionString;
        }

        private IDbConnection CrearConexion()
        {
            return new SqlConnection(_cadenaConexion);
        }

        public List<Estudiante> ObtenerTodos()
        {
            using (var db = CrearConexion())
            {
                return db.QueryAll<Estudiante>().ToList();
            }
        }

        public void Insertar(Estudiante estudiante)
        {
            using (var db = CrearConexion())
            {
                db.Insert(estudiante);
            }
        }

        public void Actualizar(Estudiante estudiante)
        {
            using (var db = CrearConexion())
            {
                db.Update(estudiante);
            }
        }
    }
}