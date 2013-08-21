<%@ Control Language="C#" AutoEventWireup="true" Inherits="Jhu.SpecSvc.Web.Pipeline.Formats.LineFitFormatControl"
    CodeBehind="LineFitFormatControl.ascx.cs" %>
<%@ Register Src="TabularFileType.ascx" TagName="TabularFileType" TagPrefix="uc1" %>
<%@ Register Src="TabularFileLineEnding.ascx" TagName="TabularFileLineEnding" TagPrefix="uc2" %>
<table class="Form">
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
            File format:
        </td>
        <td class="FormField">
            <uc1:TabularFileType ID="FileFormat" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="FormLabel">
            Line endings:
        </td>
        <td class="FormField">
            <uc2:TabularFileLineEnding ID="LineEnding" runat="server" />
        </td>
    </tr>
</table>
