<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="processStepControls_MagnitudeStep" Codebehind="FluxStepControl.ascx.cs" %>
<table class="Form">
    <tr>
        <td class="FormLabel">
            Filters:
        </td>
        <td class="FormField">
            <div style="width: 100%; overflow: auto; max-height: 120px; border: solid 1px #C0C0C0">
                <asp:CheckBoxList ID="Filters" runat="server">
                </asp:CheckBoxList>
            </div>
        </td>
    </tr>
    <tr>
        <td class="FormLabel">
            Redshift:
        </td>
        <td class="FormField">
            <asp:RadioButtonList ID="Redshift" runat="server" RepeatDirection="Horizontal" 
                AutoPostBack="True" onselectedindexchanged="Redshift_SelectedIndexChanged">
                <asp:ListItem Value="AsIs">As is</asp:ListItem>
                <asp:ListItem>Variable</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
</table>
<asp:Table runat="server" ID="RedshiftTable" CssClass="Form" Visible="false">
    <asp:TableRow runat="server" ID="RedshiftRow">
        <asp:TableCell ID="TableCell3" runat="server" CssClass="FormLabelRight">
            z<span style="font-size: 8pt; vertical-align: sub"><em>min</em></span> =
        </asp:TableCell>
        <asp:TableCell ID="TableCell4" runat="server" CssClass="FormField" Width="20%">
            <asp:TextBox ID="RedshiftMin" runat="server" Width="96px">0</asp:TextBox>
            <asp:RangeValidator ID="RedshiftMinRangeValidator" runat="server" ControlToValidate="RedshiftMin"
                Display="Dynamic" ErrorMessage="Invalid value" MaximumValue="10" MinimumValue="0"
                SetFocusOnError="True" Type="Double"></asp:RangeValidator>
        </asp:TableCell>
        <asp:TableCell ID="TableCell5" runat="server" CssClass="FormLabelRight" Width="20%">
            z<span style="font-size: 8pt; vertical-align: sub"><em>max</em></span> =
        </asp:TableCell>
        <asp:TableCell ID="TableCell6" runat="server" CssClass="FormField" Width="30%">
            <asp:TextBox ID="RedshiftMax" runat="server" Width="96px">0</asp:TextBox>
            <asp:RangeValidator ID="RedshiftMaxRangeValidator" runat="server" ControlToValidate="RedshiftMax"
                Display="Dynamic" ErrorMessage="Invalid value" MaximumValue="10" MinimumValue="0"
                SetFocusOnError="True" Type="Double"></asp:RangeValidator>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server" ID="RedshiftBinRow">
        <asp:TableCell ID="TableCell7" runat="server" CssClass="FormLabelRight">
            Δz<span style="font-size: 8pt; vertical-align: sub"><em>bin</em></span> =
        </asp:TableCell>
        <asp:TableCell ID="TableCell8" runat="server" CssClass="FormField" ColumnSpan="3">
            <asp:TextBox ID="RedshiftBin" runat="server" Width="96px">0</asp:TextBox>
            <asp:RangeValidator ID="RedshiftBinRangeValidator" runat="server" ControlToValidate="RedshiftBin"
                Display="Dynamic" ErrorMessage="Invalid value" MaximumValue="10" MinimumValue="0.0001"
                SetFocusOnError="True" Type="Double"></asp:RangeValidator>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
