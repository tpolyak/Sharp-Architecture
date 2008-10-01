<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Northwind.Web.Views.Customers.List" %>
<%@ Import Namespace="Northwind.Core" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>List Customers</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p>Customers:</p>
        <asp:ListView ID="customerList" runat="server">
            <LayoutTemplate>
                <ul>
                    <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                </ul>
            </LayoutTemplate>
            
            <ItemTemplate>
                <li>
                    <%# ((Customer) Container.DataItem).ContactName %> 
                    has placed <%# ((Customer) Container.DataItem).Orders.Count %> orders.
                </li>
            </ItemTemplate>
        </asp:ListView>
    </div>
    </form>
</body>
</html>
