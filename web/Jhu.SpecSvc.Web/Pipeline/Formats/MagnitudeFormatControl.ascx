<%@ Control Language="C#" AutoEventWireup="true" Inherits="Jhu.SpecSvc.Web.Pipeline.Formats.MagnitudeFormatControl"
    CodeBehind="MagnitudeFormatControl.ascx.cs" %>
<%@ Register Src="TabularFileType.ascx" TagName="TabularFileType" TagPrefix="uc1" %>
<%@ Register Src="TabularFileLineEnding.ascx" TagName="TabularFileLineEnding" TagPrefix="uc2" %>
<table class="PipelineForm">
    <tr>
        <td class="PipelineFormLabel">
            Magnitude system:
        </td>
        <td class="PipelineFormField">
            <asp:RadioButtonList ID="MagnitudeSystem" runat="server" RepeatDirection="Vertical">
                <asp:ListItem Selected="True" Value="ABMagnitude">AB magnitude</asp:ListItem>
                <asp:ListItem Value="Flux">Flux [erg s<span style="font-size: 8pt; vertical-align: super">-1</span> cm<span style="font-size: 8pt; vertical-align: super">-2</span>]</asp:ListItem>
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
            File format:
        </td>
        <td class="PipelineFormField">
            <uc1:TabularFileType ID="FileFormat" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="PipelineFormLabel">
            Line endings:
        </td>
        <td class="PipelineFormField">
            <uc2:TabularFileLineEnding ID="LineEnding" runat="server" />
        </td>
    </tr>
</table>
