<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Northwind.Web.Views.Categories.List" %>
<%@ Import Namespace="Northwind.Core" %>
<%@ Import Namespace="Northwind.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <form id="form1" runat="server">
    <div>
        <p>Categories:</p>
        <asp:ListView ID="categoryList" runat="server">
            <LayoutTemplate>
                <ul>
                    <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li>
                    <a href="<%# Html.BuildUrlFromExpression<CategoriesController>(c => c.Detail(((Category) Container.DataItem).ID)) %>">
                        <%# ((Category) Container.DataItem).Name %>
                    </a>
                </li>
            </ItemTemplate>
        </asp:ListView>
    </div>
    </form>
</asp:Content>
