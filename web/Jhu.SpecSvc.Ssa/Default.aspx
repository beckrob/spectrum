<%@ Page Language="C#" AutoEventWireup="true" Inherits="Jhu.SpecSvc.Ssa.Default"
    CodeBehind="Default.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IVOA Simple Spectral Access Protocol Entrypoint</title>
</head>
<body>
    <form method="GET">
    <input type="hidden" name="request" value="querydata" />
    <div>
        This is a IVOA Simple Spectral Access Protocol Entrypoint. For information about
        use, please refer to the documentation at the <a href="http://www.ivoa.net/Documents/">
            IVOA website</a>.<br />
        <br />
        <table>
            <tr>
                <td style="width: 100px">
                    POS=
                </td>
                <td style="width: 100px">
                    <input type="text" name="POS" value="180,1" />
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    SIZE=
                </td>
                <td style="width: 100px">
                    <input type="text" name="SIZE" value="0.16" />
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    TIME=
                </td>
                <td style="width: 100px">
                    <input type="text" name="TIME">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    SPECRP=
                </td>
                <td style="width: 100px">
                    <input type="text" name="SPECRP">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    SNR=
                </td>
                <td style="width: 100px">
                    <input type="text" name="SNR">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    REDSHIFT=
                </td>
                <td style="width: 100px">
                    <input type="text" name="REDSHIFT">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    VARAMPL=
                </td>
                <td style="width: 100px">
                    <input type="text" name="VARAMPL">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    TARGETNAME=
                </td>
                <td style="width: 100px">
                    <input type="text" name="TARGETNAME">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    TARGETCLASS=
                </td>
                <td style="width: 100px">
                    <input type="text" name="TARGETCLASS">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    FLUXCALIB=
                </td>
                <td style="width: 100px">
                    <input type="text" name="FLUXCALIB">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    FORMAT=
                </td>
                <td style="width: 100px">
                    <select name="FORMAT">
                        <option value="votable">VOTable</option>
                        <option value="xml">XML</option>
                        <option value="text">ASCII</option>
                        <option value="graph">GIF</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    COLLECTION=
                </td>
                <td style="width: 100px">
                    <select runat="server" id="Collection" name="COLLECTION" />
                </td>
            </tr>
        </table>
        <br />
        <input id="Submit1" type="submit" value="submit" />
        <input id="Reset1" type="reset" value="reset" /></div>
    </form>
</body>
</html>
