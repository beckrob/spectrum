<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.Master" AutoEventWireup="true"
    CodeBehind="Pipeline.aspx.cs" Inherits="Jhu.SpecSvc.Web.Pipeline.Pipeline" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="dock-top">
        <jgwc:Toolbar runat="server">
            <jgwc:ToolbarButton runat="server" ID="ResetPipeline" Text="reset pipeline" />
            <jgwc:ToolbarButton runat="server" ID="LoadPipeline" Text="load pipeline" />
            <jgwc:ToolbarButton runat="server" ID="SavePipeline" Text="save pipeline" />
            <jgwc:ToolbarButton runat="server" ID="ManagePipelines" Text="manage pipelines" />
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
    <div class="LayoutContent dock-fill dock-container">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <table class="PipelineTable" cellpadding="0" cellspacing="0">
                <asp:ListView runat="server" ID="PipelineStepList" OnItemCreated="PipelineStepList_ItemCreated"
                    OnItemCommand="PipelineStepList_ItemCommand">
                    <LayoutTemplate>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="PipelineStepHeader">
                                <asp:Label runat="server" ID="Title"></asp:Label>
                                [
                                <asp:LinkButton runat="server" ID="Active" Text="Disable" CommandName="ActivateStep"></asp:LinkButton>
                                ]
                            </td>
                            <td class="PipelineStepHeader" style="text-align: right">
                                <asp:LinkButton runat="server" ID="Remove" Text="remove" CommandName="RemoveStep"></asp:LinkButton>
                                |
                                <asp:LinkButton runat="server" ID="MoveUp" Text="move up" CommandName="MoveUpStep"></asp:LinkButton>
                                |
                                <asp:LinkButton runat="server" ID="MoveDown" Text="move down" CommandName="MoveDownStep"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td class="PipelineStepForm" colspan="2">
                                <asp:PlaceHolder runat="server" ID="controlPlaceholder"></asp:PlaceHolder>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                    <p>No pipeline steps have been added so far. Select from the list and click on 'Add Step'.</p>
                    </EmptyDataTemplate>
                </asp:ListView>
                <tr>
                    <td colspan="2">
                        <asp:DropDownList ID="StepType" runat="server">
                        </asp:DropDownList>
                        <asp:Button runat="server" ID="AddStep" Text="Add Step" OnClick="AddStep_OnClick" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
</asp:Content>
