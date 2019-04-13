<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocaleSelector.ascx.cs" Inherits="helferlein.DNN.Modules.BabelFish.UI.LocaleSelector" %>
<%@ Register Src="~/controls/labelcontrol.ascx" TagName="Label" TagPrefix="dnn" %>

<asp:Panel ID="UpdateRequestPanel" runat="server" Visible="false" EnableViewState="false">
   <asp:Image ID="UpdateRequestImage" runat="server" /><br />
   <asp:Label ID="UpdateRequestLabel" runat="server" CssClass="Normal" />
   <hr />
</asp:Panel>

<dnn:Label ID="LocalesLabel" runat="server"
   CssClass="SubHead"
   ControlName="LocalesLabel"
   Suffix=":"
   ResourceKey="LocalesLabel" />
<asp:DropDownList ID="LocalesDropDownList" runat="server"
   CssClass="Normal"
   AutoPostBack="true"
   CausesValidation="false" />
<br />

<asp:CheckBox ID="DisabledLocalesCheckBox" runat="server"
   CssClass="Normal"
   AutoPostBack="true"
   CausesValidation="false" />
<dnn:Label ID="DisabledLocalesLabel" runat="server"
   CssClass="Normal"
   ControlName="DisabledLocalesLabel"
   ResourceKey="DisabledLocalesLabel" />

<asp:CheckBox ID="AutoSaveCheckBox" runat="server"
   CssClass="Normal"
   AutoPostBack="true"
   CausesValidation="false" />
<dnn:Label ID="AutoSaveLabel" runat="server"
   CssClass="Normal"
   ControlName="AutoSaveLabel"
   ResourceKey="AutoSaveLabel" />

