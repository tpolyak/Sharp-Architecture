<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Show.aspx.cs" Inherits="Northwind.Web.Views.Categories.Show" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Category Details</h2>

    <div>
        <p>
            ID:
            <%= ViewData.Model.ID %></p>
        <p>
            Name:
            <%= ViewData.Model.Name%></p>
    </div>
</asp:Content>
