<%@ Control Language="C#" AutoEventWireup="true" Inherits="Jhu.SpecSvc.Web.Pipeline.Steps.WavelengthConversionStepControl"
    CodeBehind="WavelengthConversionStepControl.ascx.cs" %>
<asp:Table runat="server" CssClass="PipelineForm">
    <asp:TableRow runat="server">
        <asp:TableCell runat="server" CssClass="PipelineFormLabel">Method:</asp:TableCell>
        <asp:TableCell runat="server" CssClass="PipelineFormField">
            <asp:RadioButtonList ID="Method" runat="server" RepeatDirection="Vertical">
                <asp:ListItem Selected="True" Value="AirToVacuum">Air to vacuum</asp:ListItem>
                <asp:ListItem Value="VacuumToAir">Vacuum to air</asp:ListItem>
            </asp:RadioButtonList>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
