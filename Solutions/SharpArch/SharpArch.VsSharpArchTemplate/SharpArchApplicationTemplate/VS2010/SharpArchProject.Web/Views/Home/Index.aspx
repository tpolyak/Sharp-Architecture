<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="$solutionname$.Web.Controllers" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>S#arp Architecture Project</h2>
    <h3>
        What next?</h3>
    <p>
        Your S#arp Architecture 1.5 project is now setup and ready to go. This release is built on the RTM verion of ASP.NET MVC 2. The only tasks remaining
        are to:
        <ol>
            <li>
                <span style="font-weight:bold; font-style:italic">Create your database and set the connection string
                within $solutionname$.Web/NHibernate.config</span>
	        </li>
            <li>
                Optionally, modify the Fluent NHibernate preferences within $solutionname$.Data.NHibernateMaps.GetConventions()
                if you don't like the default settings.  There's lots of terrific info about Fluent NHibernate
                at <a href="http://wiki.fluentnhibernate.org/show/HomePage">http://wiki.fluentnhibernate.org/show/HomePage</a>.
	        </li>
            <li>
                Open $solutionname$.Tests.dll via NUnit and make sure all the tests are turning green.
	        </li>
            <li>
                Add your first entity with CRUD scaffolding via /Code Generation/CrudScaffolding/ScaffoldingGeneratorCommand.tt.
                (If you include a namespace, be sure to setup the appropriate view area within
                $solutionname$.Web.Controllers.RouteRegistrar.RegisterRoutesTo() after the scaffolding generator has completed.)
	        </li>
        </ol>
    </p>
    <p>
        If you need direction on what to do next, take a look at the documentation found in the
        /docs folder of the S#arp Architecture release package which provides a solid tutorial.
        You can also ask for assitance and guidance on the
        <a href="http://groups.google.com/group/sharp-architecture">S#arp Architecture discussion boards</a>.
    </p>
</asp:Content>
