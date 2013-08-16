<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="processStepControls_ContinuumFitStep" Codebehind="ContinuumFitStepControl.ascx.cs" %>
    <table class="Form">
        <tr>
            <td class="FormLabel">Fitting method:</td>
            <td class="FormField"><asp:RadioButtonList ID="Method" runat="server"
                RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="NonNegativeLeastSquares">Non-negative least squares</asp:ListItem>
                <asp:ListItem Value="LeastSquares">Least squares</asp:ListItem>
            </asp:RadioButtonList></td>
        </tr>
        <tr>
            <td class="FormLabel">&nbsp;</td>
            <td class="FormField">
                <asp:CheckBox ID="WeightWithError" runat="server" Text="Weight with error" />
            </td>
        </tr>
        <tr>
            <td class="FormLabel">Masking:</td>
            <td class="FormField">
                <asp:CheckBox ID="MaskLines" runat="server" Text="Mask strong emission lines" />
            </td>
        </tr>
        <tr>
            <td class="FormLabel">&nbsp;</td>
            <td class="FormField">
                <asp:CheckBox ID="MaskSkyLines" runat="server" Text="Mask sky lines" />
            </td>
        </tr>
        <tr>
            <td class="FormLabel">&nbsp;</td>
            <td class="FormField">
                <asp:CheckBox ID="MaskFromSpectra" runat="server" 
                    Text="Use mask from spectrum" />
            </td>
        </tr>
        <tr>
            <td class="FormLabel">Template set:</td>
            <td class="FormField">
                <asp:DropDownList ID="TemplateSet" runat="server" AutoPostBack="True" 
                    CssClass="FormField" onselectedindexchanged="TemplateSet_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="FormLabel">&nbsp;</td>
            <td class="FormField">
                <div style="width: 100%; overflow: auto; max-height:200px">
                    <asp:CheckBoxList ID="Templates" runat="server">
                    </asp:CheckBoxList>
                </div>
            </td>
        </tr>
    </table>
