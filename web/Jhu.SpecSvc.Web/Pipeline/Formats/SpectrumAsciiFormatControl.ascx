<%@ Control Language="C#" AutoEventWireup="True" Inherits="Jhu.SpecSvc.Web.Pipeline.Formats.SpectrumAsciiFormatControl"
    CodeBehind="SpectrumAsciiFormatControl.ascx.cs" %>
<table class="PipelineForm">
    <tr>
        <td class="PipelineFormLabel">
            Format:
        </td>
        <td class="PipelineFormField">
            <asp:RadioButtonList ID="FileType" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="Tabular">ASCII (Tabular)</asp:ListItem>
                <asp:ListItem Value="CommaSeparated">Comma Separated</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td class="PipelineFormLabel">
            File name prefix:
        </td>
        <td class="PipelineFormField">
            <asp:TextBox ID="Prefix" runat="server" CssClass="FormField"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="PipelineFormLabel">
            &nbsp;
        </td>
        <td class="PipelineFormField">
            <asp:CheckBox ID="WriteFields" runat="server" Text="Write detailed header" />
        </td>
    </tr>
    <tr>
        <td class="PipelineFormLabel">
            Columns:
        </td>
        <td class="PipelineFormField">
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
