<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Proyecto2Client.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Calculadora de Operaciones</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f5f5f5; 
            color: #333; 
        }
        .container {
            width: 90%;
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            background-color: #ffffff;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
            border-radius: 10px;
        }
        h1 {
            text-align: center;
            font-size: 24px;
            color: #444;
        }
        label {
            font-size: 14px;
            margin-bottom: 5px;
            display: block;
        }
        input, select {
            width: 100%;
            padding: 8px;
            margin: 10px 0;
            box-sizing: border-box;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-size: 14px;
        }
        .btn {
            padding: 8px 15px;
            font-size: 14px;
            color: white;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            transition: background-color 0.3s, transform 0.2s;
            margin: 5px 0;
        }
        .btn:hover {
            transform: scale(1.05);
        }
        .btn-agregar {
            background-color: #7BA4C3; 
        }
        .btn-agregar:hover {
            background-color: #5A7D99; 
        }
        .btn-historial {
            background-color: #7A93AC; 
        }
        .btn-historial:hover {
            background-color: #627488; 
        }
        .btn-eliminar {
            background-color: #617073; 
        }
        .btn-eliminar:hover {
            background-color: #4A585E; 
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 20px 0;
        }
        th, td {
            text-align: center;
            padding: 10px;
            border: 1px solid #ddd;
        }
        th {
            background-color: #7A93AC;
            color: white;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Calculadora de Operaciones</h1>

            <!-- Agregar Operación -->
            <label for="txtOperacion">Operación:</label>
            <asp:TextBox ID="txtOperacion" runat="server" Placeholder="Suma, Resta, etc."></asp:TextBox>

            <label for="txtValor1">Valor 1:</label>
            <asp:TextBox ID="txtValor1" runat="server" Placeholder="Ejemplo: 10"></asp:TextBox>

            <label for="txtValor2">Valor 2:</label>
            <asp:TextBox ID="txtValor2" runat="server" Placeholder="Ejemplo: 5"></asp:TextBox>

            <asp:Button ID="btnAgregar" runat="server" Text="Agregar Operación" CssClass="btn btn-agregar" OnClick="btnAgregar_Click" />

            <!-- Agregar Filtros -->

            <label for="filtroOperaciones">Filtrar Operaciones:</label>
            <asp:DropDownList ID="filtroOperaciones" runat="server">
                <asp:ListItem Text="Seleccionar" Value="seleccionar" />
                <asp:ListItem Text="Suma (+)" Value="suma" />
                <asp:ListItem Text="Resta (-)" Value="resta" />
                <asp:ListItem Text="Multiplicación (*)" Value="multiplicacion" />
                <asp:ListItem Text="División (/)" Value="division" />
                <asp:ListItem Text="Raíz Cuadrada (√)" Value="raiz cuadrada" />
                <asp:ListItem Text="Potencia (x²)" Value="potencia" />
                <asp:ListItem Text="Seno (sen)" Value="seno" />
                <asp:ListItem Text="Coseno (cos)" Value="coseno" />
            </asp:DropDownList>
            <asp:Button ID="btnFiltrar" runat="server" Text="Aplicar Filtro" CssClass="btn btn-historial" OnClick="btnFiltrar_Click" />

            <asp:Button ID="btnVerHistorial" runat="server" Text="Ver Historial Completo" CssClass="btn btn-historial" OnClick="btnVerHistorial_Click" />

            <!-- Eliminar y Borrar -->
            <label for="txtEliminarId">Eliminar Operación por ID:</label>
            <asp:TextBox ID="txtEliminarId" runat="server" Placeholder="Ejemplo: 1"></asp:TextBox>
            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar Operación" CssClass="btn btn-eliminar" OnClick="btnEliminar_Click" />
            <asp:Button ID="btnBorrarHistorial" runat="server" Text="Borrar Historial Completo" CssClass="btn btn-eliminar" OnClick="btnBorrarHistorial_Click" />

            <!-- Tabla de Resultados -->
            <asp:Literal ID="TablaPlaceholder" runat="server"></asp:Literal>
        </div>
    </form>
</body>
</html>