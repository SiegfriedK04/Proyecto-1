using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pokemon.App_Start;

namespace Pokemon
{
    public partial class Coleccion : System.Web.UI.Page
    {
        private readonly string connectionString = @"Server=.\SQLEXPRESS;Database=Pokemon;Trusted_Connection=True;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblUsuario.Text = Session["NombreUsuario"] != null ? Session["NombreUsuario"].ToString() : "Invitado";

                string view = Request.QueryString["view"];
                if (view == "miColeccion")
                {
                    ViewState["Context"] = "MiColeccion"; 
                    MostrarMiColeccion(sender, e);
                }
                else
                {
                    ViewState["Context"] = "Todas"; 
                    AplicarFiltros(sender, e); 
                }
            }
        }

        protected void BuscarPorNombre(object sender, EventArgs e)
        {
            string nombre = txtBusqueda.Text.Trim();
            string rareza = ddlRareza.SelectedValue;
            string tipo = ddlTipo.SelectedValue;
            string region = ddlRegion.SelectedValue;
            string context = ViewState["Context"]?.ToString();
            string habilidad = ddlHabilidad.SelectedValue;

            string query = @"
                SELECT Cartas.ID, Cartas.ImagenURL, 
                       ISNULL((SELECT Cantidad FROM Colecciones WHERE CartaID = Cartas.ID AND UsuarioID = @UsuarioID), 0) AS Cantidad 
                FROM Cartas
                WHERE (@Nombre = '' OR Nombre LIKE '%' + @Nombre + '%')
                  AND (@Rareza = '' OR Rareza = @Rareza)
                  AND (@Tipo = '' OR Tipo = @Tipo)
                  AND(@Habilidad = '' OR Habilidad = @Habilidad)
                  AND (@Region = '' OR Region = @Region)";


            if (context == "MiColeccion")
            {
                query += " AND EXISTS (SELECT 1 FROM Colecciones WHERE CartaID = Cartas.ID AND UsuarioID = @UsuarioID)";
            }

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@UsuarioID", Session["UsuarioID"] ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@Nombre", nombre);
                    comando.Parameters.AddWithValue("@Rareza", rareza);
                    comando.Parameters.AddWithValue("@Tipo", tipo);
                    comando.Parameters.AddWithValue("@Region", region);
                    comando.Parameters.AddWithValue("@Habilidad", habilidad);

                    conexion.Open();

                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        rptCartas.DataSource = reader;
                        rptCartas.DataBind();
                    }
                    else
                    {
                        rptCartas.DataSource = null;
                        rptCartas.DataBind();
                        lblMensaje.Text = "No se encontraron resultados para la búsqueda.";
                        lblMensaje.CssClass = "mensaje-confirmacion";
                    }
                }
            }
        }

        protected void CerrarSesion(object sender, EventArgs e)
        {
            Session.Clear(); 
            Response.Redirect("Registro.aspx"); 
        }

        protected void MostrarTodasLasCartas(object sender, EventArgs e)
        {
            ViewState["Context"] = "Todas"; 
            AplicarFiltros(sender, e);
        }

        protected void MostrarMiColeccion(object sender, EventArgs e)
        {
            ViewState["Context"] = "MiColeccion"; 
            AplicarFiltros(sender, e);
        }

        protected void AplicarFiltros(object sender, EventArgs e)
        {
            string rareza = ddlRareza.SelectedValue;
            string tipo = ddlTipo.SelectedValue;
            string region = ddlRegion.SelectedValue;
            string habilidad = ddlHabilidad.SelectedValue;
            string context = ViewState["Context"]?.ToString();

            string query = @"
                SELECT Cartas.ID, Cartas.ImagenURL, 
                       ISNULL((SELECT Cantidad FROM Colecciones WHERE CartaID = Cartas.ID AND UsuarioID = @UsuarioID), 0) AS Cantidad 
                FROM Cartas
                WHERE (@Rareza = '' OR Rareza = @Rareza)
                  AND (@Tipo = '' OR Tipo = @Tipo)
                  AND (@Region = '' OR Region = @Region)
                  AND (@Habilidad = '' OR Habilidad = @Habilidad)"; 

            if (context == "MiColeccion")
            {
                query += " AND EXISTS (SELECT 1 FROM Colecciones WHERE CartaID = Cartas.ID AND UsuarioID = @UsuarioID)";
            }

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@UsuarioID", Session["UsuarioID"] ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@Rareza", rareza);
                    comando.Parameters.AddWithValue("@Tipo", tipo);
                    comando.Parameters.AddWithValue("@Region", region);
                    comando.Parameters.AddWithValue("@Habilidad", habilidad);

                    conexion.Open();
                    rptCartas.DataSource = comando.ExecuteReader();
                    rptCartas.DataBind();
                }
            }

            btnAgregarSeleccion.Visible = context == "Todas";
        }

        protected List<int> CartasSeleccionadas
        {
            get
            {
                if (ViewState["CartasSeleccionadas"] == null)
                {
                    ViewState["CartasSeleccionadas"] = new List<int>();
                }
                return (List<int>)ViewState["CartasSeleccionadas"];
            }
            set
            {
                ViewState["CartasSeleccionadas"] = value;
            }
        }

        protected void AgregarSeleccion(object sender, EventArgs e)
        {
            if (Session["UsuarioID"] == null)
            {
                Response.Redirect("Registro.aspx");
                return;
            }

            int usuarioID = int.Parse(Session["UsuarioID"].ToString());
            BD db = new BD();

            if (CartasSeleccionadas.Count > 0)
            {
                foreach (int cartaID in CartasSeleccionadas)
                {
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
                }

                CartasSeleccionadas.Clear();

                lblMensaje.Text = "Las cartas seleccionadas se agregaron a tu colección.";
                lblMensaje.CssClass = "mensaje-confirmacion";

                AplicarFiltros(sender, e);
            }
            else
            {
                lblMensaje.Text = "No hay cartas seleccionadas.";
                lblMensaje.CssClass = "mensaje-error";
            }
        }

        protected void ActualizarSeleccion(object sender, EventArgs e)
        {
            CheckBox chkSeleccionar = (CheckBox)sender;
            RepeaterItem item = (RepeaterItem)chkSeleccionar.NamingContainer;
            HiddenField hfCartaID = (HiddenField)item.FindControl("hfCartaID");

            if (hfCartaID != null)
            {
                int cartaID = int.Parse(hfCartaID.Value);

                if (chkSeleccionar.Checked)
                {
                    if (!CartasSeleccionadas.Contains(cartaID))
                    {
                        CartasSeleccionadas.Add(cartaID);
                    }
                }
                else
                {
                    if (CartasSeleccionadas.Contains(cartaID))
                    {
                        CartasSeleccionadas.Remove(cartaID);
                    }
                }
            }

            AplicarFiltros(sender, e);
        }
    }
}