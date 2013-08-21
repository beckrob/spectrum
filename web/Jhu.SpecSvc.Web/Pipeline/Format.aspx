<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.Master" AutoEventWireup="true"
    CodeBehind="Format.aspx.cs" Inherits="Jhu.SpecSvc.Web.Pipeline.Format" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="dock-top">
        <jgwc:Toolbar runat="server">
            <jgwc:ToolbarElement runat="server" Width="200px">
                Add file format:<br />
                <asp:DropDownList ID="FormatType" runat="server" CssClass="ToolbarControl" OnSelectedIndexChanged="FormatType_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
            </jgwc:ToolbarElement>
            <jgwc:ToolbarButton runat="server" ID="ResetFormats" Text="reset formats" />
            <%--<jgwc:ToolbarButton runat="server" ID="LoadPipeline" Text="load pipeline" />
            <jgwc:ToolbarButton runat="server" ID="SavePipeline" Text="save pipeline" />
            <jgwc:ToolbarButton runat="server" ID="ManagePipelines" Text="manage pipelines" />--%>
            <jgwc:ToolbarElement runat="server">
            </jgwc:ToolbarElement>
        </jgwc:Toolbar>
    </div>
    <div class="dock-bottom">
        <p class="FormMessage">
            &nbsp;</p>
        <p class="FormButtons">
            <jswc:WizardButtons runat="server" OkEnabled="true" OnCommand="Button_Command" />
        </p>
    </div>
    <div class="LayoutContent dock-fill dock-container dock-scroll">
        <asp:UpdatePanel runat="server" RenderMode="Inline">
            <ContentTemplate>
                <asp:ListView runat="server" ID="FormatList" OnItemCreated="FormatList_ItemCreated"
                    OnItemCommand="FormatList_ItemCommand">
                    <LayoutTemplate>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <table class="PipelineStep" cellpadding="0" cellspacing="0">
                            <tr>
                                <th>
                                    <asp:Label runat="server" ID="Title"></asp:Label>
                                    [
                                    <asp:LinkButton runat="server" ID="Active" Text="Disable" CommandName="ActivateFormat"></asp:LinkButton>
                                    ]
                                </th>
                            </tr>
                            <tr>
                                <td class="PipelineStepForm">
                                    <asp:PlaceHolder runat="server" ID="controlPlaceholder" />
                                </td>
                            </tr>
                            <tr>
                                <td class="PipelineStepFooter">
                                    <asp:LinkButton runat="server" ID="Remove" Text="remove" CausesValidation="false"
                                        CommandName="RemoveFormat"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <p>
                            No output formats have been added so far. Select from the list.</p>
                    </EmptyDataTemplate>
                </asp:ListView>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
