<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.master" AutoEventWireup="true"
    CodeBehind="All.aspx.cs" Inherits="Jhu.SpecSvc.Web.Search.All" %>

<%@ Register Src="Tabs.ascx" TagName="Tabs" TagPrefix="jswu" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container">
        <div class="dock-left">
            <jswu:Tabs ID="SearchTabs" runat="server" SelectedTab="AllSearch" />
        </div>
        <div class="TabFrameVertical dock-fill">
            <jgwc:Form runat="server" ID="ConeSearchForm" SkinID="Search" Text="Get whole collection">
                <FormTemplate>
                    <p>
                        Retrieve all spectra from a collection.
                    </p>
                    <ul>
                        <li>limited to smaller collections only</li>
                    </ul>
                    <table class="FormTable">
                        <tr>
                            <td class="FormLabel" colspan="2">
                                <asp:Label runat="server" ID="CollectionLabel">Collections:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormList" colspan="2">
                                <jswc:CollectionList runat="server" ID="Collections" SearchMethod="All" />
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
