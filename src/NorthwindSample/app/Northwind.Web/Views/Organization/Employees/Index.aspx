<%@ Page Title="Employees" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Northwind.Web.Views.Organization.Employees.Index" %>
<%@ Import Namespace="Northwind.Core.Organization" %>
<%@ Import Namespace="Northwind.Web.Controllers.Organization" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Employees</h2>

    <% if (ViewContext.TempData["message"] != null){ %>
        <p><%= ViewContext.TempData["message"]%></p>
    <% } %>

    <table>
        <thead>
            <tr>
                <th>
                    Full Name
                </th>
                <th colspan="3">
                    Action
                </th>
            </tr>
        </thead>
		<%
		foreach (Employee employee in ViewData.Model) { %>
			<tr>
				<td><%= employee.FullName %></td>
				<td><%=Html.ActionLink<EmployeesController>(c => c.Show(employee.ID), "Details ")%></td>
				<td><%=Html.ActionLink<EmployeesController>(c => c.Edit(employee.ID), "Edit")%></td>
				<td><%=Html.ActionLink<EmployeesController>(c => c.Delete(employee.ID), "Delete")%></td>
			</tr>
		<%} %>
    </table>

    <p><%= Html.ActionLink<EmployeesController>(c => c.Create(), "Create New Employee") %></p>
</asp:Content>
