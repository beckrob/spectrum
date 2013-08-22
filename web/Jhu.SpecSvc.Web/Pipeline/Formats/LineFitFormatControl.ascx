<%@ Control Language="C#" AutoEventWireup="true" Inherits="Jhu.SpecSvc.Web.Pipeline.Formats.LineFitFormatControl"
    CodeBehind="LineFitFormatControl.ascx.cs" %>
<%@ Register Src="TabularFileType.ascx" TagName="TabularFileType" TagPrefix="uc1" %>
<%@ Register Src="TabularFileLineEnding.ascx" TagName="TabularFileLineEnding" TagPrefix="uc2" %>
<table class="PipelineForm">
    <tr>
        <td class="PipelineFormLabel">
            File name prefix:
        </td>
        <td class="PipelineFormField">
            <asp:TextBox ID="Prefix" runat="server"></asp:TextBox>
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
