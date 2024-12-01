using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokemonAPI.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public string Email { get; set; }

        public class LoginRequest
        {
            public string NombreUsuario { get; set; }
            public string Contrasena { get; set; }
        }

        public class RegistroRequest
        {
            public string NombreUsuario { get; set; }
            public string Email { get; set; }
            public string Contrasena { get; set; }
        }
    }
}
