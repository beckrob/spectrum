<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardButtons.ascx.cs"
    Inherits="Jhu.SpecSvc.Web.Controls.WizardButtons" %>
<asp:Button ID="Results" Text="<< Results" CssClass="FormButton" runat="server" Enabled="False"
    CommandName="results" OnCommand="Button_Command" CausesValidation="false" />
&nbsp;
<asp:Button ID="Back" Text="< Back" CssClass="FormButton" runat="server" Enabled="False"
    CommandName="back" OnCommand="Button_Command" CausesValidation="false" />
&nbsp;&nbsp;&nbsp;&nbsp;
<asp:Button ID="Ok" Text="Next >" CssClass="FormButton" runat="server" Enabled="False"
    CommandName="ok" OnCommand="Button_Command" CausesValidation="true" />
&nbsp;
<asp:Button ID="Finish" Text="Finish >>" CssClass="FormButton" runat="server" Enabled="False"
    CommandName="finish" OnCommand="Button_Command" CausesValidation="true" />
