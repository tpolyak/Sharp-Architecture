<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Northwind.Web.Views.Employees.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h2>Edit Employee</h2>

<% 
if (ViewData.Model != null) {
    Html.RenderPartial("EmployeeForm", ViewData.Model);
}
else {
    Html.RenderPartial("EmployeeForm");
}
%>

</asp:Content>
