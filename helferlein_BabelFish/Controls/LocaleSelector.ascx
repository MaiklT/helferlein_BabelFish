<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocaleSelector.ascx.cs" Inherits="helferlein.DNN.Modules.BabelFish.Controls.LocaleSelector" %>
<%@ Register Src="~/controls/labelcontrol.ascx" TagName="Label" TagPrefix="dnn" %>

<div class="dnnFormItem">
   <asp:Panel ID="UpdateRequestPanel" runat="server" Visible="false" EnableViewState="false">
      <asp:Image ID="UpdateRequestImage" runat="server" /><br />
      <asp:Label ID="UpdateRequestLabel" runat="server" CssClass="Normal" />
      <hr />
   </asp:Panel>
</div>

<div class="dnnFormItem">
   <dnn:Label ID="LocalesLabel" runat="server"
      ControlName="LocalesDropDownList"
      Suffix=":"
      ResourceKey="LocalesLabel" />
   <asp:DropDownList ID="LocalesDropDownList" runat="server"
      AutoPostBack="true"
      CausesValidation="false" />
</div>

<div class="dnnFormItem" id="DisabledLocalesPanel" runat="server">
   <dnn:Label ID="DisabledLocalesLabel" runat="server"
      ControlName="DisabledLocalesCheckBox"
      Suffix="?"
      ResourceKey="DisabledLocalesLabel" />
   <asp:CheckBox ID="DisabledLocalesCheckBox" runat="server"
      AutoPostBack="true"
      CausesValidation="false" />
</div>

<div class="dnnFormItem" id="AutoSavePanel" runat="server">
   <dnn:Label ID="AutoSaveLabel" runat="server"
      ControlName="AutoSaveCheckBox"
      Suffix="?"
      ResourceKey="AutoSaveLabel" />
   <asp:CheckBox ID="AutoSaveCheckBox" runat="server"
      AutoPostBack="true"
      CausesValidation="false" />
</div>
