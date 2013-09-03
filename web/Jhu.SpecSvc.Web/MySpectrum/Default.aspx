<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Jhu.SpecSvc.Web.MySpectrum.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle" runat="server">
<div class="LayoutContent dock-fill dock-container dock-scroll">
        <jgwc:Form ID="FolderListForm" runat="server" SkinID="FolderList" Text="MySpectra">
            <FormTemplate>
                <p>
                    Your current MySpectrum service URLs are:
                </p>
                <table class="FormTable">
                    <tr>
                        <td class="FormLabel">Search:</td>
                        <td class="FormField"><asp:HyperLink runat="server" ID="SearchUrl" /></td>
                    </tr>
                    <tr>
                        <td class="FormLabel">Admin:</td>
                        <td class="FormField"><asp:HyperLink runat="server" ID="AdminUrl" /></td>
                    </tr>
                    <tr>
                        <td class="FormLabel">Graph:</td>
                        <td class="FormField"><asp:HyperLink runat="server" ID="GraphUrl" /></td>
                    </tr>
                    <tr>
                        <td class="FormLabel"></td>
                        <td class="FormField" style="text-align:right"><asp:Button runat="server" ID="ChangeUrl" Text="Change URLs" CssClass="FormButton" OnClick="ChangeUrl_Click" CausesValidation="false" /></td>
                    </tr>
                </table>
                <p>
                    Select a MySpectrum folder from the list below
                </p>
                <table class="FormTable">
                    <tr>
                        <td class="FormLabel">
                            <asp:Label runat="server" ID="NameLabel">Folder:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormList">
                            <jgwc:MultiSelectGridView runat="server" ID="FolderList" DataKeyNames="ID" AutoGenerateColumns="false">
                                <Columns>
                                    <jgwc:SelectionField />
                                    <asp:BoundField DataField="ID" HeaderText="ID" />
                                    <asp:BoundField DataField="Name" HeaderText="Folder name" />
                                    <asp:BoundField DataField="Count" HeaderText="# of spectra" />
                                </Columns>
                            </jgwc:MultiSelectGridView>
                        </td>
                    </tr>
                </table>
            </FormTemplate>
            <ButtonsTemplate>
                <p class="FormMessage">
                    <asp:CustomValidator runat="server" ID="FolderListSelectedValidator" OnServerValidate="FolderListSelectedValidator_ServerValidate"
                        Display="Dynamic" Text="No folder selected" />
                </p>
                <p class="FormButtons">
                    <asp:Button runat="server" ID="Create" Text="Create" OnClick="Create_Click" CausesValidation="false" />
                    <asp:Button runat="server" ID="Rename" Text="Rename" OnClick="Rename_Click" />
                    <asp:Button runat="server" ID="Delete" Text="Delete" OnClick="Delete_Click" />
                    <asp:Button runat="server" ID="List" Text="List spectra" OnClick="List_Click" />
                </p>
            </ButtonsTemplate>
        </jgwc:Form>
    </div>
</asp:Content>
