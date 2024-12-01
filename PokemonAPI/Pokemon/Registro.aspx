<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="Pokemon.Registro" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pantalla Inicial</title>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/smoothness/jquery-ui.css"/>

    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            background: url('https://imgur.com/9wHryky.png') no-repeat center center fixed;
            background-size: cover;
        }

        .container {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            width: 320px;
            background-color: #fff;
            border-radius: 10px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
            padding: 20px;
            display: flex;
            flex-direction: column;
            justify-content: center; 
            align-items: center; 
            text-align: center;
        }

        .form-group {
            width: 100%; 
            margin-bottom: 15px;
            display: flex; 
            flex-direction: column; 
            align-items: center; 
        }

        .form-group label {
            font-size: 14px;
            color: #555;
            margin-bottom: 5px;
            text-align: center; 
        }

        .form-group input {
            width: 100%;
            padding: 8px;
            font-size: 14px;
            border: 1px solid #ccc;
            border-radius: 5px;
            outline: none;
            transition: all 0.3s ease;
            text-align: left; 
        }

        .btn {
            background-color: #007bff;
            color: white;
            padding: 15px 20px;
            font-size: 16px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            width: 100%;
            margin-top: 10px;
            transition: all 0.3s ease;
        }

        .btn:hover {
            background-color: #0056b3;
        }

        .btn-secondary {
            background-color: #6c757d;
        }

        .btn-secondary:hover {
            background-color: #5a6268;
        }

        .message {
            font-size: 14px;
            margin-top: 10px;
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">
        <div class="container" id="movableContainer">
            <asp:Panel ID="loginPanel" runat="server" Visible="true">
                <h1>Iniciar Sesión</h1>
                <div class="form-group">
                    <label for="txtUsuario">Nombre de Usuario</label>
                    <asp:TextBox ID="txtUsuario" runat="server" />
                </div>
                <div class="form-group">
                    <label for="txtContrasena">Contraseña</label>
                    <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" />
                </div>
                <asp:Button ID="btnLogin" runat="server" Text="Iniciar Sesión" CssClass="btn" OnClick="IniciarSesion" />
                <asp:Button ID="btnMostrarRegistro" runat="server" Text="Registrarse" CssClass="btn btn-secondary" OnClick="MostrarRegistro" />
                <div class="message">
                    <asp:Label ID="lblMensajeLogin" runat="server" />
                </div>
            </asp:Panel>

            <asp:Panel ID="registroPanel" runat="server" Visible="false">
                <h1>Registro</h1>
                <div class="form-group">
                    <label for="txtNuevoUsuario">Nombre de Usuario</label>
                    <asp:TextBox ID="txtNuevoUsuario" runat="server" />
                </div>
                <div class="form-group">
                    <label for="txtEmail">Correo Electrónico</label>
                    <asp:TextBox ID="txtEmail" runat="server" />
                </div>
                <div class="form-group">
                    <label for="txtNuevaContrasena">Contraseña</label>
                    <asp:TextBox ID="txtNuevaContrasena" runat="server" TextMode="Password" />
                </div>
                <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" CssClass="btn" OnClick="RegistrarUsuario" />
                <asp:Button ID="btnMostrarLogin" runat="server" Text="Volver al Inicio" CssClass="btn btn-secondary" OnClick="MostrarLogin" />
                <div class="message">
                    <asp:Label ID="lblMensajeRegistro" runat="server" />
                </div>
            </asp:Panel>
        </div>
    </form>

    <script>
        $(function () {
            $("#movableContainer").draggable();
        });
    </script>
</body>
</html>