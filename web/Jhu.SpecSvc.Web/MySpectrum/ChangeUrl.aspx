<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.Master" AutoEventWireup="true"
    CodeBehind="ChangeUrl.aspx.cs" Inherits="Jhu.SpecSvc.Web.MySpectrum.ChangeUrl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container dock-scroll">
        <jgwc:Form ID="ChangeUrlForm" runat="server" SkinID="ChangeUrl" Text="Change MySpectrum URLs">
            <FormTemplate>
                <p>
                    Enter MySpectrum service URLs below.
                </p>
                <table class="FormTable">
                    <tr>
                        <td class="FormLabel">
                            Search:
                        </td>
                        <td class="FormField">
                            <asp:TextBox runat="server" ID="SearchUrl" />
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                            Admin:
                        </td>
                        <td class="FormField">
                            <asp:TextBox runat="server" ID="AdminUrl" />
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                            Graph:
                        </td>
                        <td class="FormField">
                            <asp:TextBox runat="server" ID="GraphUrl" />
                        </td>
                    </tr>
                </table>
            </FormTemplate>
            <ButtonsTemplate>
                <p class="FormMessage"></p>
                <p class="FormButtons">
                    <asp:Button runat="server" ID="Ok" Text="OK" OnClick="Ok_Click" />
                    <asp:Button runat="server" ID="Cancel" Text="Cancel" OnClick="Cancel_Click" CausesValidation="false" />
                </p>
            </ButtonsTemplate>
        </jgwc:Form>
    </div>
</asp:Content>
