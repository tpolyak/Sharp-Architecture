<%@ Page Title="Employee Details" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Show.aspx.cs" Inherits="Northwind.Web.Views.Organization.Employees.Show" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Employee Details</h2>

    <ul>
        <li>
            <label for="Employee.FullName">Full Name:</label>
            <span id="Employee.FullName"><%= ViewData.Model.FullName %></span>
        </li>
    </ul>
</asp:Content>
