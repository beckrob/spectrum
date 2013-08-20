<%@ Control Language="C#" AutoEventWireup="true" Inherits="Jhu.SpecSvc.Web.Pipeline.Steps.CompositeStepControl"
    CodeBehind="CompositeStepControl.ascx.cs" %>
<table class="PipelineForm">
    <tr>
        <td class="PipelineFormLabel">
            Method:</td>
        <td class="PipelineFormField">
            <asp:RadioButtonList ID="Method" runat="server" RepeatDirection="Vertical">
                <asp:ListItem Selected="True" Value="Average">Average</asp:ListItem>
                <asp:ListItem Value="Median" Enabled="false">Median</asp:ListItem>
                <asp:ListItem Value="Sum">Sum</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
</table>
