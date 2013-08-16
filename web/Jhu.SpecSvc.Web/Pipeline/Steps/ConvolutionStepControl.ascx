<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="processStepControls_ConvolutionStep" Codebehind="ConvolutionStepControl.ascx.cs" %>
<asp:Table runat="server" CssClass="Form">
    <asp:TableRow runat="server">
        <asp:TableCell ID="TableCell1" runat="server" CssClass="FormLabelRight">
            σ<span style="font-size: 8pt; vertical-align: sub">v</span> =&nbsp;
        </asp:TableCell>
        <asp:TableCell ID="TableCell2" runat="server" CssClass="FormField">
            <asp:TextBox ID="VelocityDispersion" runat="server" Width="96px">0</asp:TextBox> km/s
            <asp:RangeValidator ID="VelocityDispersionRangeValidator" runat="server" ControlToValidate="VelocityDispersion"
                Display="Dynamic" ErrorMessage="Invalid value" MaximumValue="1000" MinimumValue="0"
                SetFocusOnError="True" Type="Double"></asp:RangeValidator>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
