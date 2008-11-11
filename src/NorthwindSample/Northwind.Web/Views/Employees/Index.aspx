<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Northwind.Web.Views.Employees.Index" %>
<%@ Import Namespace="Northwind.Core" %>
<%@ Import Namespace="Northwind.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Employees</h2>

    <% if (ViewContext.TempData["message"] != null){ %>
        <p><%= ViewContext.TempData["message"]%></p>
    <% } %>

    <div>
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
            <asp:ListView ID="employeeList" runat="server">
                <LayoutTemplate>
                    <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# ((Employee) Container.DataItem).FullName %>
                        </td>
                        <td>
                            <%# Html.ActionLink<EmployeesController>(c => c.Show(((Employee)Container.DataItem).ID), "Details")%>
                        </td>
                        <td>
                            <%# Html.ActionLink<EmployeesController>(c => c.Edit(((Employee)Container.DataItem).ID), "Edit")%>
                        </td>
                        <td>
                            <%# Html.ActionLink<EmployeesController>(c => c.Delete(((Employee)Container.DataItem).ID), "Delete")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </table>
        <p>
            <%= Html.ActionLink<EmployeesController>(c => c.Create(), "Create New Employee") %>
        </p>
    </div>
</asp:Content>
