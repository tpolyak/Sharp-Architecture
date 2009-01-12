<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="$safeprojectname$.Views.Home.Index" %>
<%@ Import Namespace="$solutionname$.Web.Controllers" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>S#arp Architecture Project</h2>
    <h3>
        What next?</h3>
    <p>
        Your S#arp Architecture project is now setup and ready to go.  The only task remaining 
        is to <span style="font-weight:bold; font-style:italic">now create your database and set the 
	connection string within Hibernate.cfg.xml</span>.
    </p>
    <p>
        If you need direction on what to do next, take a look at the documentation found in the 
        /docs folder of the S#arp Architecture release package which provides a solid tutorial.  
        You can also ask for assitance and guidance on the 
        <a href="http://groups.google.com/group/sharp-architecture">S#arp Architecture discussion boards</a>.
    </p>
</asp:Content>
