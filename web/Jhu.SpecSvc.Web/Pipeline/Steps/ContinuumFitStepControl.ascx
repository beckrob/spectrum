<%@ Control Language="C#" AutoEventWireup="true" Inherits="Jhu.SpecSvc.Web.Pipeline.Steps.ContinuumFitStepControl"
    CodeBehind="ContinuumFitStepControl.ascx.cs" %>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td style="vertical-align: top">
            <table class="PipelineForm">
                <tr>
                    <td class="PipelineFormLabel">
                        Method:
                    </td>
                    <td class="PipelineFormField">
                        <asp:RadioButtonList ID="Method" runat="server" RepeatDirection="Vertical">
                            <asp:ListItem Selected="True" Value="NonNegativeLeastSquares">Non-negative least squares</asp:ListItem>
                            <asp:ListItem Value="LeastSquares">Least squares</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="PipelineFormLabel">
                        &nbsp;
                    </td>
                    <td class="PipelineFormField">
                        <asp:CheckBox ID="WeightWithError" runat="server" Text="Weight with error" />
                    </td>
                </tr>
                <tr>
                    <td class="PipelineFormLabel">
                        Masking:
                    </td>
                    <td class="PipelineFormField">
                        <asp:CheckBox ID="MaskLines" runat="server" Text="Mask strong emission lines" />
                    </td>
                </tr>
                <tr>
                    <td class="PipelineFormLabel">
                        &nbsp;
                    </td>
                    <td class="PipelineFormField">
                        <asp:CheckBox ID="MaskSkyLines" runat="server" Text="Mask sky lines" />
                    </td>
                </tr>
                <tr>
                    <td class="PipelineFormLabel">
                        &nbsp;
                    </td>
                    <td class="PipelineFormField">
                        <asp:CheckBox ID="MaskFromSpectra" runat="server" Text="Use mask from spectrum" />
                    </td>
                </tr>
            </table>
        </td>
        <td style="width: 18px">
        </td>
        <td style="vertical-align: top">
            <table class="PipelineForm">
                <tr>
                    <td class="PipelineFormLabel">
                        Template set:
                    </td>
                    <td class="PipelineFormField">
                        <asp:DropDownList ID="TemplateSet" runat="server" AutoPostBack="True" CssClass="FormField"
                            OnSelectedIndexChanged="TemplateSet_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="PipelineFormLabel">
                        &nbsp;
                    </td>
                    <td class="PipelineFormField">
                        <div style="width: 100%; overflow: auto; max-height: 120px; border: solid 1px #C0C0C0">
                            <asp:CheckBoxList ID="Templates" runat="server">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
