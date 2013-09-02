<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Jhu.SpecSvc.Web.Programming.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle" runat="server">
    <div class="LayoutContent dock-fill dock-container dock-scroll">
        <jgwc:Form ID="ProgrammingForm" runat="server" SkinID="ProgrammingForm" Text="Programming Spectrum Services">
            <FormTemplate>
                <p>
                    You can access spectrum services via SOAP web services and a VO-complian SSA service.
                </p>
                <ul>
                    <li>SOAP web services documentation</li>
                    <li>SSA documentation</li>
                </ul>
                <table id="Table2" cellspacing="1" cellpadding="0" width="100%" border="0" style="padding-right: 1px;
                    padding-left: 1px; padding-bottom: 1px; padding-top: 1px">
                    <tr style="background-color: lightgrey">
                        <td>
                            <p align="center">
                                <strong>Service</strong></p>
                        </td>
                        <td style="background-color: lightgrey">
                            <p align="center">
                                <strong>Home</strong></p>
                        </td>
                        <td style="background-color: lightgrey">
                            <p align="center">
                                <strong>WSDL</strong></p>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <p align="left">
                                Search</p>
                            <p align="left">
                                Use this service to query the spectrum catalogs and retrieve flux values vs. wavelengths.</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                <asp:HyperLink runat="server" ID="SearchHome" Text="link" /></p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                <asp:HyperLink runat="server" ID="SearchWsdl" Text="link" /></p>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <p align="left">
                                Jobs</p>
                            <p align="left">
                                Use this service to process spectra in batch.</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                <asp:HyperLink runat="server" ID="JobsHome" Text="link" /></p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                <asp:HyperLink runat="server" ID="JobsWsdl" Text="link" /></p>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <p align="left">
                                MySpectrum Search</p>
                            <p align="left">
                                Use this service to create, modify and delete your own spectra from a client application.
                                Note: these point to the currently set MySpectrum Service, defaults to the MySpectrum
                                Service at JHU.</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                <asp:HyperLink runat="server" ID="MySpectrumSearchHome" Text="link" /></p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                <asp:HyperLink runat="server" ID="MySpectrumSearchWsdl" Text="link" /></p>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <p align="left">
                                MySpectrum Admin</p>
                            <p align="left">
                                Use this service to create, modify and delete your own spectra from a client application.
                                Note: these point to the currently set MySpectrum Service, defaults to the MySpectrum
                                Service at JHU.</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                <asp:HyperLink runat="server" ID="MySpectrumAdminHome" Text="link" /></p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                <asp:HyperLink runat="server" ID="MySpectrumAdminWsdl" Text="link" /></p>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <p align="left">
                                SSA</p>
                            <p align="left">
                                IVOA Simple Spectral Access Protocol entrypoint</p>
                        </td>
                        <td valign="top" style="text-align: center">
                            <p align="center">
                                <asp:HyperLink runat="server" ID="Ssa" Text="link" /></p>
                        </td>
                        <td valign="top">
                        </td>
                    </tr>
                </table>
            </FormTemplate>
        </jgwc:Form>
    </div>
</asp:Content>
