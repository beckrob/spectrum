<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Jhu.SpecSvc.Web.Pipeline.Default" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container dock-scroll">
        <jgwc:Form runat="server" SkinID="PipelineForm" Text="What would you like to do with the spectra?">
            <FormTemplate>
                <p>
                    Create or load a spectrum processing pipeline.
                </p>
                <ul>
                    <li>use current pipeline (if already defined)</li>
                    <li>load a pipeline saved earlier (requires registration)</li>
                    <li>create a pipeline based on a predefined template</li>
                </ul>
                <asp:RadioButtonList runat="server" ID="PipelineMode" AutoPostBack="true">
                    <asp:ListItem Value="session" Selected="True">Use existing pipeline</asp:ListItem>
                    <asp:ListItem Value="template">Use a pipeline template</asp:ListItem>
                    <asp:ListItem Value="load">Load existing pipeline</asp:ListItem>
                </asp:RadioButtonList>
                <div runat="server" id="PipelineTemplatesDiv">
                    <p>
                        Select a template from the list below:</p>
                    <asp:RadioButtonList runat="server" ID="PipelineTemplates">
                        <asp:ListItem Value="fit" Selected="True">Fit continua and lines</asp:ListItem>
                        <asp:ListItem Value="lick">Calculate Lick-indices</asp:ListItem>
                        <asp:ListItem Value="pca">Calculate robust principal components</asp:ListItem>
                        <asp:ListItem Value="composite">Calculate composites</asp:ListItem>
                        <asp:ListItem Value="magnitudes">Calculate synthetic magnitudes</asp:ListItem>
                        <asp:ListItem Value="color">Generate a color-color plot</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </FormTemplate>
            <ButtonsTemplate>
                <p class="FormButtons">
                    <jswc:WizardButtons runat="server" OkEnabled="true" ResultsEnabled="true" FinishEnabled="true"
                        OnCommand="Button_Command" />
                </p>
            </ButtonsTemplate>
        </jgwc:Form>
    </div>
</asp:Content>
