<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="DetalleCarta.aspx.cs" Inherits="Pokemon.DetalleCarta" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Detalle de Carta</title>
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
            max-width: 900px;
            margin: 50px auto;
            background-color: white;
            border-radius: 15px;
            border: 3px solid rgba(0, 0, 0, 0.2);
            box-shadow: 0 8px 15px rgba(0, 0, 0, 0.3); 
            overflow: hidden;
            padding: 20px;
        }

        .header {
            background-color: #007bff;
            color: white;
            padding: 20px;
            text-align: center;
            font-size: 26px;
            font-weight: bold;
            border-radius: 12px;
            margin-bottom: 20px;
        }

        .details {
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 20px;
            padding: 20px;
        }

        .card-image {
            flex: 1;
            max-width: 45%;
            text-align: center;
        }

        .card-image img {
            width: 100%;
            height: auto;
            object-fit: cover;
        }

        .card-info {
            flex: 1;
            max-width: 50%;
            background-color: #f9f9f9;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        .card-info h2 {
            margin-top: 0;
            color: #333;
            font-size: 28px;
        }

        .card-info p {
            margin: 15px 0;
            font-size: 18px;
            color: #555;
            line-height: 1.6;
        }

        .card-info p strong {
            color: #000;
        }

        .button-container {
            text-align: center;
            margin-top: 30px;
            display: flex;
            justify-content: center;
            gap: 15px;
        }

        .btn {
            padding: 12px 30px;
            font-size: 18px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            transition: all 0.3s ease;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }

        .btn:hover {
            transform: scale(1.05);
            box-shadow: 0 6px 8px rgba(0, 0, 0, 0.2);
        }

        .btn-agregar {
            background-color: #28a745;
            color: white;
        }

        .btn-agregar:hover {
            background-color: #218838;
        }

        .btn-delete-one {
            background-color: #f39c12; 
            color: white;
        }

        .btn-delete-one:hover {
            background-color: #e67e22;
        }

        .btn-delete-all {
            background-color: #e74c3c;
            color: white;
        }

        .btn-delete-all:hover {
            background-color: #c0392b;
        }

        .btn-secondary {
            background-color: #6c757d;
            color: white;
        }

        .btn-secondary:hover {
            background-color: #5a6268;
        }

        .mensaje-confirmacion {
            display: block;
            margin-top: 15px;
            font-size: 20px;
            color: green;
            text-align: center;
        }

        .input-cantidad {
            width: 80px;
            padding: 5px;
            font-size: 14px;
            border: 1px solid #ccc;
            border-radius: 5px;
            text-align: center;
        }

        .input-cantidad:focus {
            border-color: #007bff;
            box-shadow: 0 0 5px rgba(0, 123, 255, 0.5);
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="header">
                Detalle de la Carta
            </div>
            <div class="details">
                <div class="card-image">
                    <asp:Image ID="imgCarta" runat="server" AlternateText="Imagen de la Carta" />
                </div>

                <div class="card-info">
                    <h2><asp:Label ID="lblNombre" runat="server" /></h2>
                    <p><strong>Pokedex:</strong> <asp:Label ID="lblPokedex" runat="server" /></p>
                    <p><strong>Tipo:</strong> <asp:Label ID="lblTipo" runat="server" /></p>
                    <p><strong>Región:</strong> <asp:Label ID="lblRegion" runat="server" /></p>
                    <p><strong>Rareza:</strong> <asp:Label ID="lblRareza" runat="server" /></p>
                    <p><strong>HP:</strong> <asp:Label ID="lblHP" runat="server" /></p>
                    <p><strong>Debilidad:</strong> <asp:Label ID="lblDebilidad" runat="server" /></p>
                    <p><strong>Habilidad:</strong> <asp:Label ID="lblHabilidad" runat="server" /></p>
                    <p><strong>Cantidad:</strong> <asp:Label ID="lblCantidad" runat="server" /></p>

                    <p>
                        <strong>Editar cantidad:</strong>
                        <asp:TextBox ID="txtCantidad" runat="server" CssClass="input-cantidad" TextMode="Number" />
                        <asp:Button ID="btnActualizarCantidad" runat="server" CssClass="btn" Text="Actualizar" OnClick="ActualizarCantidad" />
                    </p>
                    <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-error"></asp:Label>
                </div>
            </div>

            <asp:Label ID="lblMensajeConfirmacion" runat="server" CssClass="mensaje-confirmacion" />

            <div class="button-container">
                <asp:Button ID="btnAgregarColeccion" runat="server" CssClass="btn btn-agregar" Text="Agregar a Mi Colección" OnClick="AgregarAColeccion" />
                <asp:Button ID="btnBorrarUnaCopia" runat="server" CssClass="btn btn-delete-one" Text="Borrar 1 Copia" OnClick="BorrarUnaCopia" />
                <asp:Button ID="btnBorrarTodasCopias" runat="server" CssClass="btn btn-delete-all" Text="Borrar Todas las Copias" OnClick="BorrarTodasCopias" />
                <asp:Button ID="btnRegresar" runat="server" CssClass="btn btn-secondary" Text="Regresar" OnClick="Regresar" />
            </div>
        </div>
    </form>
</body>
</html>