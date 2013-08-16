<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="processStepControls_NormalizeStep" Codebehind="NormalizeStepControl.ascx.cs" %>
<asp:Table runat="server" CssClass="Form">
    <asp:TableRow runat="server">
        <asp:TableCell runat="server" CssClass="FormLabel">
            Normalization method:</asp:TableCell>
        <asp:TableCell runat="server" CssClass="FormField" ColumnSpan="3">
            <asp:RadioButtonList ID="Method" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Method_SelectedIndexChanged"
                RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="FluxAtWavelength">Flux @ λ<span style="font-size: 8pt; vertical-align: sub">0</span></asp:ListItem>
                <asp:ListItem Value="FluxMedianInRanges">Median flux in ranges</asp:ListItem>
                <asp:ListItem Value="FluxIntegralInRanges">Integral flux in ranges</asp:ListItem>
            </asp:RadioButtonList>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server" ID="WavelengthRow" Visible="true">
        <asp:TableCell ID="TableCell1" runat="server" CssClass="FormLabelRight">
            λ<span style="font-size: 8pt; vertical-align: sub">0</span> =&nbsp;
        </asp:TableCell>
        <asp:TableCell ID="TableCell2" runat="server" CssClass="FormField" Width="20%">
            <asp:TextBox ID="Wavelength" runat="server" Width="96px" Enabled="False">0</asp:TextBox> Å
            <asp:RangeValidator ID="WavelengthRangeValidator" runat="server" ControlToValidate="Wavelength"
                Display="Dynamic" ErrorMessage="Invalid value" MaximumValue="100000" MinimumValue="0"
                SetFocusOnError="True" Type="Double"></asp:RangeValidator>
        </asp:TableCell>
        <asp:TableCell ID="TableCell3" runat="server" CssClass="FormLabelRight" Width="20%">
            f<span style="font-size: 8pt; vertical-align: sub">0</span> =&nbsp;
        </asp:TableCell>
        <asp:TableCell ID="TableCell4" runat="server" CssClass="FormField" Width="30%">
            <asp:TextBox ID="Flux" runat="server" Width="96px" Enabled="False">0</asp:TextBox> ADU
        </asp:TableCell>
    </asp:TableRow>
        <asp:TableRow runat="server" ID="TemplateRow" Visible="false">
        <asp:TableCell ID="TableCell5" runat="server" CssClass="FormLabel">
            Normalization template:
        </asp:TableCell>
        <asp:TableCell ID="TableCell6" runat="server" CssClass="FormField" ColumnSpan="3">
            <asp:RadioButtonList ID="Template" runat="server" AutoPostBack="True"
                RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="Galaxy">Galaxy</asp:ListItem>
                <asp:ListItem Value="Qso">QSO</asp:ListItem>
            </asp:RadioButtonList>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
