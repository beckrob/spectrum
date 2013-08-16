<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Jhu.SpecSvc.Web.Pipeline.Default" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <jgwc:Form runat="server" SkinID="ProcessMenu" Text="What would you like to do with the spectra?">
        <FormTemplate>
            <p>
                Select from the following processing templates:
            </p>
            <asp:RadioButtonList runat="server" ID="Template">
                <asp:ListItem Value="fit">Fit continua and lines</asp:ListItem>
                <asp:ListItem Value="lick">Calculate Lick-indices</asp:ListItem>
                <asp:ListItem Value="pca">Calculate robust principal components</asp:ListItem>
                <asp:ListItem Value="composite">Calculate composites</asp:ListItem>
                <asp:ListItem Value="magnitudes">Calculate synthetic magnitudes</asp:ListItem>
                <asp:ListItem Value="color">Generate a color-color plot</asp:ListItem>
                <asp:ListItem Value="plot">Plot spectra</asp:ListItem>
                <asp:ListItem Value="myspectra">Save to MySpectra</asp:ListItem>
                <asp:ListItem Value="download">Download to my computer</asp:ListItem>
            </asp:RadioButtonList>
            <p>
                More options can be selected later.</p>
        </FormTemplate>
        <ButtonsTemplate>
            <p class="FormButtons">
                <jswc:wizardbuttons runat="server" okenabled="true" ResultsEnabled="true" FinishEnabled="true" oncommand="Button_Command" />
            </p>
        </ButtonsTemplate>
    </jgwc:Form>
</asp:Content>
