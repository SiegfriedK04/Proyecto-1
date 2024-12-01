using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.UI;

namespace Pokemon
{
    public partial class Registro : Page
    {
        private readonly string apiBaseUrl = "https://localhost:44392/api/usuarios/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loginPanel.Visible = true;
                registroPanel.Visible = false;
            }
        }

        protected async void IniciarSesion(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contrasena))
            {
                lblMensajeLogin.Text = "Por favor, ingresa usuario y contraseña.";
                lblMensajeLogin.CssClass = "message error";
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"{apiBaseUrl}buscar/{usuario}");
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsAsync<dynamic>();

                        if (data != null && contrasena == data.Contrasena.ToString())
                        {
                            Session["UsuarioID"] = data.UsuarioID.ToString();
                            Session["NombreUsuario"] = data.NombreUsuario.ToString();

                            Response.Redirect("Coleccion.aspx", false);
                        }
                        else
                        {
                            lblMensajeLogin.Text = "Usuario o contraseña incorrectos.";
                            lblMensajeLogin.CssClass = "message error";
                        }
                    }
                    else
                    {
                        lblMensajeLogin.Text = "Usuario no encontrado.";
                        lblMensajeLogin.CssClass = "message error";
                    }
                }
                catch (Exception ex)
                {
                    lblMensajeLogin.Text = "Error al conectarse al servidor.";
                    lblMensajeLogin.CssClass = "message error";
                }
            }
        }

        protected async void RegistrarUsuario(object sender, EventArgs e)
        {
            string usuario = txtNuevoUsuario.Text.Trim();
            string email = txtEmail.Text.Trim();
            string contrasena = txtNuevaContrasena.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(contrasena))
            {
                lblMensajeRegistro.Text = "Por favor, completa todos los campos.";
                lblMensajeRegistro.CssClass = "message error";
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                var registroRequest = new { NombreUsuario = usuario, Email = email, Contrasena = contrasena };

                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(apiBaseUrl + "registro", registroRequest);
                    if (response.IsSuccessStatusCode)
                    {
                        lblMensajeRegistro.Text = "Usuario registrado con éxito.";
                        lblMensajeRegistro.CssClass = "message success";
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        lblMensajeRegistro.Text = "El usuario ya existe.";
                        lblMensajeRegistro.CssClass = "message error";
                    }
                    else
                    {
                        lblMensajeRegistro.Text = "Error al registrar el usuario.";
                        lblMensajeRegistro.CssClass = "message error";
                    }
                }
                catch (Exception ex)
                {
                    lblMensajeRegistro.Text = $"Error al conectarse al servidor: {ex.Message}";
                    lblMensajeRegistro.CssClass = "message error";
                }
            }
        }

        protected void MostrarRegistro(object sender, EventArgs e)
        {
            loginPanel.Visible = false;
            registroPanel.Visible = true;
        }

        protected void MostrarLogin(object sender, EventArgs e)
        {
            registroPanel.Visible = false;
            loginPanel.Visible = true;
        }
    }
}