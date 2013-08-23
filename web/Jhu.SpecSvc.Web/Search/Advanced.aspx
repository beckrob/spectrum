<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.master" AutoEventWireup="true"
    CodeBehind="Advanced.aspx.cs" Inherits="Jhu.SpecSvc.Web.Search.Advanced" %>

<%@ Register Src="Tabs.ascx" TagName="Tabs" TagPrefix="jswu" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container">
        <div class="dock-left">
            <jswu:Tabs ID="SearchTabs" runat="server" SelectedTab="AdvancedSearch" />
        </div>
        <div class="TabFrameVertical dock-fill dock-scroll">
            <jgwc:Form runat="server" ID="ConeSearchForm" SkinID="Search" Text="Advanced search">
                <FormTemplate>
                    <p>
                        Find spectra by many criteria.
                    </p>
                    <ul>
                        <li>enter required criteria</li>
                        <li>leave textboxes empty to omit criteria</li>
                    </ul>
                    <table class="FormTable">
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="KeywordLabel" runat="server">Keyword:</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="Keyword" runat="server" CssClass="FormField"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="NameLabel" runat="server">Name:</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="Name" runat="server" CssClass="FormField"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="TargetClassLabel" runat="server">Target class:</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:DropDownList ID="TargetClass" runat="server" CssClass="FormField">
                                    <asp:ListItem Value="0">(any)</asp:ListItem>
                                    <asp:ListItem Value="STAR">Star</asp:ListItem>
                                    <asp:ListItem Value="GALAXY">Galaxy</asp:ListItem>
                                    <asp:ListItem Value="QSO">QSO</asp:ListItem>
                                    <asp:ListItem Value="SKY">Sky</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="SpectralClassLabel" runat="server">Spectral class:</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="SpectralClass" runat="server" CssClass="FormField"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="CreationTypeLabel" runat="server">Creation type:</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:DropDownList ID="CreationType" runat="server" CssClass="FormField">
                                    <asp:ListItem Value="0">(any)</asp:ListItem>
                                    <asp:ListItem Value="SURVEY">Survey</asp:ListItem>
                                    <asp:ListItem Value="POINTED">Pointed</asp:ListItem>
                                    <asp:ListItem Value="THEORY">Theory</asp:ListItem>
                                    <asp:ListItem Value="ARTIFICAL">Artifical</asp:ListItem>
                                    <asp:ListItem Value="COMPOSITE">Composite</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                &nbsp;
                            </td>
                            <td class="FormField">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="RaLabel" runat="server">ra: (deg/hms)</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="Ra" runat="server" CssClass="FormField"></asp:TextBox>
                                <%--<asp:CustomValidator ID="RaValidator" runat="server" Display="Dynamic" ErrorMessage="CustomValidator"
                                    OnServerValidate="RaFromValidator_ServerValidate"></asp:CustomValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="DecLabel" runat="server">dec: (deg/dms)</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="Dec" runat="server" CssClass="FormField"></asp:TextBox>
                                <%--<asp:CustomValidator ID="DecValidator" runat="server" Display="Dynamic" ErrorMessage="CustomValidator"
                                    OnServerValidate="DecFromValidator_ServerValidate"></asp:CustomValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="SrLabel" runat="server" Text="SR: (arcmin)"></asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="Sr" runat="server" CssClass="FormField"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                &nbsp;
                            </td>
                            <td class="FormField">
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="SnrLabel" runat="server">SNR between:</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="SnrFrom" runat="server" CssClass="FormField" Width="64px"></asp:TextBox>
                                and
                                <asp:TextBox ID="SnrTo" runat="server" CssClass="FormField" Width="64px"></asp:TextBox><br />
                                <asp:RegularExpressionValidator ID="SnrFromFormatValidator" runat="server" Display="Dynamic"
                                    ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="SnrFrom"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="SnrToFormatValidator" runat="server" Display="Dynamic"
                                    ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="SnrTo"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="VarAmplLabel" runat="server">VarAmpl between:</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="VarAmplFrom" runat="server" CssClass="FormField" Width="64px"></asp:TextBox>
                                and
                                <asp:TextBox ID="VarAmplTo" runat="server" CssClass="FormField" Width="64px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="VarAmplFromFormatValidator" runat="server" Display="Dynamic"
                                    ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="VarAmplFrom"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="VarAmplToFormatValidator" runat="server" Display="Dynamic"
                                    ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="VarAmplTo"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="RedshiftLabel" runat="server">z between:</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="RedshiftFrom" runat="server" CssClass="FormField" Width="64px"></asp:TextBox>
                                and
                                <asp:TextBox ID="RedshiftTo" runat="server" CssClass="FormField" Width="64px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RedshiftFromFormatValidator" runat="server" Display="Dynamic"
                                    ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="RedshiftFrom"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="RedshiftToFormatValidator" runat="server" Display="Dynamic"
                                    ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="RedshiftTo"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="RedshiftStatErrorLabel" runat="server">z<sub>Err</sub> between<br /> (absolute):</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="RedshiftStatErrorFrom" runat="server" CssClass="FormField" Width="64px"></asp:TextBox>
                                and
                                <asp:TextBox ID="RedshiftStatErrorTo" runat="server" CssClass="FormField" Width="64px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RedshiftStatErrorFromFormatValidator" runat="server"
                                    Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="RedshiftStatErrorFrom"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="RedshiftStatErrorToFormatValidator" runat="server"
                                    Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="RedshiftStatErrorTo"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="RedshiftConfidenceLabel" runat="server">z<sub>Confidence</sub> between (absolute):</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="RedshiftConfidenceFrom" runat="server" CssClass="FormField" Width="64px"></asp:TextBox>
                                and
                                <asp:TextBox ID="RedshiftConfidenceTo" runat="server" CssClass="FormField" Width="64px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RedshiftConficendeFromFormatValidator" runat="server"
                                    ControlToValidate="RedshiftConfidenceFrom" Display="Dynamic" ErrorMessage="invalid format"
                                    ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="RedshiftConfidenceToFormatValidator" runat="server"
                                    ControlToValidate="RedshiftConfidenceTo" Display="Dynamic" ErrorMessage="invalid format"
                                    ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                &nbsp;
                            </td>
                            <td class="FormField">
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="DateLabel" runat="server">Measured between:</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="DateFrom" runat="server" CssClass="FormField" Width="64px"></asp:TextBox>
                                and
                                <asp:TextBox ID="DateTo" runat="server" CssClass="FormField" Width="64px"></asp:TextBox>
                                <%--<asp:CustomValidator ID="DateFromValidator" runat="server" Display="Dynamic" ErrorMessage="Invalid date!"
                                    ControlToValidate="DateFrom"></asp:CustomValidator>
                                <asp:CustomValidator ID="DateToValidator" runat="server" Display="Dynamic" ErrorMessage="Invalid date!"
                                    ControlToValidate="DateTo"></asp:CustomValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="VersionLabel" runat="server">Version:</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="Version" runat="server" CssClass="FormField" Width="64px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                &nbsp;
                            </td>
                            <td class="FormField">
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="SpectralCoverageLabel" runat="server">λ [Å] between:</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="SpectralCoverageFrom" runat="server" CssClass="FormField" Width="64px"></asp:TextBox>
                                and
                                <asp:TextBox ID="SpectralCoverageTo" runat="server" CssClass="FormField" Width="64px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="SpectralCoverageFromFormatValidator" runat="server"
                                    Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="SpectralCoverageFrom"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="SpectralCoverageToFormatValidator" runat="server"
                                    Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="SpectralCoverageTo"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="SpectralResPowerLabel" runat="server">λ/Δλ between:</asp:Label>
                            </td>
                            <td class="FormField">
                                <asp:TextBox ID="SpectralResPowerFrom" runat="server" CssClass="FormField" Width="64px"></asp:TextBox>
                                and
                                <asp:TextBox ID="SpectralResPowerTo" runat="server" CssClass="FormField" Width="64px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="SpectralResPowerFromFormatValidator" runat="server"
                                    Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="SpectralResPowerFrom"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="SpectralResPowerToFormatValidator" runat="server"
                                    Display="Dynamic" ErrorMessage="invalid format" ValidationExpression="^[-+]?[0-9]*[.]?[0-9]*([eE][-+]?[0-9]+)?$"
                                    ControlToValidate="SpectralResPowerTo"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel" style="height: 18px">
                                <asp:Label ID="FluxCalibrationLabel" runat="server">Flux calibration:</asp:Label>
                            </td>
                            <td class="FormField" style="width: 35%; height: 18px">
                                <asp:DropDownList ID="FluxCalibration" runat="server" CssClass="FormField">
                                    <asp:ListItem Value="0">(any)</asp:ListItem>
                                    <asp:ListItem Value="CALIBRATED">Calibrated</asp:ListItem>
                                    <asp:ListItem Value="NORMALIZED">Normalized</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel" colspan="2">
                                <asp:Label runat="server" ID="CollectionLabel">Collections:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormList" colspan="2">
                                <jswc:CollectionList runat="server" ID="Collections" SearchMethod="Advanced" />
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
