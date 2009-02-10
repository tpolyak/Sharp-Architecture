<%@ Page Title="Edit Employee" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewPage<Northwind.Core.Organization.Employee>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h2>Edit Employee</h2>

<% Html.RenderPartial("EmployeeForm", ViewData.Model, ViewData); %>

</asp:Content>
