using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using PokemonAPI.Models;

namespace PokemonAPI.Controllers
{
    [RoutePrefix("api/usuarios")]
    public class UsuariosController : ApiController
    {
        private readonly string connectionString = @"Server=.\SQLEXPRESS;Database=Pokemon;Trusted_Connection=True;";

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] Usuario.LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.NombreUsuario) || string.IsNullOrEmpty(request.Contrasena))
                return BadRequest("Por favor, ingresa usuario y contraseña.");

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                string query = "SELECT UsuarioID, NombreUsuario FROM Usuarios WHERE NombreUsuario = @NombreUsuario AND Contraseña = @Contrasena";
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@NombreUsuario", request.NombreUsuario);
                    comando.Parameters.AddWithValue("@Contrasena", request.Contrasena);
                    conexion.Open();

                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        return Ok(new
                        {
                            UsuarioID = reader["UsuarioID"],
                            NombreUsuario = reader["NombreUsuario"]
                        });
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
            }
        }

        [HttpPost]
        [Route("registro")]
        public IHttpActionResult Registro([FromBody] Usuario.RegistroRequest request)
        {
            if (string.IsNullOrEmpty(request.NombreUsuario) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Contrasena))
                return BadRequest("Por favor, completa todos los campos.");

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                string queryVerificar = "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @NombreUsuario";
                using (SqlCommand comando = new SqlCommand(queryVerificar, conexion))
                {
                    comando.Parameters.AddWithValue("@NombreUsuario", request.NombreUsuario);
                    conexion.Open();
                    int count = (int)comando.ExecuteScalar();

                    if (count > 0)
                        return Conflict(); 
                }

                string queryInsertar = "INSERT INTO Usuarios (NombreUsuario, Email, Contraseña) VALUES (@NombreUsuario, @Email, @Contrasena)";
                using (SqlCommand comando = new SqlCommand(queryInsertar, conexion))
                {
                    comando.Parameters.AddWithValue("@NombreUsuario", request.NombreUsuario);
                    comando.Parameters.AddWithValue("@Email", request.Email);
                    comando.Parameters.AddWithValue("@Contrasena", request.Contrasena);
                    comando.ExecuteNonQuery();
                }

                return Ok("Usuario registrado con éxito.");
            }
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                string query = "SELECT UsuarioID, NombreUsuario, Email FROM Usuarios";
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    conexion.Open();
                    SqlDataReader reader = comando.ExecuteReader();

                    while (reader.Read())
                    {
                        usuarios.Add(new Usuario
                        {
                            UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                            NombreUsuario = reader["NombreUsuario"].ToString(),
                            Email = reader["Email"].ToString()
                        });
                    }
                }
            }

            return Ok(usuarios);
        }

        [HttpGet]
        [Route("buscar/{nombreUsuario}")]
        public IHttpActionResult GetUsuarioPorNombre(string nombreUsuario)
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                string query = "SELECT UsuarioID, NombreUsuario, Contraseña FROM Usuarios WHERE NombreUsuario = @NombreUsuario";
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                    conexion.Open();

                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        return Ok(new
                        {
                            UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                            NombreUsuario = reader["NombreUsuario"].ToString(),
                            Contrasena = reader["Contraseña"].ToString() 
                        });
                    }
                    else
                    {
                        return NotFound(); 
                    }
                }
            }
        }
    }
}