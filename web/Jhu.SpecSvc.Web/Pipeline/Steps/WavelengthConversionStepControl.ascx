<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="processStepControls_WavelengthConversionStep" Codebehind="WavelengthConversionStepControl.ascx.cs" %>
<asp:Table runat="server" CssClass="Form">
    <asp:TableRow runat="server">
        <asp:TableCell runat="server" CssClass="FormLabel">
            Conversion method:</asp:TableCell>
        <asp:TableCell runat="server" CssClass="FormField">
            <asp:RadioButtonList ID="Method" runat="server" AutoPostBack="True" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="AirToVacuum">Air to vacuum</asp:ListItem>
                <asp:ListItem Value="VacuumToAir">Vacuum to air</asp:ListItem>
                <asp:ListItem Value="None">None</asp:ListItem>
            </asp:RadioButtonList>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
