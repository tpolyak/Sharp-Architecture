<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="Northwind.Web.Views.Employees.Create" %>
<%@ Import Namespace="Northwind.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h2>Create Employee</h2>

<% Html.RenderPartial("EmployeeForm"); %>

</asp:Content>
