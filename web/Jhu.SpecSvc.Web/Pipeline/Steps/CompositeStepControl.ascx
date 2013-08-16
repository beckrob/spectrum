<%@ Control Language="C#" AutoEventWireup="true" Inherits="Jhu.SpecSvc.Web.Pipeline.Steps.CompositeStepControl"
    CodeBehind="CompositeStepControl.ascx.cs" %>
<asp:Table runat="server" CssClass="Form">
    <asp:TableRow runat="server">
        <asp:TableCell runat="server" CssClass="FormLabel">
            Composite method:</asp:TableCell>
        <asp:TableCell runat="server" CssClass="FormField" ColumnSpan="3">
            <asp:RadioButtonList ID="Method" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="Average">Average</asp:ListItem>
                <asp:ListItem Value="Median" Enabled="false">Median</asp:ListItem>
                <asp:ListItem Value="Sum">Sum</asp:ListItem>
            </asp:RadioButtonList>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server" Visible="true">
        <asp:TableCell runat="server" CssClass="FormLabel">
            Grouping:
        </asp:TableCell>
        <asp:TableCell runat="server" CssClass="FormField" ColumnSpan="3">
            <asp:CheckBox runat="server" ID="GroupByRedshift" Text="Group by redshift" AutoPostBack="true"
                OnCheckedChanged="GroupByRedshift_OnCheckedChanged" />
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server" ID="RedshiftRow" Visible="false">
        <asp:TableCell runat="server" CssClass="FormLabelRight">
            z<span style="font-size: 8pt; vertical-align: sub"><em>min</em></span> =
        </asp:TableCell>
        <asp:TableCell runat="server" CssClass="FormField" Width="20%">
            <asp:TextBox ID="RedshiftMin" runat="server" Width="96px" Enabled="False">0</asp:TextBox>
            <asp:RangeValidator ID="RedshiftMinRangeValidator" runat="server" ControlToValidate="RedshiftMin"
                Display="Dynamic" ErrorMessage="Invalid value" MaximumValue="10" MinimumValue="0"
                SetFocusOnError="True" Type="Double"></asp:RangeValidator>
        </asp:TableCell>
        <asp:TableCell runat="server" CssClass="FormLabelRight" Width="20%">
            z<span style="font-size: 8pt; vertical-align: sub"><em>max</em></span> =
        </asp:TableCell>
        <asp:TableCell runat="server" CssClass="FormField" Width="30%">
            <asp:TextBox ID="RedshiftMax" runat="server" Width="96px" Enabled="False">0</asp:TextBox>
            <asp:RangeValidator ID="RedshiftMaxRangeValidator" runat="server" ControlToValidate="RedshiftMax"
                Display="Dynamic" ErrorMessage="Invalid value" MaximumValue="10" MinimumValue="0"
                SetFocusOnError="True" Type="Double"></asp:RangeValidator>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server" ID="RedshiftBinRow" Visible="false">
        <asp:TableCell runat="server" CssClass="FormLabelRight">
            Δz<span style="font-size: 8pt; vertical-align: sub"><em>bin</em></span> =
        </asp:TableCell>
        <asp:TableCell runat="server" CssClass="FormField" ColumnSpan="3">
            <asp:TextBox ID="RedshiftBin" runat="server" Width="96px" Enabled="False">0</asp:TextBox>
            <asp:RangeValidator ID="RedshiftBinRangeValidator" runat="server" ControlToValidate="RedshiftBin"
                Display="Dynamic" ErrorMessage="Invalid value" MaximumValue="10" MinimumValue="0.0001"
                SetFocusOnError="True" Type="Double"></asp:RangeValidator>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
