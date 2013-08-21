<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.Master" AutoEventWireup="true"
    CodeBehind="Output.aspx.cs" Inherits="Jhu.SpecSvc.Web.Pipeline.Output" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container dock-scroll">
        <jgwc:Form ID="Form1" runat="server" SkinID="ProcessMenu" Text="What would you like to do with the spectra?">
            <FormTemplate>
                <p>
                    Select from the following processing templates:
                </p>
                <asp:RadioButtonList runat="server" ID="Template">
                    <asp:ListItem Value="screen">View results interactively</asp:ListItem>
                    <asp:ListItem Value="myspectrum">Save spectra to MySpectrum</asp:ListItem>
                    <asp:ListItem Value="download">Download output as an archive</asp:ListItem>
                </asp:RadioButtonList>
                <p>
                    More options can be selected later.</p>
            </FormTemplate>
            <ButtonsTemplate>
                <p class="FormButtons">
                    <jswc:WizardButtons runat="server" OkEnabled="true" ResultsEnabled="true"
                        FinishEnabled="true" OnCommand="Button_Command" />
                </p>
            </ButtonsTemplate>
        </jgwc:Form>
    </div>
</asp:Content>
