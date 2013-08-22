<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.master" AutoEventWireup="true"
    CodeBehind="ID.aspx.cs" Inherits="Jhu.SpecSvc.Web.Search.ID" %>

<%@ Register Src="Tabs.ascx" TagName="Tabs" TagPrefix="jswu" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container">
        <div class="dock-left">
            <jswu:Tabs ID="SearchTabs" runat="server" SelectedTab="ConeSearch" />
        </div>
        <div class="TabFrameVertical dock-fill">
            <jgwc:Form runat="server" ID="ConeSearchForm" SkinID="Search" Text="ID search">
                <FormTemplate>
                    <p>
                        Retrieve spectra by IVOA Publisher ID.
                    </p>
                    <ul>
                        <li>enter one ID per line</li>
                    </ul>
                    <table class="FormTable">
                        <tr>
                            <td class="FormLabel">
                                <asp:Label runat="server" ID="CoordinatesLabel">ID list:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormField">
                                <asp:TextBox ID="ObjectList" runat="server" Wrap="False" Font-Names="Courier New"
                                    Rows="15" TextMode="MultiLine" CssClass="FormField" style="width:100%; overflow: auto">ivo://jhu/sdss/dr6/spec/2.5#80443408262365184
ivo://jhu/sdss/dr6/spec/2.5#80725178694238208
ivo://jhu/sdss/dr6/spec/2.5#80443408237199360
ivo://jhu/sdss/dr6/spec/2.5#80443408216227840
ivo://jhu/sdss/dr6/spec/2.5#80443408224616448</asp:TextBox>
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
