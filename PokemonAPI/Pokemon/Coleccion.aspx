<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Coleccion.aspx.cs" Inherits="Pokemon.Coleccion" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Colección de Cartas</title>

    <style>
    body {
        font-family: Arial, sans-serif;
        margin: 0;
        padding: 0;
        background: url('https://imgur.com/9wHryky.png') no-repeat center center fixed;
        background-size: cover;
    }

    body:before {
        content: '';
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(255, 255, 255, 0.4); 
        z-index: -1;
    }

    .container {
        max-width: 1100px;
        margin: 20px auto;
        background-color: white;
        border-radius: 15px;
        box-shadow: 0 8px 15px rgba(0, 0, 0, 0.3);
        padding: 20px;
    }

    .top-bar {
        position: fixed;
        top: 0;
        right: 0;
        background-color: #f1f1f1;
        padding: 10px 15px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        border-bottom-left-radius: 8px;
        display: flex;
        align-items: center;
        gap: 10px;
        z-index: 1000;
    }

    .user-label {
        font-size: 20px;
        font-weight: normal;
        color: #555;
    }

    .btn-logout {
        background-color: #e74c3c;
        color: white;
        padding: 5px 10px;
        font-size: 12px;
        border: none;
        border-radius: 3px;
        cursor: pointer;
        transition: all 0.2s ease;
    }

    .btn-logout:hover {
        background-color: #c0392b;
        transform: scale(1.05);
    }

    /* Filtros */
    .filters {
        display: flex;
        flex-wrap: wrap;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 15px;
        gap: 15px;
    }

    .filters label {
        margin-right: 5px;
        font-weight: bold;
    }

    select, input {
        font-family: Arial, sans-serif;
        padding: 8px 12px;
        font-size: 14px;
        border: 2px solid #ccc;
        border-radius: 8px;
        background-color: #f9f9f9;
        transition: all 0.3s ease;
        outline: none;
    }

    select:hover, input:hover {
        background-color: #e0e0e0;
        border-color: #007bff;
    }

    select:focus, input:focus {
        border-color: #0056b3;
        box-shadow: 0 0 5px rgba(0, 123, 255, 0.5);
    }

    /* Botones */
    .action-buttons {
        display: flex;
        justify-content: flex-end;
        gap: 15px;
        margin-bottom: 15px;
    }

    .btn {
        background-color: #007bff;
        color: white;
        padding: 10px 20px;
        font-size: 14px;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        transition: all 0.3s ease;
    }

    .btn:hover {
        background-color: #0056b3;
        transform: scale(1.05);
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
    }

    .btnadd {
    background-color: #28a745;
    color: white;
    padding: 10px 20px;
    font-size: 14px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    transition: all 0.3s ease;
    }

    .btnadd:hover {
        background-color: #218838;
        transform: scale(1.05);
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
    }

    .mensaje-confirmacion {
        color: green;
        font-size: 20px;
        text-align: center;
        margin-top: 10px;
    }

    .mensaje-error {
        color: red;
        font-size: 14px;
        text-align: center;
        margin-top: 10px;
    }

    .card-container {
        display: flex;
        flex-wrap: wrap;
        justify-content: center;
        gap: 15px;
        padding: 20px;
    }

    .card {
        width: 180px;
        height: 250px;
        text-align: center;
        overflow: hidden;
        border-radius: 10px;
        border: 1px solid #ccc;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        position: relative;
        transition: transform 0.2s ease-in-out, border-color 0.2s ease-in-out;
    }

    .card:hover {
        transform: scale(1.05);
    }

    .card img {
        width: 100%;
        height: auto;
        display: block;
    }

    .card .counter {
        position: absolute;
        bottom: 10px;
        right: 10px;
        background-color: white;
        color: black;
        font-size: 14px;
        padding: 5px;
        border-radius: 50%;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
    }

    .select-checkbox {
        position: absolute;
        top: 10px;
        left: 10px;
        z-index: 10;
        display: none; 
        transform: scale(1.5); 
        color: #007bff; 
    }

    .card:hover .select-checkbox {
        display: block;
    }

    .select-checkbox:checked {
        display: block;
        color: #007bff; 
    }

    .card.selected {
        border: 3px solid #007bff;
        box-shadow: 0 4px 8px rgba(0, 123, 255, 0.5);
        transition: border-color 0.3s ease, box-shadow 0.3s ease;
    }

</style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="top-bar">
            <asp:Label ID="lblUsuario" runat="server" CssClass="user-label"></asp:Label>
            <asp:Button ID="btnCerrarSesion" runat="server" CssClass="btn-logout" Text="Cerrar Sesión" OnClick="CerrarSesion" />
        </div>

        <div class="container">
            <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-confirmacion" Text="" />
            <div class="action-buttons">
                <asp:Button ID="btnAgregarSeleccion" runat="server" CssClass="btnadd" Text="Agregar cartas a Mi Colección" OnClick="AgregarSeleccion" />
            </div>

            <div class="filters">
                <label for="ddlRareza">Rareza:</label>
                <asp:DropDownList ID="ddlRareza" runat="server" AutoPostBack="true" CssClass="select-style" OnSelectedIndexChanged="AplicarFiltros">
                    <asp:ListItem Text="Todas" Value="" />
                    <asp:ListItem Text="1 diamante" Value="1 diamante" />
                    <asp:ListItem Text="2 diamantes" Value="2 diamantes" />
                    <asp:ListItem Text="3 diamantes" Value="3 diamantes" />
                    <asp:ListItem Text="4 diamantes" Value="4 diamantes" />
                    <asp:ListItem Text="1 estrella" Value="1 estrella" />
                    <asp:ListItem Text="2 estrellas" Value="2 estrellas" />
                    <asp:ListItem Text="3 estrellas" Value="3 estrellas" />
                    <asp:ListItem Text="1 corona" Value="1 corona" />
                </asp:DropDownList>

                <label for="ddlTipo">Tipo:</label>
                <asp:DropDownList ID="ddlTipo" runat="server" AutoPostBack="true" CssClass="select-style" OnSelectedIndexChanged="AplicarFiltros">
                    <asp:ListItem Text="Todos" Value="" />
                    <asp:ListItem Text="Acero" Value="Acero" />
                    <asp:ListItem Text="Agua" Value="Agua" />
                    <asp:ListItem Text="Dragon" Value="Dragon" />
                    <asp:ListItem Text="Electrico" Value="Electrico" />
                    <asp:ListItem Text="Fuego" Value="Fuego" />
                    <asp:ListItem Text="Lucha" Value="Lucha" />
                    <asp:ListItem Text="Normal" Value="Normal" />
                    <asp:ListItem Text="Oscuro" Value="Oscuro" />
                    <asp:ListItem Text="Planta" Value="Planta" />
                    <asp:ListItem Text="Psiquico" Value="Psiquico" />
                    <asp:ListItem Text="Item" Value="Item" />
                    <asp:ListItem Text="Supporter" Value="Supporter" />
                </asp:DropDownList>

                <label for="ddlRegion">Región:</label>
                <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="true" CssClass="select-style" OnSelectedIndexChanged="AplicarFiltros">
                    <asp:ListItem Text="Todas" Value="" />
                    <asp:ListItem Text="Kanto" Value="Kanto" />
                    <asp:ListItem Text="Johto" Value="Johto" />
                    <asp:ListItem Text="Hoenn" Value="Hoenn" />
                    <asp:ListItem Text="Sinnoh" Value="Sinnoh" />
                    <asp:ListItem Text="Unova" Value="Unova" />
                    <asp:ListItem Text="Kalos" Value="Kalos" />
                    <asp:ListItem Text="Alola" Value="Alola" />
                    <asp:ListItem Text="Galar" Value="Galar" />
                </asp:DropDownList>

                <label for="ddlHabilidad">Habilidad:</label>
                <asp:DropDownList ID="ddlHabilidad" runat="server" AutoPostBack="true" CssClass="select-style" OnSelectedIndexChanged="AplicarFiltros">
                    <asp:ListItem Text="Todas" Value="" />
                    <asp:ListItem Text="Sí" Value="Y" />
                    <asp:ListItem Text="No" Value="N" />
                </asp:DropDownList>
            </div>

            <div class="search-container">
                <label for="txtBusqueda">Buscar:</label>
                <asp:TextBox ID="txtBusqueda" runat="server" CssClass="select-style" />
                <asp:Button ID="btnBuscar" runat="server" CssClass="btn" Text="Buscar" OnClick="BuscarPorNombre" />
            </div>

            <div class="action-buttons">
                <asp:Button ID="btnMostrarMiColeccion" runat="server" CssClass="btn" Text="Mostrar Mi Colección" OnClick="MostrarMiColeccion" />
                <asp:Button ID="btnMostrarTodas" runat="server" CssClass="btn" Text="Mostrar Todas las Cartas" OnClick="MostrarTodasLasCartas" />
            </div>

            <div class="card-container">
                <asp:Repeater ID="rptCartas" runat="server">
                    <ItemTemplate>
                        <div class="card <%# CartasSeleccionadas.Contains(Convert.ToInt32(Eval("ID"))) ? "selected" : "" %>">
                            <asp:HiddenField ID="hfCartaID" runat="server" Value='<%# Eval("ID") %>' />

                            <asp:CheckBox ID="chkSeleccionar" runat="server" CssClass="select-checkbox" 
                                          Checked='<%# CartasSeleccionadas.Contains(Convert.ToInt32(Eval("ID"))) %>'
                                          AutoPostBack="true" OnCheckedChanged="ActualizarSeleccion" />

                            <a href="DetalleCarta.aspx?ID=<%# Eval("ID") %>">
                                <img src='<%# Eval("ImagenURL") %>' alt="Carta" />
                            </a>

                            <div class="counter"><%# Eval("Cantidad") %></div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </form>
</body>
</html>