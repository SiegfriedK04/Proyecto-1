using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace Pokemon.App_Start
{
    public class BD
    {
        private readonly string connectionString = @"Server=.\SQLEXPRESS;Database=Pokemon;Trusted_Connection=True;";

        public SqlConnection Conectar()
        {
            SqlConnection conexion = new SqlConnection(connectionString);
            conexion.Open();
            return conexion;
        }

        public void EjecutarComando(string query, SqlParameter[] parametros = null)
        {
            using (SqlConnection conexion = Conectar())
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    if (parametros != null)
                    {
                        comando.Parameters.AddRange(parametros);
                    }
                    comando.ExecuteNonQuery();
                }
            }
        }

        public DataTable EjecutarConsulta(string query, SqlParameter[] parametros = null)
        {
            DataTable tabla = new DataTable();

            using (SqlConnection conexion = Conectar())
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    if (parametros != null)
                    {
                        comando.Parameters.AddRange(parametros);
                    }

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                    {
                        adaptador.Fill(tabla);
                    }
                }
            }

            return tabla;
        }
    }
}