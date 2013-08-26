<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Spectrum.master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="Jhu.SpecSvc.Web.Pipeline.List" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="middle" runat="server">
    <div class="dock-bottom">
        <p class="FormMessage">
            <%--<asp:CustomValidator ID="PipelineListValidator" runat="server" ErrorMessage="No spectrum was selected. Click on the checkboxes
            next to object names to select a spectrum." OnServerValidate="SpectrumSelected_ServerValidate" />--%></p>
        <p class="FormButtons">
            
        </p>
    </div>
    <div class="LayoutContent dock-fill dock-container">
        <div class="dock-fill dock-scroll">
            <jgwc:MultiSelectGridView runat="server" ID="PipelineList" DataKeyNames="ID">
            </jgwc:MultiSelectGridView>
        </div>
    </div>
</asp:Content>
