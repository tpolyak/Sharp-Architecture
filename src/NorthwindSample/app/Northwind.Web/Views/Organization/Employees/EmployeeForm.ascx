<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<Northwind.Core.Organization.Employee>" %>
<%@ Import Namespace="Northwind.Core.Organization" %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("id", (ViewData.Model != null) ? ViewData.Model.Id : 0) %>

    <!--
        Be sure to use CSS driven forms instead of layout via tables; 
        see http://wufoo.com/gallery/ for a ton of examples and templates.
        You can also Google "tableless forms" or "CSS forms."
    -->
    <ul>
        <li>
            <label for="employee.Firstname">First Name *</label>
            <%= Html.TextBox("employee.FirstName", 
                (ViewData.Model != null) ? ViewData.Model.FirstName : "")%>
            <%= Html.ValidationMessage("employee.FirstName")%>
        </li>
        <li>
            <label for="employee.LastName">Last Name *</label>
            <%= Html.TextBox("employee.LastName", 
                (ViewData.Model != null) ? ViewData.Model.LastName : "")%>
            <%= Html.ValidationMessage("employee.LastName")%>
        </li>
        <li>
            <label for="employee.LastName">Phone Extension *</label>
            <%= Html.TextBox("employee.PhoneExtension",
                (ViewData.Model != null) ? ViewData.Model.PhoneExtension : 0)%>
            <%= Html.ValidationMessage("employee.PhoneExtension")%>
        </li>
        <li>
            <%= Html.SubmitButton("btnSave", "Save Employee") %>
        </li>
    </ul>
<% } %>
