<%@ Control Language="C#" AutoEventWireup="true" Inherits="Jhu.SpecSvc.Web.Pipeline.Steps.ConvolutionStepControl"
    CodeBehind="ConvolutionStepControl.ascx.cs" %>
<table class="PipelineForm">
    <tr>
        <td class="PipelineFormLabel" style="text-align: right">
            σ<span style="font-size: 8pt; vertical-align: sub">v</span> =&nbsp;
        </td>
        <td class="PipelineFormField">
            <asp:TextBox ID="VelocityDispersion" runat="server">0</asp:TextBox>
            km/s
            <asp:RangeValidator ID="VelocityDispersionRangeValidator" runat="server" ControlToValidate="VelocityDispersion"
                Display="Dynamic" ErrorMessage="Invalid value" MaximumValue="1000" MinimumValue="0"
                SetFocusOnError="True" Type="Double"></asp:RangeValidator>
        </td>
    </tr>
</table>
