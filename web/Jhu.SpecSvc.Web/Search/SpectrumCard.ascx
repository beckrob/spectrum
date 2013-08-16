<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpectrumCard.ascx.cs"
    Inherits="Jhu.SpecSvc.Web.Search.SpectrumCard" %>
<table class="SpectrumCard" id="selectionElement">
    <tr>
        <th colspan="2">
            <asp:CheckBox ID="SelectionCheckbox" runat="server" Style="vertical-align: middle" />
            <asp:Label ID="Name" runat="server" Text="Label" />
        </th>
        <th style="text-align: right">
            <asp:Label ID="Number" runat="server" Text="Label" />
        </th>
    </tr>
    <tr>
        <td style="width:60px">
            Class:
        </td>
        <td>
            <asp:Label ID="Class" runat="server" Text="Label" />
        </td>
        <td style="text-align: right">
            <asp:HyperLink ID="PublisherID" runat="server" Text="PID" />
        </td>
    </tr>
    <tr>
        <td style="width:60px">
            Redshift:
        </td>
        <td>
            <asp:Label ID="Redshift" runat="server" Text="Label" />
        </td>
        <td style="text-align: right">
            <asp:HyperLink ID="CreatorID" runat="server" Text="CID" />
        </td>
    </tr>
    <tr>
        <td class="ListCell">
            Position:
        </td>
        <td colspan="2">
            <asp:Label ID="Pos" runat="server" Text="Label" />
        </td>
    </tr>
    <tr>
        <td class="SpectrumGraph" colspan="3">
            <asp:Image ID="Graph" runat="server" />
        </td>
    </tr>
</table>
