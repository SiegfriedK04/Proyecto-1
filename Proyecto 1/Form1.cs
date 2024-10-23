using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_1
{
    public partial class Form1 : Form
    {
        string connectionString = "Server=localhost\\SQLEXPRESS;Database=Calculadora;Trusted_Connection=True;";
        double valor1 = 0, valor2 = 0;
        string operacion = "";


        public Form1()
        {
            InitializeComponent();
        }

        private void BtnNumero_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                if (btn.Text == "-")
                {
                    if (Pantalla.Text.StartsWith("-"))
                        Pantalla.Text = Pantalla.Text.Substring(1);
                    else
                        Pantalla.Text = "-" + Pantalla.Text;
                }
                else
                {
                    if (Pantalla.Text == "0" || Pantalla.Text == "-0")
                        Pantalla.Text = btn.Text;
                    else
                        Pantalla.Text += btn.Text;
                }
            }
        }

        private void Operaciones_Basicas(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                if (Pantalla.Text == "" && btn.Text == "-")
                {
                    Pantalla.Text = "-";
                    return;
                }

                if (Pantalla.Text.EndsWith(" ") && btn.Text == "-")
                {
                    Pantalla.Text += "-";
                    return;
                }

                if (!double.TryParse(Pantalla.Text, out valor1))
                {
                    MessageBox.Show("Por favor ingrese un número válido.");
                    return;
                }

                operacion = btn.Text.Trim();
                if (operacion == "X")
                    operacion = "*";

                Pantalla.Text += " " + operacion + " ";
            }
        }

        private void BtnIgual_Click(object sender, EventArgs e)
        {
            try
            {
                string[] partes = Pantalla.Text.Split(' ');

                if (partes.Length < 3)
                {
                    MessageBox.Show("Operación incompleta. Ingrese dos números y una operación.");
                    return;
                }

                valor1 = Convert.ToDouble(partes[0]);
                operacion = partes[1];
                valor2 = Convert.ToDouble(partes[2]);
                double resultado = 0;

                switch (operacion)
                {
                    case "+":
                        resultado = valor1 + valor2;
                        break;
                    case "-":
                        resultado = valor1 - valor2;
                        break;
                    case "*":
                        resultado = valor1 * valor2;
                        break;
                    case "/":
                        if (valor2 != 0)
                            resultado = valor1 / valor2;
                        else
                            throw new DivideByZeroException("No se puede dividir entre cero.");
                        break;
                    default:
                        throw new InvalidOperationException("Operación no válida.");
                }

                Pantalla.Text = resultado.ToString();
                GuardarOperacion(operacion, valor1, valor2, resultado);
                valor1 = resultado;
            }
            catch (FormatException)
            {
                MessageBox.Show("Error: Ingrese un número válido.");
                Pantalla.Clear();
            }
            catch (DivideByZeroException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}");
            }
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            if (Pantalla.Text.Length > 0)
                Pantalla.Text = Pantalla.Text.Substring(0, Pantalla.Text.Length - 1);

            if (Pantalla.Text.Length == 0)
                Pantalla.Text = "0";
        }

        private void BtnC_Click(object sender, EventArgs e)
        {
            Pantalla.Clear();
            valor1 = valor2 = 0;
            operacion = "";
        }
        private void BtnCos_Click(object sender, EventArgs e)
        {
            try
            {
                double valor = Convert.ToDouble(Pantalla.Text);
                double resultado = Math.Cos(valor * Math.PI / 180); 
                Pantalla.Text = resultado.ToString("N2");
                GuardarOperacion("Cos", valor, 0, resultado);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void BtnSen_Click(object sender, EventArgs e)
        {
            try
            {
                double valor = Convert.ToDouble(Pantalla.Text);
                double resultado = Math.Sin(valor * Math.PI / 180); 
                Pantalla.Text = resultado.ToString("N2");
                GuardarOperacion("Sen", valor, 0, resultado);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void BtnCE_Click(object sender, EventArgs e)
        {
            string[] partes = Pantalla.Text.Split(' ');

            if (partes.Length > 1)
            {
                Pantalla.Text = string.Join(" ", partes.Take(partes.Length - 1));
            }
            else
            {
                Pantalla.Text = "0";
            }
        }

        private void BtnDecimal_Click(object sender, EventArgs e)
        {
            if (!Pantalla.Text.Contains("."))
                Pantalla.Text += ".";
        }

        private void GuardarOperacion(string operacion, double valor1, double valor2, double resultado)
        {
            string descripcionOperacion = ObtenerDescripcionOperacion(operacion, valor1, valor2, resultado);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Operaciones (Operacion, Valor1, Valor2, Resultado) VALUES (@Operacion, @Valor1, @Valor2, @Resultado)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Operacion", descripcionOperacion);
                cmd.Parameters.AddWithValue("@Valor1", valor1);
                cmd.Parameters.AddWithValue("@Valor2", valor2);
                cmd.Parameters.AddWithValue("@Resultado", resultado);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private string ObtenerDescripcionOperacion(string operacion, double valor1, double valor2, double resultado)
        {
            string tipoOperacion;

            switch (operacion)
            {
                case "+":
                    tipoOperacion = "Suma";
                    break;
                case "-":
                    tipoOperacion = "Resta";
                    break;
                case "*":
                    tipoOperacion = "Multiplicación";
                    break;
                case "/":
                    tipoOperacion = "División";
                    break;
                case "%":
                    tipoOperacion = "Porcentaje";
                    break;
                case "√":
                    tipoOperacion = "Raíz Cuadrada";
                    break;
                case "x²":
                    tipoOperacion = "Cuadrado";
                    break;
                case "Cos":
                    tipoOperacion = "Coseno";
                    break;
                case "Sen":
                    tipoOperacion = "Seno";
                    break;
                default:
                    tipoOperacion = "Operación Desconocida";
                    break;
            }

            string descripcionOperacion = operacion == "√" || operacion == "x²" || operacion == "%" ||
                                           operacion == "Cos" || operacion == "Sen"
                ? $"{tipoOperacion}: {valor1}"
                : $"{tipoOperacion}: {valor1} {operacion} {valor2}";

            return $"{descripcionOperacion}\nResultado: {resultado.ToString("N2")}";
        }

        private void BtnHistorial_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT Operacion FROM Operaciones";
                    SqlCommand cmd = new SqlCommand(query, con);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    StringBuilder historial = new StringBuilder();

                    if (!reader.HasRows)
                    {
                        MessageBox.Show("No se encontraron operaciones en el historial.");
                        return;
                    }

                    while (reader.Read())
                    {
                        historial.AppendLine(reader["Operacion"].ToString());
                        historial.AppendLine(new string('-', 40));
                    }

                    con.Close();
                    MessageBox.Show(historial.ToString(), "Historial de Operaciones");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el historial: {ex.Message}");
            }
        }

        private void BtnOperacion_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                try
                {
                    double valor1 = Convert.ToDouble(Pantalla.Text);
                    double resultado = 0;

                    switch (btn.Text)
                    {
                        case "√":
                            resultado = Math.Sqrt(valor1);
                            break;
                        case "x²":
                            resultado = Math.Pow(valor1, 2);
                            break;
                        case "%":
                            resultado = valor1 / 100;
                            break;
                        default:
                            MessageBox.Show("Operación no soportada.");
                            return;
                    }

                    Pantalla.Text = resultado.ToString();
                    GuardarOperacion(btn.Text, valor1, 0, resultado);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void BtnBorrar_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "DELETE FROM Operaciones; DBCC CHECKIDENT ('Operaciones', RESEED, 0);";
                    new SqlCommand(query, con).ExecuteNonQuery();
                    MessageBox.Show("Historial borrado exitosamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al borrar el historial: {ex.Message}");
            }
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}