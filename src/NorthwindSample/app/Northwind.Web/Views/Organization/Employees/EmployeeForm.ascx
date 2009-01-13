<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeForm.ascx.cs" Inherits="Northwind.Web.Views.Organization.Employees.EmployeeForm" %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.Hidden("id", (ViewData.Model != null) ? ViewData.Model.ID : 0) %>

    <!--
        Be sure to use CSS driven forms instead of layout via tables; 
        see http://wufoo.com/gallery/ for a ton of examples and templates.
        You can also Google "tableless forms" or "CSS forms."
    -->
    <ul>
        <li>
            <label for="Employee.Firstname">First Name *</label>
            <%= Html.TextBox("Employee.FirstName", 
                (ViewData.Model != null) ? ViewData.Model.FirstName : "")%>
            <%= Html.ValidationMessage("Employee.FirstName")%>
        </li>
        <li>
            <label for="Employee.LastName">Last Name *</label>
            <%= Html.TextBox("Employee.LastName", 
                (ViewData.Model != null) ? ViewData.Model.LastName : "")%>
            <%= Html.ValidationMessage("Employee.LastName")%>
        </li>
        <li>
            <label for="Employee.LastName">Phone Extension *</label>
            <%= Html.TextBox("Employee.PhoneExtension",
                (ViewData.Model != null) ? ViewData.Model.PhoneExtension : 0)%>
            <%= Html.ValidationMessage("Employee.PhoneExtension")%>
        </li>
        <li>
            <%= Html.SubmitButton("btnSave", "Save Employee") %>
        </li>
    </ul>
<% } %>
