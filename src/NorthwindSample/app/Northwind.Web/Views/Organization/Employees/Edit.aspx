<%@ Page Title="Edit Employee" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Northwind.Web.Views.Organization.Employees.Edit" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h2>Edit Employee</h2>

<% Html.RenderPartial("EmployeeForm", ViewData.Model, ViewData); %>

</asp:Content>
