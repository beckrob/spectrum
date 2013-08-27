<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.master" AutoEventWireup="true"
    CodeBehind="PipelineList.aspx.cs" Inherits="Jhu.SpecSvc.Web.Pipeline.PipelineList" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container dock-scroll">
        <jgwc:Form ID="PipelineListForm" runat="server" SkinID="PipelineList">
            <FormTemplate>
                <p>
                    Select a pipeline from the list below.
                </p>
                <table class="FormTable">
                    <tr>
                        <td class="FormLabel">
                            <asp:Label runat="server" ID="NameLabel">Pipeline:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormList">
                            <jgwc:MultiSelectGridView runat="server" ID="List" DataKeyNames="ID" AutoGenerateColumns="false">
                                <Columns>
                                    <jgwc:SelectionField />
                                    <asp:BoundField DataField="Name" HeaderText="Pipeline name" />
                                </Columns>
                            </jgwc:MultiSelectGridView>
                        </td>
                    </tr>
                </table>
            </FormTemplate>
            <ButtonsTemplate>
                <p class="FormMessage">
                    <asp:CustomValidator runat="server" ID="ListSelectedValidator" OnServerValidate="ListSelectedValidator_ServerValidate"
                        Display="Dynamic" Text="No pipeline selected" />
                </p>
                <p class="FormButtons">
                    <asp:Button runat="server" ID="Ok" Text="OK" OnClick="Ok_Click" />
                    <asp:Button runat="server" ID="Rename" Text="Rename" OnClick="Rename_Click" />
                    <asp:Button runat="server" ID="Delete" Text="Delete" OnClick="Delete_Click" />
                    <asp:Button runat="server" ID="Cancel" Text="Cancel" OnClick="Cancel_Click" CausesValidation="false" />
                </p>
            </ButtonsTemplate>
        </jgwc:Form>
    </div>
</asp:Content>
