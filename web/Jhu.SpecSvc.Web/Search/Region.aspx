<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.master" AutoEventWireup="true"
    CodeBehind="Region.aspx.cs" Inherits="Jhu.SpecSvc.Web.Search.Region" %>

<%@ Register Src="Tabs.ascx" TagName="Tabs" TagPrefix="jswu" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container">
        <div class="dock-left">
            <jswu:Tabs ID="SearchTabs" runat="server" SelectedTab="RegionSearch" />
        </div>
        <div class="TabFrameVertical dock-fill">
            <jgwc:Form runat="server" ID="RedirectForm" SkinID="Search" Text="Region search">
                <FormTemplate>
                    <p>
                        Find spectra in an arbitrarily complex region.
                    </p>
                    <ul>
                        <li>you will be redirected to the JHU Footprint Service</li>
                        <li>use the footprint editor to define seach region</li>
                    </ul>
                </FormTemplate>
                <ButtonsTemplate>
                    <asp:Button ID="Redirect" runat="server" Text="Next >" OnClick="Redirect_Click" CssClass="FormButton" />
                </ButtonsTemplate>
            </jgwc:Form>
            <jgwc:Form runat="server" ID="RegionSearchForm" SkinID="Search" Text="Region search"
                Visible="false">
                <FormTemplate>
                    <p>
                        Find spectra in an arbitrarily complex region.
                    </p>
                    <ul>
                        <li>region successfully received from Footprint Service</li>
                        <li>select collection before executing search</li>
                    </ul>
                    <table class="FormTable">
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
