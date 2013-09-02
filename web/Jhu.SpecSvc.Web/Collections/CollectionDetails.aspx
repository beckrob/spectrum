<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.Master" AutoEventWireup="true"
    CodeBehind="CollectionDetails.aspx.cs" Inherits="Jhu.SpecSvc.Web.Collections.CollectionDetails" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container dock-scroll">
        <jgwc:Form ID="ColectionDetailsForm" runat="server" SkinID="CollectionDetails">
            <FormTemplate>
                <p>
                    Enter a name for this pipeline.
                </p>
                <table class="FormTable">
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="CollectionIDLabel" runat="server">IVOA URI:</asp:Label>
                        </td>
                        <td class="FormField">
                            <asp:TextBox ID="CollectionID" runat="server" CssClass="FormField">ivo://myinstitution/myspectrum</asp:TextBox>
                            <asp:RegularExpressionValidator ID="IdFormatValidator" runat="server" ErrorMessage="Invalid URI format"
                                ControlToValidate="CollectionID" Display="Dynamic" ValidationExpression="ivo://([\w-]+\.)*[\w-]+(/[\w- ./?%&amp;=]*)?"></asp:RegularExpressionValidator>
                            <asp:CustomValidator ID="IdValidator" runat="server" ErrorMessage="IdValidator" ControlToValidate="CollectionID"
                                Display="Dynamic" OnServerValidate="IdValidator_ServerValidate"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="IdRequiredValidator" runat="server" ControlToValidate="CollectionID"
                                Display="Dynamic" ErrorMessage="URI is required"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="NameLabel" runat="server">Collection name:</asp:Label>
                        </td>
                        <td class="FormField">
                            <asp:TextBox ID="Name" runat="server" CssClass="FormField">My Spectrum Collection</asp:TextBox>
                            <asp:RequiredFieldValidator ID="NameRequiredValidator" runat="server" ControlToValidate="Name"
                                Display="Dynamic" ErrorMessage="Name is required"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="DescriptionLabel" runat="server">Description:</asp:Label>
                        </td>
                        <td class="FormField">
                            <asp:TextBox ID="Description" runat="server" CssClass="FormField" Rows="5"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="CollectionTypeLabel" runat="server">Collection type:</asp:Label>
                        </td>
                        <td class="FormField">
                            <asp:DropDownList ID="CollectionType" runat="server">
                                <asp:ListItem Value="2">SOAP Web Service</asp:ListItem>
                                <asp:ListItem Value="3">SSA endpoint</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="LocationLabel" runat="server">Geographical location:</asp:Label>
                        </td>
                        <td class="FormField">
                            <asp:TextBox ID="Location" runat="server" CssClass="FormField" Rows="5"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="ConnectionStringLabel" runat="server">Web Service URL:</asp:Label>
                        </td>
                        <td class="FormField">
                            <asp:TextBox ID="ConnectionString" runat="server">http://hostname/myspectrum/search.asmx</asp:TextBox>
                            <asp:CustomValidator ID="ConnectionStringValidator" runat="server" ErrorMessage="CustomValidator"
                                ControlToValidate="ConnectionString" Display="Dynamic" OnServerValidate="ConnectionStringValidator_ServerValidate"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="GraphUrlLabel" runat="server">Graph URL:</asp:Label>
                        </td>
                        <td class="FormField">
                            <asp:TextBox ID="GraphUrl" runat="server">http://hostname/myspectrum/graph.aspx?SpectrumID=[$ID]</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                        </td>
                        <td class="FormField">
                            <asp:CheckBox ID="Public" runat="server" Text="Public"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel" valign="top">
                            <asp:Label ID="SearchMethodsLabel" runat="server">Supported search methods:</asp:Label>
                        </td>
                        <td class="FormField">
                            <asp:CheckBoxList ID="SearchMethods" runat="server">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                </table>
            </FormTemplate>
            <ButtonsTemplate>
                <p class="FormMessage">
                </p>
                <p class="FormButtons">
                    <asp:Button ID="Ok" runat="server" Text="OK" OnClick="Ok_Click" />
                    <asp:Button ID="Cancel" runat="server" Text="Cancel" CausesValidation="false" OnClick="Cancel_Click" />
                </p>
            </ButtonsTemplate>
        </jgwc:Form>
    </div>
</asp:Content>
