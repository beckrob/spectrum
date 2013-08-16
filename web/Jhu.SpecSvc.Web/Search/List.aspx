<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="Jhu.SpecSvc.Web.Search.List" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <style>
        table.SpectrumCard
        {
            border: 1px solid #C0C0C0;
            border-collapse: collapse;
            display: inline-table;
            margin-right: 4px;
            margin-bottom: 8px;
        }
        table.SpectrumCard tr th
        {
            background-color: #C0C0C0;
            text-align: left;
            vertical-align: middle;
        }
        table.SpectrumCard tr td img
        {
            width: <%= GraphWidth %>px;
            height: <%= GraphHeight %>px;
        }
        table.SpectrumCard tr td
        {
            padding: 2px;
        }
        table.SpectrumCard tr td.SpectrumGraph
        {
            padding: 0px;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="dock-top">
        <jgwc:Toolbar runat="server">
            <jgwc:ToolbarElement runat="server" Width="200px">
                Spectrum search results
            </jgwc:ToolbarElement>
            <jgwc:ToolbarElement runat="server" Width="120px">
                <asp:Label ID="OutputTableLabel" runat="server" Text="View:" /><br />
                <asp:DropDownList ID="SpectrumListViewList" runat="server" CssClass="ToolbarControl"
                    Width="120px" AutoPostBack="true" OnSelectedIndexChanged="SpectrumListViewList_SelectedIndexChanged"
                    CausesValidation="false">
                    <asp:ListItem Value="List">Details list</asp:ListItem>
                    <asp:ListItem Value="Graph">Spectrum plot</asp:ListItem>
                    <asp:ListItem Value="Image">SDSS image</asp:ListItem>
                </asp:DropDownList>
            </jgwc:ToolbarElement>
            <jgwc:ToolbarElement runat="server" Width="120px">
                <asp:Label ID="CommentsLabel" runat="server" Text="Coordinate display:" /><br />
                <asp:DropDownList ID="DegreeFormatList" runat="server" CssClass="ToolbarControl"
                    Width="120px" AutoPostBack="true" OnSelectedIndexChanged="DegreeFormatList_SelectedIndexChanged"
                    CausesValidation="false">
                    <asp:ListItem Value="Decimal">Decimal</asp:ListItem>
                    <asp:ListItem Value="Sexagesimal">Sexagesimal</asp:ListItem>
                </asp:DropDownList>
            </jgwc:ToolbarElement>
            <jgwc:ToolbarElement runat="server" Width="80px">
                <asp:Label runat="server" Text="Total results:" /><br />
                <asp:Label runat="server" ID="TotalResultsCount" CssClass="ToolbarControl" Width="80px"
                    Style="text-align: center" />
            </jgwc:ToolbarElement>
            <jgwc:ToolbarElement runat="server" Width="80px">
                <asp:Label runat="server" Text="Displaying:" /><br />
                <asp:Label runat="server" ID="DisplayedResultsRange" CssClass="ToolbarControl" Width="80px"
                    Style="text-align: center" />
            </jgwc:ToolbarElement>
            <jgwc:ToolbarElement runat="server" Width="200px">
                <asp:Label runat="server" Text="Jump to page:" /><br />
                <span class="ToolbarControl" style="display: inline-block; width: 200px; text-align: center">
                    <asp:DataPager ID="SpectrumListPager" runat="server" PagedControlID="SpectrumList"
                        PageSize="15">
                        <Fields>
                            <asp:NextPreviousPagerField FirstPageText="<<" PreviousPageText="<" ShowFirstPageButton="true"
                                ShowPreviousPageButton="true" ShowNextPageButton="false" ShowLastPageButton="false" />
                            <asp:NumericPagerField ButtonCount="5" NextPageText="..." PreviousPageText="..." />
                            <asp:NextPreviousPagerField LastPageText=">>" NextPageText=">" ShowFirstPageButton="false"
                                ShowPreviousPageButton="false" ShowNextPageButton="true" ShowLastPageButton="true" />
                        </Fields>
                    </asp:DataPager>
                </span>
            </jgwc:ToolbarElement>
            <jgwc:ToolbarElement runat="server" />
        </jgwc:Toolbar>
    </div>
    <div class="dock-bottom">
        <p class="FormMessage">
            <asp:CustomValidator ID="SpectrumSelectedValidator" runat="server" ErrorMessage="No spectrum was selected. Click on the checkboxes
            next to object names to select a spectrum." OnServerValidate="SpectrumSelected_ServerValidate" /></p>
        <p class="FormButtons">
            <jswc:WizardButtons runat="server" OkEnabled="true" OnCommand="Button_Command" />
        </p>
    </div>
    <div class="LayoutContent dock-fill dock-container">
        <div class="dock-fill dock-scroll">
            <asp:ObjectDataSource runat="server" ID="SpectrumDataSource" TypeName="Jhu.SpecSvc.Web.Search.ResultsDataSource"
                DataObjectTypeName="Jhu.SpecSvc.SpectrumLib.Spectrum" EnableViewState="true"
                EnablePaging="true" SelectMethod="GetResultsSpectra" SelectCountMethod="CountResultsSpectra"
                StartRowIndexParameterName="from" MaximumRowsParameterName="max" EnableCaching="false"
                OnObjectCreated="SpectrumDataSource_ObjectCreated" />
            <jgwc:MultiSelectListView runat="server" ID="SpectrumCards" DataSourceID="SpectrumDataSource"
                OnItemCreated="SpectrumCards_ItemCreated" DataKeyNames="ResultID" SelectionCheckboxID="spectrumItem"
                SelectionElementID="spectrumItem">
                <LayoutTemplate>
                    <asp:PlaceHolder runat="server" ID="groupPlaceholder" />
                </LayoutTemplate>
                <GroupTemplate>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                </GroupTemplate>
                <ItemTemplate>
                    <asp:PlaceHolder runat="server" ID="detailsPlaceholder" />
                </ItemTemplate>
            </jgwc:MultiSelectListView>
            <jgwc:MultiSelectListView runat="server" ID="SpectrumList" DataSourceID="SpectrumDataSource"
                OnItemCreated="SpectrumList_ItemCreated" DataKeyNames="ResultID" SelectionCheckboxID="spectrumItem"
                SelectionElementID="spectrumItem">
                <LayoutTemplate>
                    <table class="GridView" style="border-collapse: collapse">
                        <tr>
                            <th>
                            </th>
                            <th>
                            </th>
                            <th>
                                Object name
                            </th>
                            <th>
                                Class
                            </th>
                            <th>
                                Position
                            </th>
                            <th>
                                Redshift
                            </th>
                            <th>
                            </th>
                        </tr>
                        <asp:PlaceHolder runat="server" ID="groupPlaceholder" />
                    </table>
                </LayoutTemplate>
                <GroupTemplate>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                </GroupTemplate>
                <ItemTemplate>
                    <asp:PlaceHolder runat="server" ID="detailsPlaceholder" />
                </ItemTemplate>
            </jgwc:MultiSelectListView>
        </div>
    </div>
</asp:Content>
