<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BabelFish.ascx.cs" Inherits="helferlein.DNN.Modules.BabelFish.UI.BabelFish" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="helferlein" TagName="LocaleSelector" Src="Controls/LocaleSelector.ascx" %>
<%@ Register TagPrefix="helferlein" TagName="BabelFishSelector" Src="Controls/BabelFishSelector.ascx" %>
<%@ Register TagPrefix="helferlein" TagName="BabelFishMultiSelector" Src="Controls/BabelFishMultiSelector.ascx" %>

<div class="dnnFormMessage dnnFormInfo">
   <asp:Label ID="BabelFishLabel" runat="server" CssClass="Normal" />
</div>

<div class="dnnForm" ID="TestForm" runat="server">
   <h2><asp:Label ID="TestFormHeaderLabel" runat="server" ResourceKey="TestFormHeader" /></h2>
   <fieldset>
      <helferlein:LocaleSelector ID="LocaleSelector" runat="server"
         ShowDisabledLocales="True"
         ShowUpdateRequestPanel="True"
         AutoSave="False"
         DisabledLanguages="False"
         ShowAutoSave="True"
         HideWhenOnlyOneLanguageIsAvailable="False" />
      <div class="dnnFormItem">
         <dnn:Label ID="SelectorLabel" runat="server"
            ControlName="BabelFishSelector"
            Suffix=":"
            ResourceKey="BabelFishSelector" />
         <helferlein:BabelFishSelector ID="BabelFishSelector" runat="server"
            IsTest="True" />
      </div>
      <div class="dnnFormItem">
         <dnn:Label ID="MultiSelectorLabel" runat="server"
            ControlName="BabelFishMultiSelector"
            Suffix=":"
            ResourceKey="BabelFishMultiSelector" />
         <helferlein:BabelFishMultiSelector ID="BabelFishMultiSelector" runat="server"
            IsTest="True" />
      </div>
   </fieldset>
</div>
