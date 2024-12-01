using System.Data.SqlClient;
using System.Web.Http;
using System;
using System.Net;
using System.Collections.Generic;
using System.Data;

namespace PokemonAPI.Controllers
{

    [RoutePrefix("api/cartas")]
    public class CartasController : ApiController
    {

        [HttpGet]
        [Route("todas")]
        public IHttpActionResult ObtenerTodasLasCartas()
        {
            string query = @"
                SELECT ID, Nombre, Tipo, Region, Rareza, ImagenURL 
                FROM Cartas";

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        conexion.Open();
                        using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                        {
                            adaptador.Fill(dt);
                        }
                    }
                }

                var cartas = new List<dynamic>();

                foreach (DataRow row in dt.Rows)
                {
                    cartas.Add(new
                    {
                        ID = row["ID"],
                        Nombre = row["Nombre"],
                        Tipo = row["Tipo"],
                        Region = row["Region"],
                        Rareza = row["Rareza"],
                        ImagenURL = row["ImagenURL"]
                    });
                }

                return Ok(cartas);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private readonly string connectionString = @"Server=.\SQLEXPRESS;Database=Pokemon;Trusted_Connection=True;";

        [HttpGet]
        [Route("detalles/{cartaID}")]
        public IHttpActionResult GetDetalleCarta(int cartaID, [FromUri] int usuarioID = 0)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT C.Pokedex, C.Nombre, C.Tipo, C.Region, C.Rareza, C.HP, C.Debilidad, 
                               ISNULL(C.Habilidad, 'N') AS Habilidad, C.ImagenURL,
                               ISNULL(Co.Cantidad, 0) AS Cantidad
                        FROM Cartas C
                        LEFT JOIN Colecciones Co ON C.ID = Co.CartaID AND Co.UsuarioID = @UsuarioID
                        WHERE C.ID = @ID";

                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@ID", cartaID);
                        comando.Parameters.AddWithValue("@UsuarioID", usuarioID);

                        conexion.Open();
                        SqlDataReader reader = comando.ExecuteReader();

                        if (reader.Read())
                        {
                            var detallesCarta = new
                            {
                                Pokedex = reader["Pokedex"],
                                Nombre = reader["Nombre"],
                                Tipo = reader["Tipo"],
                                Region = reader["Region"],
                                Rareza = reader["Rareza"],
                                HP = reader["HP"],
                                Debilidad = reader["Debilidad"],
                                Habilidad = reader["Habilidad"],
                                ImagenURL = reader["ImagenURL"],
                                Cantidad = reader["Cantidad"]
                            };
                            return Ok(detallesCarta);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}