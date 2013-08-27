<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.Master" AutoEventWireup="true"
    CodeBehind="PipelineDetails.aspx.cs" Inherits="Jhu.SpecSvc.Web.Pipeline.PipelineDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container dock-scroll">
        <jgwc:Form ID="PipelineDetailsForm" runat="server" SkinID="PipelineDetails">
            <FormTemplate>
                <p>
                    Enter a name for this pipeline.
                </p>
                <table class="FormTable">
                    <tr>
                        <td class="FormLabel">
                            <asp:Label runat="server" ID="NameLabel">Name:</asp:Label>
                        </td>
                        <td class="FormField">
                            <asp:TextBox runat="server" ID="Name" CssClass="FormField" />
                        </td>
                    </tr>
                </table>
            </FormTemplate>
            <ButtonsTemplate>
                <p class="FormMessage">
                    <asp:RequiredFieldValidator ID="NameRequiredValidator" runat="server" ControlToValidate="Name"
                        Display="Dynamic" Text="A pipeline name is required." />
                </p>
                <p class="FormButtons">
                    <asp:Button runat="server" Text="OK" OnClick="Ok_Click" />
                    <asp:Button runat="server" Text="Cancel" CausesValidation="false" OnClick="Cancel_Click" />
                </p>
            </ButtonsTemplate>
        </jgwc:Form>
    </div>
</asp:Content>
