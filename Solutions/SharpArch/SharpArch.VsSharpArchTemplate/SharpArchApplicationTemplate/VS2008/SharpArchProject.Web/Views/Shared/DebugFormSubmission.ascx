<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>

<hr />
<h2>ModelState</h2>

<table style="border-width:1px; border-color: Black; border-style:solid; width:100%">
    <thead>
        <tr>
            <th style="font-weight:bold">Key</th>
            <th style="font-weight:bold">Attempted Value (for display purposes)</th>
            <th style="font-weight:bold">Raw Value</th>
            <th style="font-weight:bold">Error Count</th>
            <th style="font-weight:bold">Error 1 Message</th>
        </tr>
    </thead>
    <tbody>
        <% foreach (string key in ViewData.ModelState.Keys) { %>
            <tr>
                <td><%= key %></td>
                <td><%= ViewData.ModelState[key].Value.AttemptedValue %></td>
                <td><%= ViewData.ModelState[key].Value.RawValue %></td>
                <td><%= ViewData.ModelState[key].Errors.Count %></td>
                <td>
                    <% if (ViewData.ModelState[key].Errors.Count > 0) {
                           for (int i=0; i < ViewData.ModelState[key].Errors.Count; i++) {
                               Response.Write("Error " + i + ": " + ViewData.ModelState[key].Errors[i].ErrorMessage + 
                                   "; Exception = " + 
                                   (ViewData.ModelState[key].Errors[i].Exception == null 
                                        ? "N/A"
                                        : ViewData.ModelState[key].Errors[i].Exception.GetType().ToString() + (ViewData.ModelState[key].Errors[i].Exception.InnerException == null
                                            ? " (with no inner exception)"
                                            : ViewData.ModelState[key].Errors[i].Exception.InnerException.GetType().ToString())) + "<br />");
                           }
                    } %>
                </td>
            </tr>
        <% } %>
    </tbody>
</table>

<br />

<h3>Request.Form Elements</h3>
<p><%= Request.Form.ToString().Replace("&", "<br />")%></p>
