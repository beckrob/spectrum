<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.Master" AutoEventWireup="true"
    CodeBehind="Pipeline.aspx.cs" Inherits="Jhu.SpecSvc.Web.Pipeline.Pipeline" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="dock-top">
        <jgwc:Toolbar runat="server">
            <jgwc:ToolbarElement runat="server" Width="200px">
                Add pipeline step:<br />
                <asp:DropDownList ID="StepType" runat="server" CssClass="ToolbarControl" OnSelectedIndexChanged="StepType_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
            </jgwc:ToolbarElement>
            <jgwc:ToolbarButton runat="server" ID="ResetPipeline" Text="reset pipeline" OnClick="ResetPipeline_Click" />
            <jgwc:ToolbarButton runat="server" ID="LoadPipeline" Text="load pipeline" OnClick="LoadPipeline_Click" />
            <jgwc:ToolbarButton runat="server" ID="SavePipeline" Text="save pipeline" OnClick="SavePipeline_Click" />
            <jgwc:ToolbarButton runat="server" ID="ManagePipelines" Text="manage pipelines" OnClick="ManagePipelines_Click" />
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
                <asp:ListView runat="server" ID="PipelineStepList" OnItemCreated="PipelineStepList_ItemCreated"
                    OnItemCommand="PipelineStepList_ItemCommand">
                    <LayoutTemplate>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <table class="PipelineStep" cellpadding="0" cellspacing="0">
                            <tr>
                                <th>
                                    <asp:Label runat="server" ID="Title"></asp:Label>
                                    [
                                    <asp:LinkButton runat="server" ID="Active" Text="Disable" CommandName="ActivateStep"></asp:LinkButton>
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
                                        CommandName="RemoveStep"></asp:LinkButton>
                                    |
                                    <asp:LinkButton runat="server" ID="MoveUp" Text="move forward" CausesValidation="false"
                                        CommandName="MoveUpStep"></asp:LinkButton>
                                    |
                                    <asp:LinkButton runat="server" ID="MoveDown" Text="move backward" CausesValidation="false"
                                        CommandName="MoveDownStep"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <p>
                            No pipeline steps have been added so far. Select from the list and click on 'Add
                            Step'.</p>
                    </EmptyDataTemplate>
                </asp:ListView>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
