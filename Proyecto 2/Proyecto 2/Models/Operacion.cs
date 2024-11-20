using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_2_API.Models
{
    public class Operacion
    {
        public int Id { get; set; } 
        public string OperacionRealizada { get; set; } 
        public decimal Valor1 { get; set; } 
        public decimal Valor2 { get; set; } 
        public decimal Resultado { get; set; } 
    }
}