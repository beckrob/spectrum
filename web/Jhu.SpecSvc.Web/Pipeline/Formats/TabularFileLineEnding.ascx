<%@ Control Language="C#" AutoEventWireup="true" Inherits="Jhu.SpecSvc.Web.Pipeline.Formats.TabularFileLineEnding"
    CodeBehind="TabularFileLineEnding.ascx.cs" %>
<asp:RadioButtonList ID="lineEnding" runat="server" RepeatDirection="Horizontal">
    <asp:ListItem Value="Windows">Windows (CR LF)</asp:ListItem>
    <asp:ListItem Selected="True" Value="Unix">Unix (LF)</asp:ListItem>
</asp:RadioButtonList>
