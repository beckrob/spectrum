<%@ Control Language="C#" AutoEventWireup="true" Inherits="Jhu.SpecSvc.Web.Pipeline.Formats.TabularFileLineEnding"
    CodeBehind="TabularFileLineEnding.ascx.cs" %>
<asp:RadioButtonList ID="lineEnding" runat="server" RepeatDirection="Horizontal">
    <asp:ListItem Selected="True" Value="Unix">LF</asp:ListItem>
    <asp:ListItem Value="Windows">CR LF</asp:ListItem>
</asp:RadioButtonList>
