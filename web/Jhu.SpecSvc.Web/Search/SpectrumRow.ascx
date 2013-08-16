<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpectrumRow.ascx.cs"
    Inherits="Jhu.SpecSvc.Web.Search.SpectrumRow" %>
<tr id="selectionElement">
    <td>
        <asp:CheckBox ID="SelectionCheckbox" runat="server" Style="vertical-align: middle" />
    </td>
    <td>
        <asp:Label ID="Number" runat="server" Text="Label" />
    </td>
    <td>
        <asp:Label ID="Name" runat="server" Text="Label" />
    </td>
    <td>
        <asp:Label ID="Class" runat="server" Text="Label" />
    </td>
    <td>
        <asp:Label ID="Pos" runat="server" Text="Label" />
    </td>
    <td>
        <asp:Label ID="Redshift" runat="server" Text="Label" />
    </td>
    <td>
        <asp:HyperLink ID="PublisherID" runat="server" Text="PID" />
        <asp:HyperLink ID="CreatorID" runat="server" Text="CID" />
    </td>
</tr>
