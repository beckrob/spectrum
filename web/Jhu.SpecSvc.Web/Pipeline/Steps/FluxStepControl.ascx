<%@ Control Language="C#" AutoEventWireup="true" Inherits="Jhu.SpecSvc.Web.Pipeline.Steps.FluxStepControl"
    CodeBehind="FluxStepControl.ascx.cs" %>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <table class="PipelineForm">
                <tr>
                    <td class="PipelineFormLabel">
                        Filters:
                    </td>
                    <td class="PipelineFormField">
                        <div style="width: 100%; overflow: auto; max-height: 128px; border: solid 1px #C0C0C0">
                            <asp:CheckBoxList ID="Filters" runat="server">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
        <td style="width: 18px"></td>
        <td runat="server">
            <table class="PipelineForm">
                <td class="PipelineFormLabel">
                    Redshift:
                </td>
                <td class="PipelineFormField">
                    <asp:RadioButtonList ID="Redshift" runat="server" RepeatDirection="Vertical" AutoPostBack="True"
                        OnSelectedIndexChanged="Redshift_SelectedIndexChanged">
                        <asp:ListItem Value="AsIs">As is</asp:ListItem>
                        <asp:ListItem>Variable</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <tr runat="server" id="RedshiftRow1">
                    <td class="PipelineFormLabel" style="text-align: right">
                        z<span style="font-size: 8pt; vertical-align: sub"><em>min</em></span> =
                    </td>
                    <td class="PipelineFormField">
                        <asp:TextBox ID="RedshiftMin" runat="server">0</asp:TextBox>
                        <asp:RangeValidator ID="RedshiftMinRangeValidator" runat="server" ControlToValidate="RedshiftMin"
                            Display="Dynamic" ErrorMessage="Invalid value" MaximumValue="10" MinimumValue="0"
                            SetFocusOnError="True" Type="Double"></asp:RangeValidator>
                    </td>
                </tr>
                <tr runat="server" id="RedshiftRow2">
                    <td class="PipelineFormLabel" style="text-align: right">
                        z<span style="font-size: 8pt; vertical-align: sub"><em>max</em></span> =
                    </td>
                    <td class="PipelineFormField">
                        <asp:TextBox ID="RedshiftMax" runat="server">0</asp:TextBox>
                        <asp:RangeValidator ID="RedshiftMaxRangeValidator" runat="server" ControlToValidate="RedshiftMax"
                            Display="Dynamic" ErrorMessage="Invalid value" MaximumValue="10" MinimumValue="0"
                            SetFocusOnError="True" Type="Double"></asp:RangeValidator>
                    </td>
                </tr>
                <tr runat="server" id="RedshiftRow3">
                    <td class="PipelineFormLabel" style="text-align: right">
                        Δz<span style="font-size: 8pt; vertical-align: sub"><em>bin</em></span> =
                    </td>
                    <td class="PipelineFormField">
                        <asp:TextBox ID="RedshiftBin" runat="server">0</asp:TextBox>
                        <asp:RangeValidator ID="RedshiftBinRangeValidator" runat="server" ControlToValidate="RedshiftBin"
                            Display="Dynamic" ErrorMessage="Invalid value" MaximumValue="10" MinimumValue="0.0001"
                            SetFocusOnError="True" Type="Double"></asp:RangeValidator>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
