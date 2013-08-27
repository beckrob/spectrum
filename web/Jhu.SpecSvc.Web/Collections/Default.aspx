<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Jhu.SpecSvc.Web.Collections.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container dock-scroll">
        <jgwc:Form ID="CollectionListForm" runat="server" SkinID="CollectionList" Text="Collection">
            <FormTemplate>
                <p>
                    View collection availability and manage your own collections.
                </p>
                <ul>
                    <li>sign in to manage your own collections</li>
                </ul>
                <table class="FormTable">
                    <tr>
                        <td class="FormLabel">
                            <asp:Label runat="server" ID="NameLabel">Collection:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormList">
                            <jgwc:MultiSelectGridView runat="server" ID="List" DataKeyNames="ID" AutoGenerateColumns="false">
                                <Columns>
                                    <jgwc:SelectionField />
                                    <asp:BoundField DataField="Name" HeaderText="Collection name" />
                                    <asp:BoundField DataField="Type" HeaderText="Type" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                </Columns>
                            </jgwc:MultiSelectGridView>
                        </td>
                    </tr>
                </table>
            </FormTemplate>
            <ButtonsTemplate>
                <p class="FormMessage">
                    <asp:CustomValidator runat="server" ID="ListSelectedValidator" OnServerValidate="ListSelectedValidator_ServerValidate"
                        Display="Dynamic" Text="No collection selected" />
                </p>
                <p class="FormButtons">
                    <asp:Button runat="server" ID="Create" Text="Create" OnClick="Create_Click" />
                    <asp:Button runat="server" ID="Modify" Text="Modify" OnClick="Modify_Click" />
                    <asp:Button runat="server" ID="Delete" Text="Delete" OnClick="Delete_Click" />
                    |
                    <asp:Button runat="server" ID="Test" Text="Test availability" OnClick="Test_Click" CausesValidation="false" />
                </p>
            </ButtonsTemplate>
        </jgwc:Form>
    </div>
</asp:Content>
