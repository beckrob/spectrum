<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.master" AutoEventWireup="true"
    CodeBehind="Object.aspx.cs" Inherits="Jhu.SpecSvc.Web.Search.Object" %>

<%@ Register Src="Tabs.ascx" TagName="Tabs" TagPrefix="jswu" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container">
        <div class="dock-left">
            <jswu:Tabs ID="SearchTabs" runat="server" SelectedTab="ObjectSearch" />
        </div>
        <div class="TabFrameVertical dock-fill">
            <jgwc:Form runat="server" ID="ConeSearchForm" SkinID="Search" Text="Object list search">
                <FormTemplate>
                    <p>
                        Retrieve spectra by coordinates.
                    </p>
                    <ul>
                        <li>enter one coordinate pair per line,</li>
                        <li>optionally, use your own object IDs, enter them in the first column</li>
                        <li>search radius is limited to 25 arcsec</li>
                    </ul>
                    <table class="FormTable">
                        <tr>
                            <td class="FormLabel" colspan="2">
                                <asp:Label runat="server" ID="CoordinatesLabel">ID list:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormField" colspan="2">
                                <asp:TextBox ID="ObjectList" runat="server" Wrap="False" Font-Names="Courier New"
                                    Rows="8" TextMode="MultiLine" CssClass="FormField" Style="width: 100%; overflow: auto">256.091095,  62.794872
195.658020,  -0.978863 
197.731613,  -1.456814 
  1.175780,   0.006516 
350.606628,   0.414013 </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                            </td>
                            <td class="FormField">
                                <asp:CheckBox ID="Ids" runat="server" Text="First column contains IDs" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="SrLabel" runat="server">Search radius:</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="Sr" runat="server" CssClass="FormFieldNarrow">5</asp:TextBox>&nbsp;arcsec
                                <asp:RegularExpressionValidator ID="SrFormatValidator" runat="server" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ErrorMessage="invalid format" Display="Dynamic" ControlToValidate="Sr"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="SrRequiredValidator" runat="server" ErrorMessage="required"
                                    Display="Dynamic" ControlToValidate="Sr"></asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="SrRangeValidator" runat="server" ErrorMessage="invalid range"
                                    Display="Dynamic" ControlToValidate="Sr" Type="Double" MinimumValue="0" MaximumValue="25"></asp:RangeValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel" colspan="2">
                                <asp:Label runat="server" ID="CollectionLabel">Collections:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormList" colspan="2">
                                <jswc:CollectionList runat="server" ID="Collections" SearchMethod="Object" />
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
