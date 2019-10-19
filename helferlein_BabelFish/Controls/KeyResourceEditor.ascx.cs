/*
dnnWerk.at ( https://www.dnnwerk.at )
(C) Michael Tobisch 2009-2019

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions 
of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.
*/

using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using helferlein.DNN.Modules.BabelFish.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace helferlein.DNN.Modules.BabelFish.Controls
{
   public partial class KeyResourceEditor : PortalModuleBase
   {
      protected LocaleSelector LocaleSelector;

      private BabelFishController _babelFishController = null;
      private PortalSecurity _portalSecurity = null;

      protected BabelFishController BabelFishController
      {
         get
         {
            if (_babelFishController == null)
               _babelFishController = new BabelFishController();
            return _babelFishController;
         }
      }

      protected PortalSecurity PortalSecurity
      {
         get
         {
            if (_portalSecurity == null)
               _portalSecurity = new PortalSecurity();
            return _portalSecurity;
         }
      }

      public string BabelFishQualifier
      {
         get { return Convert.ToString(ViewState["BabelFishQualifier"]); }
         set { ViewState["BabelFishQualifier"] = value; }
      }

      public string DataItemName
      {
         get { return Convert.ToString(ViewState["DataItemName"]); }
         set { ViewState["DataItemName"] = value; }
      }

      public string ExternalResourceFile
      {
         get { return Convert.ToString(ViewState["ResourceFile"]); }
         set { ViewState["ResourceFile"] = value; }
      }

      public int MaxKeyLength
      {
         get
         {
            object o = ViewState["MaxKeyLength"];
            if (o == null) return 100;
            else return Math.Min(Convert.ToInt32(o), 100);
         }
         set { ViewState["MaxKeyLength"] = value; }
      }

      public bool IncludeComment
      {
         get
         {
            object o = ViewState["IncludeComment"];
            if (o == null) return false;
            else return Convert.ToBoolean(o);
         }
         set
         {
            ViewState["IncludeComment"] = value;
            KeyResourceCommentPanel.Visible = value;
         }
      }

      public bool AutoSave
      {
         get { return LocaleSelector.AutoSave; }
         set { LocaleSelector.AutoSave = value; }
      }

      public bool ShowAutoSave
      {
         get { return LocaleSelector.ShowAutoSave; }
         set { LocaleSelector.ShowAutoSave = value; }
      }

      public bool ShowDisabledLocaled
      {
         get { return LocaleSelector.ShowDisabledLocales; }
         set { LocaleSelector.ShowDisabledLocales = value; }
      }

      public bool ShowUpdateRequestPanel
      {
         get { return LocaleSelector.ShowUpdateRequestPanel; }
         set { LocaleSelector.ShowUpdateRequestPanel = value; }
      }

      private string CurrentKey
      {
         get { return Convert.ToString(ViewState["CurrentKey"]); }
         set { ViewState["CurrentKey"] = value; }
      }

      protected void Page_Init(object sender, EventArgs e)
      {
         LocalResourceFile = string.Format("{0}/App_LocalResources/CommonResources", ControlPath);
         KeyResourcesGrid.RowDataBound += new GridViewRowEventHandler(KeyResourcesGrid_RowDataBound);
         AddButton.Click += new EventHandler(AddButton_Click);
         BackButton.Click += new EventHandler(BackButton_Click);
         UpdateReturnButton.Click += new EventHandler(UpdateReturnButton_Click);
         UpdateButton.Click += new EventHandler(UpdateButton_Click);
         CancelButton.Click += new EventHandler(CancelButton_Click);
         LocaleSelector.SelectedIndexChanged += new EventHandler(LocaleSelector_SelectedIndexChanged);
         LocaleSelector.UpdateRequested += new EventHandler(LocaleSelector_UpdateRequested);
      }

      protected void Page_Load(object sender, EventArgs e)
      {
         if (!(Page.IsPostBack))
         {
            BindGrid();

            KeyTextBox.MaxLength = MaxKeyLength;
            KeyTextBox.Width = Unit.Parse(Convert.ToString(Math.Min(Convert.ToInt32(MaxKeyLength), 20)) + "em");

            SetErrorMessages();
         }
      }

      protected void KeyResourcesGrid_RowDataBound(object sender, GridViewRowEventArgs e)
      {
         GridView keyResourcesGrid = (GridView)sender;
         switch (e.Row.RowType)
         {
            case DataControlRowType.Header:
               foreach (TableCell tc in e.Row.Cells)
                  if (!(string.IsNullOrEmpty(tc.Text)))
                     tc.Text = Localization.GetString(tc.Text + ".Header", LocalResourceFile);
               break;
            case DataControlRowType.DataRow:
               ImageButton editButton = (ImageButton)e.Row.FindControl("EditButton");
               editButton.AlternateText = Localization.GetString("cmdEdit.Text");
               editButton.CommandArgument = ((DataRowView)e.Row.DataItem)["StringKey"].ToString();
               ImageButton deleteButton = (ImageButton)e.Row.FindControl("DeleteButton");
               deleteButton.AlternateText = Localization.GetString("cmdDelete.Text");
               deleteButton.CommandArgument = ((DataRowView)e.Row.DataItem)["StringKey"].ToString();
               deleteButton.OnClientClick = "return confirm('" + string.Format(Localization.GetString("ConfirmDelete.Text"), ((DataRowView)e.Row.DataItem)["StringKey"].ToString().Replace("'", @"\'")) + "');";
               break;
            case DataControlRowType.EmptyDataRow:
               Label emptyDataLabel = (Label)e.Row.FindControl("EmptyDataLabel");
               string dataItemName;
               if ((!(string.IsNullOrEmpty(ExternalResourceFile))) && (!(string.IsNullOrEmpty(Localization.GetString(DataItemName + ".Text", ExternalResourceFile)))))
                  dataItemName = Localization.GetString(DataItemName + ".Text", ExternalResourceFile);
               else
                  dataItemName = DataItemName;
               emptyDataLabel.Text = string.Format(Localization.GetString("EmptyDataLabel.Text", LocalResourceFile), dataItemName.ToLower(), Localization.GetString("Add"));
               break;
            default:
               break;
         }
      }

      protected void EditButton_Click(object sender, EventArgs e)
      {
         ImageButton editButton = (ImageButton)sender;
         CurrentKey = editButton.CommandArgument;
         SetMode("Edit");
         SetValues();
      }

      protected void LocaleSelector_SelectedIndexChanged(object sender, EventArgs e)
      {
         SetValues();
      }

      protected void LocaleSelector_UpdateRequested(object sender, EventArgs e)
      {
         LocaleSelector localeSelector = (LocaleSelector)sender;
         Update(localeSelector.ProcessedLocale);
      }

      protected void AddButton_Click(object sender, EventArgs e)
      {
         CurrentKey = string.Empty;
         SetMode("Edit");
         SetValues();
      }

      protected void DeleteButton_Click(object sender, EventArgs e)
      {
         ImageButton deleteButton = (ImageButton)sender;
         BabelFishController.DropKey(PortalId, BabelFishQualifier, deleteButton.CommandArgument);
         BindGrid();
      }

      protected void BackButton_Click(object sender, EventArgs e)
      {
         Response.Redirect(Globals.NavigateURL());
      }

      protected void UpdateReturnButton_Click(object sender, EventArgs e)
      {
         if (Update(LocaleSelector.ProcessedLocale))
         {
            CurrentKey = string.Empty;
            SetMode("Grid");
         }
      }

      protected void UpdateButton_Click(object sender, EventArgs e)
      {
         Update(LocaleSelector.ProcessedLocale);
      }

      protected void CancelButton_Click(object sender, EventArgs e)
      {
         CurrentKey = string.Empty;
         SetMode("Grid");
      }

      private void BindGrid()
      {
         List<BabelFishInfo> keys = BabelFishController.GetStrings(PortalId, CultureInfo.CurrentCulture.Name, BabelFishQualifier);

         DataTable values = new DataTable();
         DataColumn keyColumn = new DataColumn("StringKey", typeof(string));
         values.Columns.Add(keyColumn);
         DataColumn valueColumn = new DataColumn("StringText", typeof(string));
         values.Columns.Add(valueColumn);
         DataColumn fallbackColumn = new DataColumn("FallBack", typeof(string));
         values.Columns.Add(fallbackColumn);
         DataColumn displayValueColumn = new DataColumn("DisplayValue", typeof(string));
         values.Columns.Add(displayValueColumn);

         foreach (BabelFishInfo k in keys)
         {
            DataRow row = values.NewRow();
            row["StringKey"] = k.StringKey;
            row["StringText"] = k.StringText;
            row["FallBack"] = k.FallBack;
            row["DisplayValue"] = k.DisplayValue;
            values.Rows.Add(row);
         }
         KeyResourcesGrid.DataSource = values;
         // KeyResourcesGrid.Sort("DisplayValue", SortDirection.Ascending);
         KeyResourcesGrid.DataBind();
      }

      private void SetMode(string mode)
      {
         if (mode == "Grid")
         {
            GridPanel.Visible = true;
            LocaleSelector.Reset();
            EditPanel.Visible = false;
            BindGrid();
         }
         else
         {
            GridPanel.Visible = false;
            EditPanel.Visible = true;
         }
      }

      private void SetValues()
      {
         if (!(string.IsNullOrEmpty(CurrentKey)))
         {
            // We're in edit mode
            BabelFishInfo babelFish = BabelFishController.GetString(PortalId, LocaleSelector.CurrentLocale, BabelFishQualifier, CurrentKey);
            KeyTextBox.Text = CurrentKey;
            KeyTextBox.Enabled = false;
            if (babelFish == null)
            {
               KeyResourceTextBox.Text = string.Empty;
               KeyResourceCommentTextBox.Text = string.Empty;
            }
            else
            {
               KeyResourceTextBox.Text = babelFish.StringText;
               KeyResourceCommentTextBox.Text = babelFish.StringComment;
            }
         }
         else
         {
            // We're in append mode
            KeyTextBox.Text = string.Empty;
            KeyTextBox.Enabled = true;
            KeyResourceTextBox.Text = string.Empty;
            KeyResourceCommentTextBox.Text = string.Empty;
         }
         KeyRequiredImage.AlternateText = Localization.GetString("Required.Text");
         KeyResourceRequiredImage.AlternateText = Localization.GetString("Required.Text");
      }

      private void SetErrorMessages()
      {
         KeyValidator.ErrorMessage = Localization.GetString("KeyValidator.ErrorMessage", LocalResourceFile);
         KeyResourceValidator.ErrorMessage = Localization.GetString("KeyResourceValidator.ErrorMessage", LocalResourceFile);
      }

      private bool Update(string locale)
      {
         bool returnValue = false;
         try
         {
            Page.Validate();
            if (Page.IsValid)
            {
               if (KeyTextBox.Enabled)
                  BabelFishController.Add(PortalId, locale, BabelFishQualifier, KeyTextBox.Text, KeyResourceTextBox.Text, PortalSecurity.InputFilter(KeyResourceCommentTextBox.Text, PortalSecurity.FilterFlag.MultiLine | PortalSecurity.FilterFlag.NoMarkup));
               else
                  BabelFishController.Change(PortalId, locale, BabelFishQualifier, KeyTextBox.Text, KeyResourceTextBox.Text, PortalSecurity.InputFilter(KeyResourceCommentTextBox.Text, PortalSecurity.FilterFlag.MultiLine | PortalSecurity.FilterFlag.NoMarkup));
               CurrentKey = KeyTextBox.Text;
               KeyTextBox.Enabled = false;
               returnValue = true;
            }
         }
         catch (Exception ex)
         {
            Exceptions.ProcessModuleLoadException(this, ex);
         }
         return returnValue;
      }
   }
}
