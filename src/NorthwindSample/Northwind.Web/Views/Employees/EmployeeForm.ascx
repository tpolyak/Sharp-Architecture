<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeForm.ascx.cs" Inherits="Northwind.Web.Views.Employees.EmployeeForm" %>

<% using (Html.BeginForm()) { %>
    <%= Html.Hidden("id", (ViewData.Model != null) ? ViewData.Model.ID : 0) %>

    <!-- Although not shown here, be sure to use CSS driven forms over layout 
    via tables; see http://wufoo.com/gallery/ for a ton of examples and templates. -->
    <table>
        <tr>
            <td>First Name *</td>
            <td>
                <%= Html.TextBox("Employee.FirstName", 
                    (ViewData.Model != null) ? ViewData.Model.FirstName : "")%>
                <%= Html.ValidationMessage("Employee.FirstName")%>
            </td>
        </tr>
        <tr>
            <td>Last Name *</td>
            <td>
                <%= Html.TextBox("Employee.LastName", 
                    (ViewData.Model != null) ? ViewData.Model.LastName : "")%>
                <%= Html.ValidationMessage("Employee.LastName")%>
            </td>
        </tr>
        <tr>
            <td>Phone Extension *</td>
            <td>
                <%= Html.TextBox("Employee.PhoneExtension",
                                        (ViewData.Model != null) ? ViewData.Model.PhoneExtension : 0)%>
                <%= Html.ValidationMessage("Employee.PhoneExtension")%>
            </td>
        </tr>
        <tr>
            <td colspan="2"><%= Html.SubmitButton() %></td>
        </tr>
    </table>
<% } %>
