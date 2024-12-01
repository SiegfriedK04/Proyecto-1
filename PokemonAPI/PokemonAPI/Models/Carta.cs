using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokemonAPI.Models
{
    public class Carta
    {
        public int ID { get; set; }
        public int? Pokedex { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Region { get; set; }
        public string Rareza { get; set; }
        public int? HP { get; set; }
        public string Debilidad { get; set; }
        public string ImagenURL { get; set; }
        public string Habilidad { get; set; }
        public string Categoria { get; set; }
    }
}