<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CollectionList.ascx.cs" Inherits="Jhu.SpecSvc.Web.Controls.CollectionList" %>
<jgwc:MultiSelectGridView runat="server" ID="collectionList" DataKeyNames="Id" AutoGenerateColumns="false">
    <Columns>
        <jgwc:SelectionField />
        <asp:BoundField DataField="Name" />
        <asp:BoundField DataField="Type" />
    </Columns>
</jgwc:MultiSelectGridView>