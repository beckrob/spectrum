<%@ Control Language="C#" AutoEventWireup="true" Inherits="Jhu.SpecSvc.Web.Pipeline.Formats.TabularFileType"
    CodeBehind="TabularFileType.ascx.cs" %>
<asp:RadioButtonList ID="fileType" runat="server" RepeatDirection="Horizontal">
    <asp:ListItem Selected="True" Value="AsciiTabular">ASCII Tabular</asp:ListItem>
    <asp:ListItem Value="AsciiCsv">ASCII CSV</asp:ListItem>
    <asp:ListItem Enabled="False" Value="VOTable">VOTable</asp:ListItem>
</asp:RadioButtonList>
