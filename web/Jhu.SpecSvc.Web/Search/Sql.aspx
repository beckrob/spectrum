<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.master" AutoEventWireup="true"
    CodeBehind="Sql.aspx.cs" Inherits="Jhu.SpecSvc.Web.Search.Sql" %>

<%@ Register Src="Tabs.ascx" TagName="Tabs" TagPrefix="jswu" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../CodeMirror/lib/codemirror.css">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container">
        <div class="dock-left">
            <jswu:Tabs ID="SearchTabs" runat="server" SelectedTab="SqlSearch" />
        </div>
        <div class="TabFrameVertical dock-fill">
            <jgwc:Form runat="server" ID="SqlSearchForm" SkinID="Search" Text="SQL search">
                <FormTemplate>
                    <p>
                        Find spectra using a SQL query.
                    </p>
                    <ul>
                        <li>the query must return all columns from the 'Spectra' table</li>
                        <li>refer to the documentation for details on the database schema</li>
                    </ul>
                    <table class="FormTable">
                        <tr>
                            <td class="FormLabel">
                                <asp:Label runat="server">Query:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormList">
                                <jgwc:CodeMirror runat="server" ID="Query" Mode="text/x-sql" Theme="default" Text="SELECT TOP 100 * FROM Spectra" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label runat="server" ID="CollectionLabel">Collections:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormList">
                                <jswc:CollectionList runat="server" ID="Collections" />
                            </td>
                        </tr>
                    </table>
                </FormTemplate>
                <ButtonsTemplate>
                    <asp:Button ID="OK" runat="server" Text="Run search" OnClick="Ok_Click" CssClass="FormButton" />
                </ButtonsTemplate>
            </jgwc:Form>
        </div>
    </div>
</asp:Content>
