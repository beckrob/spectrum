<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Jhu.SpecSvc.Web.Pipeline.Formats.SpectrumPlotFormatControl" Codebehind="SpectrumPlotFormatControl.ascx.cs" %>
<table class="Form">
    <tr>
        <td class="FormLabel">
            File format:
        </td>
        <td class="FormField" colspan="3">
            <asp:RadioButtonList ID="ImageFormat" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="Jpeg">JPEG</asp:ListItem>
                <asp:ListItem Value="Gif">GIF</asp:ListItem>
                <asp:ListItem Value="Png">PNG</asp:ListItem>
                <asp:ListItem Value="Ps">PS</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td class="FormLabel">
            Width:
        </td>
        <td class="FormField" style="width: 20%">
            <asp:TextBox ID="Width" runat="server" CssClass="FormField"></asp:TextBox>
        </td>
        <td class="FormLabel" style="width: 30%">
            Height:
        </td>
        <td class="FormField" style="width: 20%">
            <asp:TextBox ID="Height" runat="server" CssClass="FormField"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="FormLabelRight">
            λ<sub>min</sub> =
        </td>
        <td class="FormField" style="width: 20%">
            <asp:TextBox ID="XMin" runat="server" CssClass="FormField"></asp:TextBox>
        </td>
        <td class="FormLabelRight" style="width: 30%">
            λ<sub>max</sub> =
        </td>
        <td class="FormField" style="width: 20%">
            <asp:TextBox ID="XMax" runat="server" CssClass="FormField"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="FormLabel">
            &nbsp;
        </td>
        <td class="FormField" colspan="3">
            <asp:CheckBox ID="XLogScale" runat="server" Text="log λ scale" />
        </td>
    </tr>
    <tr>
        <td class="FormLabelRight">
            f<sub>min</sub> =
        </td>
        <td class="FormField" style="width: 20%">
            <asp:TextBox ID="YMin" runat="server" CssClass="FormField"></asp:TextBox>
        </td>
        <td class="FormLabelRight" style="width: 30%">
            f<sub>max</sub> =
        </td>
        <td class="FormField" style="width: 20%">
            <asp:TextBox ID="YMax" runat="server" CssClass="FormField"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="FormLabel">
            &nbsp;
        </td>
        <td class="FormField" colspan="3">
            <asp:CheckBox ID="YLogScale" runat="server" Text="log f scale" />
        </td>
    </tr>
    <tr>
        <td class="FormLabel">
            &nbsp;
        </td>
        <td class="FormField" colspan="3">
            <asp:CheckBox ID="SpectralLines" runat="server" Text="Mark spectral lines" />
        </td>
    </tr>
    <tr>
        <td class="FormLabel">
            File name prefix:
        </td>
        <td class="FormField" colspan="3">
            <asp:TextBox ID="Prefix" runat="server" CssClass="FormField"></asp:TextBox>
        </td>
    </tr>
</table>
