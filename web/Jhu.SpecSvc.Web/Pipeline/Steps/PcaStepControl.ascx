<%@ Control Language="C#" AutoEventWireup="true" Inherits="processStepControls_PcaStep" Codebehind="PcaStepControl.ascx.cs" %>
<table class="Form">
    <tr>
        <td class="FormLabel">
            Components:
        </td>
        <td class="FormField" style="width: 30%">
            <asp:TextBox ID="Components" runat="server" Width="96px">10</asp:TextBox>
        </td>
        <td class="FormLabel" style="width: 20%">
            Iterations:
        </td>
        <td class="FormField" style="width: 30%">
            <asp:TextBox ID="Iterations" runat="server" Width="96px">3</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="FormLabel">
            &nbsp;
        </td>
        <td class="FormField" colspan="3">
            <asp:CheckBox runat="server" ID="Gappy" Text="Gappy PCA" />
        </td>
    </tr>
</table>
