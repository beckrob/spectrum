<%@ Control Language="C#" AutoEventWireup="True" Inherits="Jhu.SpecSvc.Web.Pipeline.Formats.SpectrumAsciiFormatControl"
    CodeBehind="SpectrumAsciiFormatControl.ascx.cs" %>
<table class="Form">
    <tr>
        <td class="FormLabel">
            Format:
        </td>
        <td class="FormField">
            <asp:RadioButtonList ID="FileType" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="Tabular">ASCII (Tabular)</asp:ListItem>
                <asp:ListItem Value="CommaSeparated">Comma Separated</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td class="FormLabel">
            File name prefix:
        </td>
        <td class="FormField">
            <asp:TextBox ID="Prefix" runat="server" CssClass="FormField"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="FormLabel">
            &nbsp;
        </td>
        <td class="FormField">
            <asp:CheckBox ID="WriteFields" runat="server" Text="Write detailed header" />
        </td>
    </tr>
    <tr>
        <td class="FormLabel">
            Columns:
        </td>
        <td class="FormField">
            <asp:ListView runat="server" ID="columns" OnItemCreated="columns_ItemCreated" OnLoad="columns_Load">
                <LayoutTemplate>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                </LayoutTemplate>
                <ItemTemplate>
                    <asp:DropDownList ID="column" runat="server" AutoPostBack="true">
                    </asp:DropDownList>
                </ItemTemplate>
            </asp:ListView>
            <asp:DropDownList ID="newColumn" runat="server" AutoPostBack="true" OnSelectedIndexChanged="newColumn_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
</table>
