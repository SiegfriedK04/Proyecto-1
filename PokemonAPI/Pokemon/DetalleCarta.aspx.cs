using Pokemon.App_Start;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pokemon
{
    public partial class DetalleCarta : System.Web.UI.Page
    {
        private readonly string apiBaseUrl = "https://localhost:44392/api/cartas/";
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = Request.QueryString["ID"];

                if (!string.IsNullOrEmpty(id) && int.TryParse(id, out int cartaID))
                {
                    await CargarDetalleCarta(cartaID);
                }
                else
                {
                    Response.Redirect("Coleccion.aspx");
                }
            }
        }

        private async Task CargarDetalleCarta(int cartaID)
        {
            string usuarioID = Session["UsuarioID"]?.ToString() ?? "0";
            string apiUrl = $"{apiBaseUrl}detalles/{cartaID}?usuarioID={usuarioID}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"Llamando al API: {apiUrl}");
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsAsync<dynamic>();

                        System.Diagnostics.Debug.WriteLine($"Datos recibidos del API: {data}");

                        lblPokedex.Text = data.Pokedex?.ToString() ?? "N/A";
                        lblNombre.Text = data.Nombre?.ToString() ?? "N/A";
                        lblTipo.Text = data.Tipo?.ToString() ?? "N/A";
                        lblRegion.Text = data.Region?.ToString() ?? "N/A";
                        lblRareza.Text = data.Rareza?.ToString() ?? "N/A";
                        lblHP.Text = data.HP?.ToString() ?? "N/A";
                        lblDebilidad.Text = data.Debilidad?.ToString() ?? "N/A";
                        lblHabilidad.Text = data.Habilidad?.ToString() ?? "N/A";
                        imgCarta.ImageUrl = data.ImagenURL?.ToString() ?? string.Empty;

                        lblCantidad.Text = data.Cantidad?.ToString() ?? "0";

                        ViewState["CartaID"] = cartaID;
                    }
                    else
                    {
                        lblMensaje.Text = "No se encontró la carta.";
                        lblMensaje.CssClass = "mensaje-error";
                        System.Diagnostics.Debug.WriteLine($"Error del API: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    lblMensaje.Text = $"Error al cargar detalles: {ex.Message}";
                    lblMensaje.CssClass = "mensaje-error";
                    System.Diagnostics.Debug.WriteLine($"Excepción: {ex.Message}");
                }
            }
        }

        protected void AgregarAColeccion(object sender, EventArgs e)
        {
            if (Session["UsuarioID"] == null)
            {
                Response.Redirect("Registro.aspx");
                return;
            }

            int usuarioID = int.Parse(Session["UsuarioID"].ToString());
            int cartaID = (int)ViewState["CartaID"];

            BD db = new BD();

            string queryVerificar = @"
                SELECT Cantidad 
                FROM Colecciones 
                WHERE UsuarioID = @UsuarioID AND CartaID = @CartaID";

            SqlParameter[] parametrosVerificar = {
                new SqlParameter("@UsuarioID", usuarioID),
                new SqlParameter("@CartaID", cartaID)
            };

            DataTable dt = db.EjecutarConsulta(queryVerificar, parametrosVerificar);

            if (dt.Rows.Count > 0) 
            {
                string queryActualizar = @"
                    UPDATE Colecciones 
                    SET Cantidad = Cantidad + 1 
                    WHERE UsuarioID = @UsuarioID AND CartaID = @CartaID";

                SqlParameter[] parametrosActualizar = {
                    new SqlParameter("@UsuarioID", usuarioID),
                    new SqlParameter("@CartaID", cartaID)
                };

                db.EjecutarComando(queryActualizar, parametrosActualizar);
            }
            else 
            {
                string queryInsertar = @"
                    INSERT INTO Colecciones (UsuarioID, CartaID, Cantidad)
                    VALUES (@UsuarioID, @CartaID, 1)";

                SqlParameter[] parametrosInsertar = {
                    new SqlParameter("@UsuarioID", usuarioID),
                    new SqlParameter("@CartaID", cartaID)
                };

                db.EjecutarComando(queryInsertar, parametrosInsertar);
            }

            Response.Redirect("Coleccion.aspx?view=miColeccion");
        }

        protected void BorrarUnaCopia(object sender, EventArgs e)
        {
            if (Session["UsuarioID"] == null || ViewState["CartaID"] == null)
            {
                Response.Redirect("Registro.aspx");
                return;
            }

            int usuarioID = int.Parse(Session["UsuarioID"].ToString());
            int cartaID = (int)ViewState["CartaID"];
            BD db = new BD();

            string queryCantidad = @"
                SELECT Cantidad 
                FROM Colecciones 
                WHERE UsuarioID = @UsuarioID AND CartaID = @CartaID";

            SqlParameter[] parametrosConsulta = {
                new SqlParameter("@UsuarioID", usuarioID),
                new SqlParameter("@CartaID", cartaID)
            };

            DataTable dt = db.EjecutarConsulta(queryCantidad, parametrosConsulta);

            if (dt.Rows.Count > 0)
            {
                int cantidadActual = int.Parse(dt.Rows[0]["Cantidad"].ToString());

                if (cantidadActual > 1)
                {
                    string queryActualizar = @"
                        UPDATE Colecciones 
                        SET Cantidad = Cantidad - 1 
                        WHERE UsuarioID = @UsuarioID AND CartaID = @CartaID";

                    SqlParameter[] parametrosActualizar = {
                        new SqlParameter("@UsuarioID", usuarioID),
                        new SqlParameter("@CartaID", cartaID)
                    };

                    db.EjecutarComando(queryActualizar, parametrosActualizar);

                    lblMensajeConfirmacion.Text = "Una copia de la carta fue eliminada de tu colección.";
                    lblMensajeConfirmacion.CssClass = "mensaje-confirmacion";
                }
                else
                {
                    lblMensajeConfirmacion.Text = "Solo tienes una copia de la carta. Usa el botón 'Borrar Todas las Copias' si deseas eliminarla.";
                    lblMensajeConfirmacion.CssClass = "mensaje-error";
                }
            }
            else
            {
                lblMensajeConfirmacion.Text = "La carta no se encuentra en tu colección.";
                lblMensajeConfirmacion.CssClass = "mensaje-error";
            }

            Response.Redirect("Coleccion.aspx?view=miColeccion");
        }


        protected void BorrarTodasCopias(object sender, EventArgs e)
        {
            if (Session["UsuarioID"] == null || ViewState["CartaID"] == null)
            {
                Response.Redirect("Registro.aspx");
                return;
            }

            int usuarioID = int.Parse(Session["UsuarioID"].ToString());
            int cartaID = (int)ViewState["CartaID"];
            BD db = new BD();

            string queryEliminar = @"
                DELETE FROM Colecciones 
                WHERE UsuarioID = @UsuarioID AND CartaID = @CartaID";

            SqlParameter[] parametrosEliminar = {
                new SqlParameter("@UsuarioID", usuarioID),
                new SqlParameter("@CartaID", cartaID)
            };

            db.EjecutarComando(queryEliminar, parametrosEliminar);

            lblMensajeConfirmacion.Text = "Todas las copias de la carta fueron eliminadas de tu colección.";
            lblMensajeConfirmacion.CssClass = "mensaje-confirmacion";

            Response.Redirect("Coleccion.aspx?view=miColeccion");
        }

        protected void Regresar(object sender, EventArgs e)
        {
            Response.Redirect("Coleccion.aspx");
        }

        protected void ActualizarCantidad(object sender, EventArgs e)
        {
            if (Session["UsuarioID"] == null || ViewState["CartaID"] == null)
            {
                Response.Redirect("Registro.aspx");
                return;
            }

            int usuarioID = int.Parse(Session["UsuarioID"].ToString());
            int cartaID = (int)ViewState["CartaID"];

            if (int.TryParse(txtCantidad.Text, out int nuevaCantidad) && nuevaCantidad >= 0)
            {
                BD db = new BD();

                try
                {
                    if (nuevaCantidad == 0)
                    {
                        string queryEliminar = @"
                            DELETE FROM Colecciones 
                            WHERE UsuarioID = @UsuarioID AND CartaID = @CartaID";

                                    SqlParameter[] parametrosEliminar = {
                                        new SqlParameter("@UsuarioID", usuarioID),
                                        new SqlParameter("@CartaID", cartaID)
                                    };

                        db.EjecutarComando(queryEliminar, parametrosEliminar); 

                        lblMensajeConfirmacion.Text = "La carta ha sido eliminada de tu colección.";
                        lblMensajeConfirmacion.CssClass = "mensaje-confirmacion";
                    }
                    else
                    {
                        string queryActualizar = @"
                            UPDATE Colecciones 
                            SET Cantidad = @Cantidad 
                            WHERE UsuarioID = @UsuarioID AND CartaID = @CartaID";

                                    SqlParameter[] parametrosActualizar = {
                                        new SqlParameter("@UsuarioID", usuarioID),
                                        new SqlParameter("@CartaID", cartaID),
                                        new SqlParameter("@Cantidad", nuevaCantidad)
                                    };

                        db.EjecutarComando(queryActualizar, parametrosActualizar); 

                        lblMensajeConfirmacion.Text = "La cantidad ha sido actualizada correctamente.";
                        lblMensajeConfirmacion.CssClass = "mensaje-confirmacion";
                    }
                }
                catch (Exception ex)
                {
                    lblMensajeConfirmacion.Text = $"Error al actualizar la colección: {ex.Message}";
                    lblMensajeConfirmacion.CssClass = "mensaje-error";
                }
            }
            else
            {
                lblMensajeConfirmacion.Text = "Por favor, ingresa una cantidad válida.";
                lblMensajeConfirmacion.CssClass = "mensaje-error";
            }
        }
    }
}