<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Northwind.Web.Views.Categories.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div>
        <p>Category Details:</p>
        <p>ID: <%= ViewData.Model.ID %></p>
        <p>Name: <%= ViewData.Model.Name%></p>
    </div>
</asp:Content>
