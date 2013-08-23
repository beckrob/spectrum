<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.master" AutoEventWireup="true"
    CodeBehind="Model.aspx.cs" Inherits="Jhu.SpecSvc.Web.Search.Model" %>

<%@ Register Src="Tabs.ascx" TagName="Tabs" TagPrefix="jswu" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container">
        <div class="dock-left">
            <jswu:Tabs ID="SearchTabs" runat="server" SelectedTab="ModelSearch" />
        </div>
        <div class="TabFrameVertical dock-fill">
            <jgwc:Form runat="server" ID="ModelSearchForm" SkinID="Search" Text="Model search">
                <FormTemplate>
                    <p>
                        Find model spectra.
                    </p>
                    <ul>
                        <li>..</li>
                    </ul>
                    <table class="FormTable">
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="Z_metLabel" runat="server">z<sub>met</sub> between</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="Z_metFrom" runat="server" CssClass="FormFieldNarrow"></asp:TextBox>
                                and
                                <asp:TextBox ID="Z_metTo" runat="server" CssClass="FormFieldNarrow"></asp:TextBox><br />
                                <asp:RegularExpressionValidator ID="Z_metFromFormatValidator" runat="server" Display="Dynamic"
                                    ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="Z_metFrom"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="Z_metToFormatValidator" runat="server" Display="Dynamic"
                                    ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="Z_metTo"></asp:RegularExpressionValidator>
                            </td>
                            <td class="FormField" style="width: 15%; text-align: left">
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="T_effLabel" runat="server">T<sub>eff</sub> between:</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="T_effFrom" runat="server" CssClass="FormFieldNarrow"></asp:TextBox>
                                and
                                <asp:TextBox ID="T_effTo" runat="server" CssClass="FormFieldNarrow"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="T_effFromFormatValidator" runat="server" Display="Dynamic"
                                    ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="T_effFrom"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="T_effToFormatValidator" runat="server" Display="Dynamic"
                                    ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="T_effTo"></asp:RegularExpressionValidator>
                            </td>
                            <td class="FormField" style="width: 15%; text-align: left">
                                K
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="Log_gLabel" runat="server">log g between:</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="Log_gFrom" runat="server" CssClass="FormFieldNarrow"></asp:TextBox>
                                and
                                <asp:TextBox ID="Log_gTo" runat="server" CssClass="FormFieldNarrow"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="Log_gFromFormatValidator" runat="server" Display="Dynamic"
                                    ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="Log_gFrom"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="Log_gToFormatValidator" runat="server" Display="Dynamic"
                                    ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="Log_gTo"></asp:RegularExpressionValidator>
                            </td>
                            <td class="FormField" style="width: 15%; text-align: left">
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="Tau_V0Label" runat="server">τ<sub>V0</sub> between</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="Tau_V0From" runat="server" CssClass="FormFieldNarrow"></asp:TextBox>
                                and
                                <asp:TextBox ID="Tau_V0To" runat="server" CssClass="FormFieldNarrow"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="Tau_V0FromFormatValidator" runat="server" Display="Dynamic"
                                    ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="Tau_V0From"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="Tau_V0ToFormatValidator" runat="server" Display="Dynamic"
                                    ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="Tau_V0To"></asp:RegularExpressionValidator>
                            </td>
                            <td class="FormField" style="width: 15%; text-align: left">
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="MuLabel" runat="server">μ between:</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="MuFrom" runat="server" CssClass="FormFieldNarrow"></asp:TextBox>
                                and
                                <asp:TextBox ID="MuTo" runat="server" CssClass="FormFieldNarrow"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="MuFromFormatValidator" runat="server" ControlToValidate="MuFrom"
                                    Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="MuToFormatValidator" runat="server" ControlToValidate="MuTo"
                                    Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"></asp:RegularExpressionValidator>
                            </td>
                            <td class="FormField" style="width: 15%; text-align: left">
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="T_formLabel" runat="server" Text="t<sub>form</sub> between"></asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="T_formFrom" runat="server" CssClass="FormFieldNarrow"></asp:TextBox>
                                and
                                <asp:TextBox ID="T_formTo" runat="server" CssClass="FormFieldNarrow"></asp:TextBox><br />
                                <asp:RegularExpressionValidator ID="T_formFromFormatValidator" runat="server" ControlToValidate="T_formFrom"
                                    Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="T_formToFormatValidator" runat="server" ControlToValidate="T_formTo"
                                    Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"></asp:RegularExpressionValidator>
                            </td>
                            <td class="FormField" style="width: 15%; text-align: left">
                                Gyr
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="GammaLabel" runat="server" Text="Γ between"></asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="GammaFrom" runat="server" CssClass="FormFieldNarrow"></asp:TextBox>
                                and
                                <asp:TextBox ID="GammaTo" runat="server" CssClass="FormFieldNarrow"></asp:TextBox><br />
                                <asp:RegularExpressionValidator ID="GammaFromFormatValidator" runat="server" ControlToValidate="GammaFrom"
                                    Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="GammaToFormatValidator" runat="server" ControlToValidate="GammaTo"
                                    Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"></asp:RegularExpressionValidator>
                            </td>
                            <td class="FormField" style="width: 15%; text-align: left">
                                Gyr<span style="font-size: 8pt; vertical-align: super">-1</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="N_burstsLabel" runat="server" Text="n<sub>bursts</sub> between"></asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="N_burstsFrom" runat="server" CssClass="FormFieldNarrow"></asp:TextBox>
                                and
                                <asp:TextBox ID="N_burstsTo" runat="server" CssClass="FormFieldNarrow"></asp:TextBox><br />
                                <asp:RegularExpressionValidator ID="N_burstsFromFormatValidator" runat="server" ControlToValidate="N_burstsFrom"
                                    Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="N_burstsToFormatValidator" runat="server" ControlToValidate="N_burstsTo"
                                    Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"></asp:RegularExpressionValidator>
                            </td>
                            <td class="FormField" style="width: 15%; text-align: left">
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="AgeLabel" runat="server" Text="Age between"></asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="AgeFrom" runat="server" CssClass="FormFieldNarrow"></asp:TextBox>
                                and
                                <asp:TextBox ID="AgeTo" runat="server" CssClass="FormFieldNarrow"></asp:TextBox><br />
                                <asp:RegularExpressionValidator ID="AgeFromFormatValidator" runat="server" ControlToValidate="AgeFrom"
                                    Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="AgeToFormatValidator" runat="server" ControlToValidate="AgeTo"
                                    Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"></asp:RegularExpressionValidator>
                            </td>
                            <td class="FormField" style="width: 15%; text-align: left">
                                Gyr
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="Age_lastBurstLabel" runat="server" Text="Age<sub>last burst</sub> between"></asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="Age_lastBurstFrom" runat="server" CssClass="FormFieldNarrow"></asp:TextBox>
                                and
                                <asp:TextBox ID="Age_lastBurstTo" runat="server" CssClass="FormFieldNarrow"></asp:TextBox><br />
                                <asp:RegularExpressionValidator ID="Age_lastBurstFromFormatValidator" runat="server"
                                    ControlToValidate="Age_lastBurstFrom" Display="Dynamic" ErrorMessage="invalid format"
                                    ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="Age_lastBurstToFormatValidator" runat="server"
                                    ControlToValidate="Age_lastBurstTo" Display="Dynamic" ErrorMessage="invalid format"
                                    ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"></asp:RegularExpressionValidator>
                            </td>
                            <td class="FormField" style="width: 15%; text-align: left">
                                Gyr
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel" colspan="2">
                                <asp:Label runat="server" ID="CollectionLabel">Collections:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormList" colspan="2">
                                <jswc:CollectionList runat="server" ID="Collections" SearchMethod="Model" />
                            </td>
                        </tr>
                    </table>
                </FormTemplate>
                <ButtonsTemplate>
                    <asp:Button ID="OK" runat="server" Text="Run search" OnClick="Ok_Click" CssClass="FormButton" />
                </ButtonsTemplate>
            </jgwc:Form>
        </div>
    </div>
</asp:Content>
