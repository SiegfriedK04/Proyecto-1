using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokemonAPI.Models
{
    public class Coleccion
    {
        public int ColeccionID { get; set; }
        public int UsuarioID { get; set; }
        public int CartaID { get; set; }
        public int Cantidad { get; set; }
    }
}