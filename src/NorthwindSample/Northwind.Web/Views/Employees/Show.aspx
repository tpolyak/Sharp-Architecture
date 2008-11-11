<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Show.aspx.cs" Inherits="Northwind.Web.Views.Employees.Show" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Employee Details</h2>

    <div>
        <p>
            Full Name:
            <%= ViewData.Model.FullName %>
        </p>
    </div>
</asp:Content>
