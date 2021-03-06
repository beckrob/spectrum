﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="Jhu.SpecSvc.Web.Pipeline.Steps.RedshiftStepControl"
    CodeBehind="RedshiftStepControl.ascx.cs" %>
<asp:Table runat="server" CssClass="PipelineForm">
    <asp:TableRow runat="server">
        <asp:TableCell runat="server" CssClass="PipelineFormLabel">Method:</asp:TableCell>
        <asp:TableCell runat="server" CssClass="PipelineFormField">
            <asp:RadioButtonList ID="Method" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Method_SelectedIndexChanged"
                RepeatDirection="Vertical">
                <asp:ListItem Selected="True" Value="RestFrame">Restframe</asp:ListItem>
                <asp:ListItem Value="ObservationFrame">Observed frame</asp:ListItem>
                <asp:ListItem Value="Custom">Custom redshift</asp:ListItem>
            </asp:RadioButtonList>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server" ID="RedshiftRow" Visible="false">
        <asp:TableCell runat="server" CssClass="PipelineFormLabel" HorizontalAlign="Right">
            z =&nbsp;
        </asp:TableCell>
        <asp:TableCell runat="server" CssClass="PipelineFormField">
            <asp:TextBox ID="Redshift" runat="server" Width="96px" Enabled="False">0</asp:TextBox>
            <asp:RangeValidator ID="RedshiftRangeValidator" runat="server" ControlToValidate="Redshift"
                Display="Dynamic" ErrorMessage="Invalid value" MaximumValue="10" MinimumValue="-10"
                SetFocusOnError="True" Type="Double"></asp:RangeValidator>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
