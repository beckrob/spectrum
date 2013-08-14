<%@ Page Title="" Language="C#" MasterPageFile="~/UI.master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="Jhu.SpecSvc.Web.Search.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="toolbar" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middle" runat="server">
    <h3>
        Spectrum search results</h3>
    <asp:ObjectDataSource runat="server" ID="SpectrumDataSource" TypeName="Jhu.SpecSvc.Web.Search.ResultsDataSource"
        DataObjectTypeName="Jhu.SpecSvc.SpectrumLib.Spectrum" EnableViewState="true" EnablePaging="true"
        SelectMethod="GetResultsSpectra" SelectCountMethod="CountResultsSpectra" StartRowIndexParameterName="from"
        MaximumRowsParameterName="max" EnableCaching="false" OnObjectCreated="SpectrumDataSource_ObjectCreated" />
    <jgwc:MultiSelectListView runat="server" ID="spectrumList" GroupItemCount="4" DataSourceID="SpectrumDataSource">
        <LayoutTemplate>
            <table>
                <asp:PlaceHolder runat="server" ID="groupPlaceholder" />
            </table>
        </LayoutTemplate>
        <GroupTemplate>
            <tr>
                <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
            </tr>
        </GroupTemplate>
        <ItemTemplate>
            <%# Eval("Target.Name.Value") %>
        </ItemTemplate>
    </jgwc:MultiSelectListView>
</asp:Content>
