<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Jhu.SpecSvc.Web.Pipeline.Steps.NormalizeStepControl" Codebehind="NormalizeStepControl.ascx.cs" %>
<table class="PipelineForm">
    <tr>
        <td class="PipelineFormLabel"></td>
        <td class="PipelineFormField">
            <asp:RadioButtonList ID="Method" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Method_SelectedIndexChanged"
                RepeatDirection="Vertical">
                <asp:ListItem Selected="True" Value="FluxAtWavelength">Flux @ λ<span style="font-size: 8pt; vertical-align: sub">0</span></asp:ListItem>
                <asp:ListItem Value="FluxMedianInRanges">Median flux in ranges</asp:ListItem>
                <asp:ListItem Value="FluxIntegralInRanges">Integral flux in ranges</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr runat="server" id="WavelengthRow">
        <td class="PipelineFormLabel" style="text-align:right">
            λ<span style="font-size: 8pt; vertical-align: sub">0</span> =&nbsp;
        </td>
        <td class="PipelineFormField">
            <asp:TextBox ID="Wavelength" runat="server">0</asp:TextBox> Å
            <asp:RangeValidator ID="WavelengthRangeValidator" runat="server" ControlToValidate="Wavelength"
                Display="Dynamic" ErrorMessage="Invalid value" MaximumValue="100000" MinimumValue="0"
                SetFocusOnError="True" Type="Double"></asp:RangeValidator>
        </td>
        </tr>
    <tr runat="server" id="FluxRow">
        <td class="PipelineFormLabel" style="text-align:right">
            f<span style="font-size: 8pt; vertical-align: sub">0</span> =&nbsp;
        </td>
        <td class="PipelineFormField">
            <asp:TextBox ID="Flux" runat="server">0</asp:TextBox> ADU
        </td>
    </tr>
        <tr runat="server" ID="TemplateRow">
        <td class="PipelineFormLabel">
            Template:
        </td>
        <td class="PipelineFormField">
            <asp:RadioButtonList ID="Template" runat="server" AutoPostBack="True"
                RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="Galaxy">Galaxy</asp:ListItem>
                <asp:ListItem Value="Qso">QSO</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
</table>
