<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.Master" AutoEventWireup="true"
    CodeBehind="Folder.aspx.cs" Inherits="Jhu.SpecSvc.Web.MySpectrum.Folder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container dock-scroll">
        <jgwc:Form ID="FolderForm" runat="server" SkinID="FolderForm">
            <FormTemplate>
                <p>
                    Enter the folder name.
                </p>
                <table class="FormTable">
                    <tr>
                        <td class="FormLabel">
                            Name:
                        </td>
                        <td class="FormField">
                            <asp:TextBox runat="server" ID="Name" />
                        </td>
                    </tr>
                </table>
            </FormTemplate>
            <ButtonsTemplate>
                <p class="FormMessage">
                </p>
                <p class="FormButtons">
                    <asp:Button runat="server" ID="Ok" Text="OK" OnClick="Ok_Click" />
                    <asp:Button runat="server" ID="Cancel" Text="Cancel" OnClick="Cancel_Click" CausesValidation="false" />
                </p>
            </ButtonsTemplate>
        </jgwc:Form>
    </div>
</asp:Content>
