<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tabs.ascx.cs" Inherits="Jhu.SpecSvc.Web.Search.Tabs" %>
<jgwc:TabHeader ID="TabHeader" runat="server" Orientation="Vertical" CssClass="TabVertical">
    <tabs>
    <jgwc:Tab runat="server" Text="Free search" ID="FreeSearch" />
    <jgwc:Tab runat="server" Text="Cone search" ID="ConeSearch" />
    <jgwc:Tab runat="server" Text="Region search" ID="RegionSearch" />
    <jgwc:Tab runat="server" Text="Redshift search" ID="RedshiftSearch" />
    <jgwc:Tab runat="server" Text="Advanced search" ID="AdvancedSearch" />
    <jgwc:Tab runat="server" Text="Model search" ID="ModelSearch" />
    <jgwc:Tab runat="server" Text="ID search" ID="IDSearch" />
    <jgwc:Tab runat="server" Text="SQL search" ID="SqlSearch" />
    <jgwc:Tab runat="server" Text="SkyServer search" ID="SkyServerSearch" />
    <jgwc:Tab runat="server" Text="Similar search" ID="SimilarSearch" />
    <jgwc:Tab runat="server" Text="Get whole collection" ID="AllSearch" />
    </tabs>
</jgwc:TabHeader>
