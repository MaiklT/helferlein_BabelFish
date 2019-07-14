<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KeyResourceEditor.ascx.cs" Inherits="helferlein.DNN.Modules.BabelFish.Controls.KeyResourceEditor" %>

<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="dnn" %>
<%@ Register Src="~/controls/labelcontrol.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register Src="~/DesktopModules/helferlein_BabelFish/LocaleSelector.ascx" TagName="LocaleSelector" TagPrefix="helferlein" %>

<asp:Panel ID="GridPanel" runat="server">
   <asp:GridView ID="KeyResourcesGrid" runat="server" AutoGenerateColumns="false" Width="100%">
      <Columns>
         <asp:TemplateField>
            <ItemTemplate>
               <asp:ImageButton ID="EditButton" runat="server" ImageUrl="~/images/edit.gif" OnClick="EditButton_Click" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
         </asp:TemplateField>
         <asp:TemplateField>
            <ItemTemplate>
               <asp:ImageButton ID="DeleteButton" runat="server" ImageUrl="~/images/delete.gif" OnClick="DeleteButton_Click" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
         </asp:TemplateField>
         <asp:BoundField HeaderText="StringKey" DataField="StringKey" SortExpression="StringKey" />
         <asp:BoundField HeaderText="StringText" DataField="StringText" SortExpression="StringText" />
         <asp:BoundField HeaderText="FallBack" DataField="FallBack" SortExpression="FallBack" />
         <asp:BoundField HeaderText="DisplayValue" DataField="DisplayValue" SortExpression="DisplayValue" />
      </Columns>
      <EmptyDataTemplate>
         <asp:Label ID="EmptyDataLabel" runat="server" CssClass="NormalRed" Text="No data" />
      </EmptyDataTemplate>
   </asp:GridView>
   <br />
   <dnn:CommandButton ID="AddButton" runat="server"
      ImageUrl="~/images/add.gif"
      ResourceKey="Add"
      CausesValidation="false" />
   <dnn:CommandButton ID="BackButton" runat="server"
      ImageUrl="~/images/action_export.gif"
      ResourceKey="cmdBack"
      CausesValidation="false" />
</asp:Panel>
<asp:Panel ID="EditPanel" runat="server" Visible="false">
   <helferlein:LocaleSelector ID="LocaleSelector" runat="server" ValidationGroup="KeyResourceValidationGroup" />
   <br /><br />
   <dnn:Label ID="KeyLabel" runat="server"
      ControlName="KeyLabel"
      CssClass="SubHead"
      Suffix=":"
      ResourceKey="KeyLabel"
      Text="Key:" />
   <asp:TextBox ID="KeyTextBox" runat="server"
      CssClass="NormalTextBox"
      Width="10em" />
   <asp:RequiredFieldValidator ID="KeyValidator" runat="server"
      ControlToValidate="KeyTextBox"
      CssClass="NormalRed"
      Text="*"
      ValidationGroup="KeyResourceValidationGroup" />
   <asp:Image ID="KeyRequiredImage" runat="server"
      ImageUrl="~/images/required.gif" />
   <br /><br />
   <dnn:Label ID="KeyResourceLabel" runat="server"
      ControlName="KeyResourceLabel"
      CssClass="SubHead"
      Suffix=":"
      ResourceKey="KeyResourceLabel"
      Text="Event Type:" />
   <asp:TextBox ID="KeyResourceTextBox" runat="server"
      CssClass="NormalTextBox"
      MaxLength="100"
      Width="25em" />
   <asp:RequiredFieldValidator ID="KeyResourceValidator" runat="server"
      ControlToValidate="KeyResourceTextBox"
      CssClass="NormalRed"
      Text="*"
      ValidationGroup="KeyResourceValidationGroup" />
   <asp:Image ID="KeyResourceRequiredImage" runat="server"
      ImageUrl="~/images/required.gif" />
   <br /><br />
   <asp:Panel ID="KeyResourceCommentPanel" runat="server" Visible="false">
      <dnn:Label ID="KeyResourceCommentLabel" runat="server"
         ControlName="KeyResourceCommentLabel"
         CssClass="SubHead"
         ResourceKey="KeyResourceCommentLabel"
         Suffix=":"
         Text="Comment" />
      <asp:TextBox ID="KeyResourceCommentTextBox" runat="server"
         CssClass="NormalTextBox"
         Rows="5"
         TextMode="MultiLine"
         Width="30em" />
      <br /><br />
   </asp:Panel>
   <dnn:CommandButton ID="UpdateReturnButton" runat="server"
      ImageUrl="~/images/save.gif"
      ResourceKey="UpdateReturnButton"
      ValidationGroup="KeyResourceValidationGroup" />
   <dnn:CommandButton ID="UpdateButton" runat="server"
      ImageUrl="~/images/save.gif"
      ResourceKey="cmdUpdate"
      ValidationGroup="KeyResourceValidationGroup" />
   <dnn:CommandButton ID="CancelButton" runat="server"
      ImageUrl="~/images/cancel.gif"
      ResourceKey="cmdCancel"
      CausesValidation="false" />
   <asp:ValidationSummary ID="KeyResourceValidationSummary" runat="server"
      ShowMessageBox="true"
      ShowSummary="false"
      ValidationGroup="KeyResourceValidationGroup" />
</asp:Panel>
