<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Northwind.Web.Views.Home.Index" %>
<%@ Import Namespace="Northwind.Controllers" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Northwind MVC Example Pages</h2>
    <h3>Categories</h3>
    <p>
        Three pages have been included for viewing: list, detail and create.
        <ul>
            <li>
                <%= Html.ActionLink((CategoriesController c) => c.List(), "List Categories") %>: 
                Using a master page, this page simply lists out all the categories in the database.  
                This uses an "out of the box" generic DAO to retrieve the categories from the database.  
                To view the category details, click a category name on the listing page.
            </li>
            <li>
                Create Category:  To create a category, use the following URL, replacing the 
                category name paramater's value with anything you'd like:  
                <%= Html.BuildUrlFromExpression<CategoriesController>(c => c.Create("Your_Category_Name")) %>
            </li>
        </ul>
    </p>
    <h3>Customers</h3>
    <p>
        Likewise for cutomer data, three pages have been included for viewing: list, detail and create.
        <ul>
            <li>
                <%= Html.ActionLink((CustomersController c) => c.List(), "List Customers") %>: 
                Not using a master page, this page lists the customers in the database who are from Venezuela.  
                Since this uses a custom data-access method, this demonstrates extending the 
                generic DAO with a custom retrieval method.  As it lists the number of orders that each 
                customer has placed, it's also an example of lazy-loaded collections.
            </li>
            <li>
                Create Customer:  To create a customer, use the following URL, replacing the 
                company name and customer's assigned ID with anything you'd like; 
                but note that the assigned ID must be exactly 5 characters:  
                <%= Html.BuildUrlFromExpression<CustomersController>(c => c.Create("Some_Company_Name", "A_5_Character_Unique_ID")) %>.  
                This is an example of creating an object with an assigned ID.  Using <i>assigned IDs should typically be 
                considered a bad practice</i> and should be avoided unless you're having to 
                integrate with a legacy application.
            </li>
        </ul>
    </p>
    <h3>Unit Tests</h3>
    <p>
        There are many more examples of functionality, including data access unit tests, found in the 
        Northwind.Tests project.  I encourage you to take a look.
    </p>
</asp:Content>
