<%@ Page Title="" Language="C#" MasterPageFile="~/UI.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Jhu.SpecSvc.Web.Search.Default" %>

<%@ Register Src="Tabs.ascx" TagName="Tabs" TagPrefix="jswu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="toolbar" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middle" runat="server">
    <div class="dock-left">
        <jswu:Tabs ID="SearchTabs" runat="server" SelectedTab="FreeSearch" />
    </div>
    <div class="TabFrameVertical dock-fill">
        <jgwc:Form runat="server" ID="DefaultSearchForm" SkinID="Search" Text="Free search">
            <FormTemplate>
                <p>
                    To find spectra within a circular region around an object.
                </p>
                <ul>
                    <li>enter object name or J2000 equatorial coordinates</li>
                    <li>use decimal or sexagesimal format</li>
                    <li>NED and Simbad will be used to resolve object names</li>
                    <li>search radius is 10 arcsec</li>
                </ul>
                <table class="FormTable">
                    <tr>
                        <td class="FormLabel">
                            <asp:Label runat="server" ID="QueryLabel">Query:</asp:Label>
                        </td>
                        <td class="FormField">
                            <asp:TextBox runat="server" ID="Query" CssClass="FormField" Text="M101" />
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                            <asp:Label runat="server" ID="RadiusLabel">Search radius:</asp:Label>
                        </td>
                        <td class="FormField">
                            <asp:TextBox runat="server" ID="Radius" CssClass="FormField" Text="10" />
                            arcmin
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel" colspan="2">
                            <asp:Label runat="server" ID="CollectionLabel">Collections:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormList" colspan="2">
                            <jsw:CollectionList runat="server" ID="Collections" />
                        </td>
                    </tr>
                </table>
            </FormTemplate>
            <ButtonsTemplate>
                <asp:Button ID="OK" runat="server" Text="Run search" OnClick="Ok_Click" CssClass="FormButton" />
            </ButtonsTemplate>
        </jgwc:Form>
    </div>
</asp:Content>
