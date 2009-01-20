<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="Northwind.Web.Views.Categories.Create" %>
<%@ Import Namespace="Northwind.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h2>Category Created</h2>

<p>You just created a new category with a name of <%= ViewData.Model.CategoryName %>!</p>
<p>No really, go check the DB...there it is!</p>
</asp:Content>
