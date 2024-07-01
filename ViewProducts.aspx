<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewProducts.aspx.cs" Inherits="ProductCreation.ViewProducts" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View CASA Products Page</title>
    <style>
        * {
            padding:0;
            margin: 0
        }
        .gridview {
            width: 100%;
            margin-top: 1em 
        }
        .gridview th, .gridview td {
    text-align: left;
    padding: 0.5em;
    border-color: orangered;
        }
        .gridview th {
            background-color: orangered;
           
        }
    </style>
</head>
<body class="w-full m-0 p-0" style="width: 100%; height:100%; background-color: white">
    <form id="form1" runat="server">
        <div style="padding: 2em">
              <h2>CASA Products</h2>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" CssClass="gridview"></asp:GridView>
        </div>
    </form>
</body>
</html>
