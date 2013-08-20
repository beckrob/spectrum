<%@ Control Language="C#" AutoEventWireup="true" Inherits="Jhu.SpecSvc.Web.Pipeline.Steps.RebinStepControl" Codebehind="RebinStepControl.ascx.cs" %>
<table class="PipelineForm">
    <tr>
        <td class="PipelineFormLabel" style="text-align: right">
            λ<span style="font-size: 8pt; vertical-align: sub"><em>min</em></span> =&nbsp;
        </td>
        <td class="PipelineFormField">
            <asp:TextBox ID="RebinLimitMin" runat="server">3000</asp:TextBox>
            Å
        </td>
    </tr>
    <tr>
        <td class="PipelineFormLabel" style="text-align: right">
            λ<span style="font-size: 8pt; vertical-align: sub"><em>max</em></span> =&nbsp;
        </td>
        <td class="PipelineFormField">
            <asp:TextBox ID="RebinLimitMax" runat="server">8000</asp:TextBox>
            Å
        </td>
    </tr>
    <tr>
        <td class="PipelineFormLabel" style="text-align: right">
            Δλ<span style="font-size: 8pt; vertical-align: sub"><em>bin</em></span> =&nbsp;
        </td>
        <td class="PipelineFormField">
            <asp:TextBox ID="RebinBinsize" runat="server">1</asp:TextBox>
            Å
        </td>
    </tr>
</table>
