<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.master" AutoEventWireup="true"
    CodeBehind="SkyServer.aspx.cs" Inherits="Jhu.SpecSvc.Web.Search.SkyServer" %>

<%@ Register Src="Tabs.ascx" TagName="Tabs" TagPrefix="jswu" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../CodeMirror/lib/codemirror.css">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container">
        <div class="dock-left">
            <jswu:Tabs ID="SearchTabs" runat="server" SelectedTab="SkyServerSearch" />
        </div>
        <div class="TabFrameVertical dock-fill">
            <jgwc:Form runat="server" ID="SkyServerSearchForm" SkinID="Search" Text="SkyServer search">
                <FormTemplate>
                    <p>
                        Find spectra using a SkyServer SQL query.
                    </p>
                    <ul>
                        <li>the query must return the specObjID column only</li>
                        <li>refer to the documentation and the SkyServer web site for details on the database
                            schema</li>
                    </ul>
                    <table class="FormTable">
                        <tr>
                            <td class="FormLabel" colspan="2">
                                <asp:Label runat="server">Query:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormList" colspan="2">
                                <jgwc:CodeMirror runat="server" ID="Query" Mode="text/x-sql" Theme="default" Text="SELECT TOP 10 specObjId FROM SpecObj" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel" valign="top">
                                Target:
                            </td>
                            <td class="FormField">
                                <asp:DropDownList ID="Target" runat="server">
                                    <asp:ListItem Selected="True">DR7</asp:ListItem>
                                    <asp:ListItem>DR6</asp:ListItem>
                                    <asp:ListItem>DR5</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel" colspan="2">
                                <asp:Label runat="server" ID="CollectionLabel">Collections:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormList" colspan="2">
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
