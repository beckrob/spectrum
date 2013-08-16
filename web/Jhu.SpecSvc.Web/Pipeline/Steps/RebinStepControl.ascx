<%@ Control Language="C#" AutoEventWireup="true" Inherits="processStepControls_RebinStep" Codebehind="RebinStepControl.ascx.cs" %>
<table class="Form">
    <tr>
        <td class="FormLabel" style="text-align: right">
            λ<span style="font-size: 8pt; vertical-align: sub"><em>min</em></span> =&nbsp;
        </td>
        <td class="FormField">
            <asp:TextBox ID="RebinLimitMin" runat="server" Width="96px">3000</asp:TextBox>
            Å
        </td>
    </tr>
    <tr>
        <td class="FormLabel" style="text-align: right">
            λ<span style="font-size: 8pt; vertical-align: sub"><em>max</em></span> =&nbsp;
        </td>
        <td class="FormField">
            <asp:TextBox ID="RebinLimitMax" runat="server" Width="96px">8000</asp:TextBox>
            Å
        </td>
    </tr>
    <tr>
        <td class="FormLabel" style="text-align: right">
            Δλ<span style="font-size: 8pt; vertical-align: sub"><em>bin</em></span> =&nbsp;
        </td>
        <td class="FormField">
            <asp:TextBox ID="RebinBinsize" runat="server" Width="96px">1</asp:TextBox>
            Å
        </td>
    </tr>
</table>
