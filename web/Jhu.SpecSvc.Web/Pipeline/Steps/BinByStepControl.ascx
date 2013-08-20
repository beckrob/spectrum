<%@ Control Language="C#" AutoEventWireup="true" Inherits="Jhu.SpecSvc.Web.Pipeline.Steps.BinByStepControl"
    CodeBehind="BinByStepControl.ascx.cs" %>
<table class="PipelineForm">
    <tr>
        <td class="PipelineFormLabel">
            Bin by:
        </td>
        <td class="PipelineFormField">
            <asp:RadioButtonList ID="Parameter" runat="server" AutoPostBack="True" RepeatDirection="Vertical">
                <asp:ListItem Selected="True" Value="Redshift">Redshift</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td class="PipelineFormLabel" style="text-align: right">
            z<span style="font-size: 8pt; vertical-align: sub"><em>min</em></span> =&nbsp;
        </td>
        <td class="PipelineFormField">
            <asp:TextBox ID="BinLimitMin" runat="server">0</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="PipelineFormLabel" style="text-align: right">
            z<span style="font-size: 8pt; vertical-align: sub"><em>max</em></span> =&nbsp;
        </td>
        <td class="PipelineFormField">
            <asp:TextBox ID="BinLimitMax" runat="server">0.1</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="PipelineFormLabel" style="text-align: right">
            Δz<span style="font-size: 8pt; vertical-align: sub"><em>bin</em></span> =&nbsp;
        </td>
        <td class="PipelineFormField">
            <asp:TextBox ID="Binsize" runat="server">0.01</asp:TextBox>
        </td>
    </tr>
</table>
