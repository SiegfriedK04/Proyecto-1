using Proyecto_2_API.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Proyecto_2_API.Controllers
{
    public class OperacionesController : ApiController
    {
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=Calculadora;Trusted_Connection=True;";

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Operaciones")]
        public IEnumerable<Operacion> Get()
        {
            List<Operacion> operaciones = new List<Operacion>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Operaciones";
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    operaciones.Add(new Operacion
                    {
                        Id = (int)reader["Id"],
                        OperacionRealizada = reader["Operacion"].ToString(),
                        Valor1 = (decimal)reader["Valor1"],
                        Valor2 = reader["Valor2"] == DBNull.Value ? 0 : (decimal)reader["Valor2"],
                        Resultado = (decimal)reader["Resultado"]
                    });
                }
            }

            return operaciones;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Operaciones")]
        public IHttpActionResult Post([FromBody] Operacion operacion)
        {
            try
            {
                decimal resultado = CalcularResultado(operacion.OperacionRealizada, operacion.Valor1, operacion.Valor2);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Operaciones (Operacion, Valor1, Valor2, Resultado) VALUES (@Operacion, @Valor1, @Valor2, @Resultado)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Operacion", operacion.OperacionRealizada);
                    cmd.Parameters.AddWithValue("@Valor1", operacion.Valor1);
                    cmd.Parameters.AddWithValue("@Valor2", operacion.Valor2);
                    cmd.Parameters.AddWithValue("@Resultado", resultado);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return Ok("Operación agregada con éxito.");
            }
            catch (DivideByZeroException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private decimal CalcularResultado(string operacion, decimal valor1, decimal valor2)
        {
            string operacionNormalizada = operacion.ToLower();

            switch (operacionNormalizada)
            {
                case "suma":
                case "+":
                    return valor1 + valor2;

                case "resta":
                case "-":
                    return valor1 - valor2;

                case "multiplicacion":
                case "*":
                    return valor1 * valor2;

                case "division":
                case "/":
                    if (valor2 == 0) throw new DivideByZeroException("No se puede dividir entre cero.");
                    return valor1 / valor2;

                case "raiz cuadrada":
                case "√":
                    if (valor1 < 0) throw new InvalidOperationException("No se puede calcular la raíz cuadrada de un número negativo.");
                    return (decimal)Math.Sqrt((double)valor1);

                case "potencia":
                case "x²":
                    return (decimal)Math.Pow((double)valor1, 2);

                case "seno":
                case "sen":
                    return (decimal)Math.Sin((double)valor1); 

                case "coseno":
                case "cos":
                    return (decimal)Math.Cos((double)valor1); 

                default:
                    throw new InvalidOperationException("Operación no soportada.");
            }
        }

        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("api/Operaciones/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Operaciones WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        return NotFound();
                }

                return Ok($"Operación con ID {id} eliminada con éxito.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("api/Operaciones/borrar-historial")]
        public IHttpActionResult BorrarHistorial()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Operaciones; DBCC CHECKIDENT ('Operaciones', RESEED, 0);";
                    SqlCommand cmd = new SqlCommand(query, con);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return Ok("Historial borrado exitosamente.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}