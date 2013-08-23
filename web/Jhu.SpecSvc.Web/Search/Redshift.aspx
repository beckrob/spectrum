<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.master" AutoEventWireup="true"
    CodeBehind="Redshift.aspx.cs" Inherits="Jhu.SpecSvc.Web.Search.Redshift" %>

<%@ Register Src="Tabs.ascx" TagName="Tabs" TagPrefix="jswu" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container">
        <div class="dock-left">
            <jswu:Tabs ID="SearchTabs" runat="server" SelectedTab="RedshiftSearch" />
        </div>
        <div class="TabFrameVertical dock-fill">
            <jgwc:Form runat="server" ID="ConeSearchForm" SkinID="Search" Text="Redshift search">
                <FormTemplate>
                    <p>
                        To find spectra in a redshift range
                    </p>
                    <ul>
                        <li>enter the redshift range, and</li>
                        <li>select the catalogs.</li>
                    </ul>
                    <table class="FormTable">
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="RedshiftTitle" runat="server">Redshift between:</asp:Label>
                            </td>
                            <td class="FormField">
                                <p>
                                    <asp:TextBox ID="RedshiftFrom" runat="server" CssClass="FormFieldNarrow">0.01</asp:TextBox>&nbsp;and
                                    <asp:TextBox ID="RedshiftTo" runat="server" CssClass="FormFieldNarrow">0.011</asp:TextBox><br>
                                    <asp:RegularExpressionValidator ID="RedshiftFromFormatValidator" runat="server" ControlToValidate="RedshiftFrom"
                                        Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"></asp:RegularExpressionValidator>&nbsp;
                                    <asp:RegularExpressionValidator ID="RedshiftToFormatValidator" runat="server" ControlToValidate="RedshiftTo"
                                        Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"></asp:RegularExpressionValidator>&nbsp;
                                    <asp:RequiredFieldValidator ID="RedshiftFromRequiredValidator" runat="server" ControlToValidate="RedshiftFrom"
                                        ErrorMessage="required"></asp:RequiredFieldValidator>&nbsp;
                                    <asp:RequiredFieldValidator ID="RedshiftToRequiredValidator" runat="server" ControlToValidate="RedshiftTo"
                                        ErrorMessage="required"></asp:RequiredFieldValidator></p>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel" colspan="2">
                                <asp:Label runat="server" ID="CollectionLabel">Collections:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormList" colspan="2">
                                <jswc:CollectionList runat="server" ID="Collections" SearchMethod="Redshift" />
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
