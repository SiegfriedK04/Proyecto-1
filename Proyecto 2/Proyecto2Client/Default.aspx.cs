using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Proyecto2Client
{
    public partial class Default : Page
    {
        private static readonly string apiUrl = "https://localhost:44386/api/Operaciones";

        [Serializable] 
        public class Operacion
        {
            public int Id { get; set; }
            public decimal Valor1 { get; set; }
            public string OperacionRealizada { get; set; }
            public decimal Valor2 { get; set; }
            public decimal Resultado { get; set; }
        }

        protected async void btnVerHistorial_Click(object sender, EventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetStringAsync(apiUrl);

                    var operaciones = JsonConvert.DeserializeObject<List<Operacion>>(response);

                    ViewState["Operaciones"] = operaciones;

                    string tablaHtml = "<table><tr><th>ID</th><th>Valor 1</th><th>Operación</th><th>Valor 2</th><th>Resultado</th></tr>";
                    foreach (var operacion in operaciones)
                    {
                        string operacionSimplificada = SimplificarOperacion(operacion.OperacionRealizada);

                        tablaHtml += $"<tr><td>{operacion.Id}</td><td>{operacion.Valor1}</td><td>{operacionSimplificada}</td><td>{operacion.Valor2}</td><td>{operacion.Resultado}</td></tr>";
                    }
                    tablaHtml += "</table>";

                    TablaPlaceholder.Text = tablaHtml;
                }
            }
            catch (Exception ex)
            {
                TablaPlaceholder.Text = $"<p style='color:red;'>Error al obtener el historial: {ex.Message}</p>";
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                var operaciones = ViewState["Operaciones"] as List<Operacion>;
                if (operaciones == null || operaciones.Count == 0)
                {
                    TablaPlaceholder.Text = "<p style='color:red;'>Por favor, carga el historial primero.</p>";
                    return;
                }

                string filtroSeleccionado = filtroOperaciones.SelectedValue;

                if (filtroSeleccionado == "seleccionar")
                {
                    TablaPlaceholder.Text = "<p style='color:red;'>Por favor, selecciona un filtro válido.</p>";
                    return;
                }

                var operacionesFiltradas = operaciones.Where(o =>
                    o.OperacionRealizada.Equals(filtroSeleccionado, StringComparison.OrdinalIgnoreCase) ||
                    (filtroSeleccionado == "suma" && o.OperacionRealizada.Contains("+")) ||
                    (filtroSeleccionado == "resta" && o.OperacionRealizada.Contains("-")) ||
                    (filtroSeleccionado == "multiplicacion" && o.OperacionRealizada.Contains("*")) ||
                    (filtroSeleccionado == "division" && o.OperacionRealizada.Contains("/")) ||
                    (filtroSeleccionado == "raiz cuadrada" && o.OperacionRealizada.Contains("√")) ||
                    (filtroSeleccionado == "potencia" && o.OperacionRealizada.Contains("x²")) ||
                    (filtroSeleccionado == "seno" && o.OperacionRealizada.Contains("sen")) ||
                    (filtroSeleccionado == "coseno" && o.OperacionRealizada.Contains("cos"))
                ).ToList();

                string tablaHtml = "<table><tr><th>ID</th><th>Valor 1</th><th>Operación</th><th>Valor 2</th><th>Resultado</th></tr>";
                foreach (var operacion in operacionesFiltradas)
                {
                    string operacionSimplificada = SimplificarOperacion(operacion.OperacionRealizada);

                    tablaHtml += $"<tr><td>{operacion.Id}</td><td>{operacion.Valor1}</td><td>{operacionSimplificada}</td><td>{operacion.Valor2}</td><td>{operacion.Resultado}</td></tr>";
                }
                tablaHtml += "</table>";

                TablaPlaceholder.Text = tablaHtml;
            }
            catch (Exception ex)
            {
                TablaPlaceholder.Text = $"<p style='color:red;'>Error al aplicar el filtro: {ex.Message}</p>";
            }
        }

        private string SimplificarOperacion(string descripcion)
        {
            string desc = descripcion.ToLower();
            if (desc.Contains("+") || desc.Contains("suma")) return "+";
            if (desc.Contains("-") || desc.Contains("resta")) return "-";
            if (desc.Contains("*") || desc.Contains("multiplicacion")) return "*";
            if (desc.Contains("/") || desc.Contains("division")) return "/";
            if (desc.Contains("raiz cuadrada") || desc.Contains("√")) return "√";
            if (desc.Contains("potencia") || desc.Contains("x²")) return "x²";
            if (desc.Contains("seno") || desc.Contains("sen")) return "Sen";
            if (desc.Contains("coseno") || desc.Contains("cos")) return "Cos";
            return descripcion;
        }

        protected async void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                string operacion = txtOperacion.Text.ToLower();
                decimal valor1 = 0;
                decimal valor2 = 0;

                if (!decimal.TryParse(txtValor1.Text, out valor1))
                {
                    TablaPlaceholder.Text = "<p style='color:red;'>Por favor, ingrese un valor válido para el Valor 1.</p>";
                    return;
                }

                if (!EsOperacionDeUnSoloValor(operacion) && !decimal.TryParse(txtValor2.Text, out valor2))
                {
                    TablaPlaceholder.Text = "<p style='color:red;'>Por favor, ingrese un valor válido para el Valor 2.</p>";
                    return;
                }

                using (HttpClient client = new HttpClient())
                {
                    var nuevaOperacion = new
                    {
                        OperacionRealizada = operacion,
                        Valor1 = valor1,
                        Valor2 = EsOperacionDeUnSoloValor(operacion) ? 0 : valor2 
                    };

                    var json = JsonConvert.SerializeObject(nuevaOperacion);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        TablaPlaceholder.Text = "<p style='color:green;'>Operación agregada con éxito.</p>";
                    }
                    else
                    {
                        TablaPlaceholder.Text = $"<p style='color:red;'>Error al agregar operación: {response.ReasonPhrase}</p>";
                    }
                }
            }
            catch (Exception ex)
            {
                TablaPlaceholder.Text = $"<p style='color:red;'>Error al agregar operación: {ex.Message}</p>";
            }
        }

        private bool EsOperacionDeUnSoloValor(string operacion)
        {
            string[] operacionesUnValor = { "raiz cuadrada", "√", "potencia", "x²", "seno", "sen", "coseno", "cos" };
            return operacionesUnValor.Contains(operacion);
        }

        protected async void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(txtEliminarId.Text) || !int.TryParse(txtEliminarId.Text, out int id))
                {
                    TablaPlaceholder.Text = "<p style='color:red;'>Por favor, ingrese un ID válido para eliminar.</p>";
                    return;
                }

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.DeleteAsync($"{apiUrl}/{id}");

                    response.EnsureSuccessStatusCode();
                    TablaPlaceholder.Text = "<p style='color:green;'>Operación eliminada correctamente.</p>";
                }
            }
            catch (Exception ex)
            {
                TablaPlaceholder.Text = $"<p style='color:red;'>Error al eliminar operación: {ex.Message}</p>";
            }
        }

        protected async void btnBorrarHistorial_Click(object sender, EventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.DeleteAsync($"{apiUrl}/borrar-historial");

                    if (response.IsSuccessStatusCode)
                    {
                        TablaPlaceholder.Text = $"<p style='color:green;'>Historial borrado exitosamente.</p>";
                    }
                    else
                    {
                        TablaPlaceholder.Text = $"<p style='color:red;'>Error al borrar el historial: {response.ReasonPhrase}</p>";
                    }
                }
            }
            catch (Exception ex)
            {
                TablaPlaceholder.Text = $"<p style='color:red;'>Error al borrar el historial: {ex.Message}</p>";
            }
        }
    }
}
