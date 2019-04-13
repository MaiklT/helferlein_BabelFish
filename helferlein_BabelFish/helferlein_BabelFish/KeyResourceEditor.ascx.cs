/*
helferlein.com ( http://www.helferlein.com )
Michael Tobisch

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

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using helferlein.DNN.Modules.BabelFish.Business;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Security;

namespace helferlein.DNN.Modules.BabelFish.UI
{
   public partial class KeyResourceEditor : PortalModuleBase
   {
      protected global::helferlein.DNN.Modules.BabelFish.UI.LocaleSelector LocaleSelector;

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
            object o = base.ViewState["IncludeComment"];
            if (o == null) return false;
            else return Convert.ToBoolean(o);
         }
         set
         {
            base.ViewState["IncludeComment"] = value;
            this.KeyResourceCommentPanel.Visible = value;
         }
      }

      public bool AutoSave
      {
         get { return this.LocaleSelector.AutoSave; }
         set { this.LocaleSelector.AutoSave = value; }
      }

      public bool ShowAutoSave
      {
         get { return this.LocaleSelector.ShowAutoSave; }
         set { this.LocaleSelector.ShowAutoSave = value; }
      }

      public bool ShowDisabledLocaled
      {
         get { return this.LocaleSelector.ShowDisabledLocales; }
         set { this.LocaleSelector.ShowDisabledLocales = value; }
      }

      public bool ShowUpdateRequestPanel
      {
         get { return this.LocaleSelector.ShowUpdateRequestPanel; }
         set { this.LocaleSelector.ShowUpdateRequestPanel = value; }
      }

      private string CurrentKey
      {
         get { return Convert.ToString(ViewState["CurrentKey"]); }
         set { ViewState["CurrentKey"] = value; }
      }

      protected void Page_Init(object sender, EventArgs e)
      {
         this.LocalResourceFile = String.Format("{0}/App_LocalResources/CommonResources", base.ControlPath);
         this.KeyResourcesGrid.RowDataBound += new GridViewRowEventHandler(this.KeyResourcesGrid_RowDataBound);
         this.AddButton.Click += new EventHandler(this.AddButton_Click);
         this.BackButton.Click += new EventHandler(this.BackButton_Click);
         this.UpdateReturnButton.Click += new EventHandler(this.UpdateReturnButton_Click);
         this.UpdateButton.Click += new EventHandler(this.UpdateButton_Click);
         this.CancelButton.Click += new EventHandler(this.CancelButton_Click);
         this.LocaleSelector.SelectedIndexChanged += new EventHandler(LocaleSelector_SelectedIndexChanged);
         this.LocaleSelector.UpdateRequested += new EventHandler(LocaleSelector_UpdateRequested);
      }

      protected void Page_Load(object sender, EventArgs e)
      {
         if (!(Page.IsPostBack))
         {
            this.BindGrid();

            this.KeyTextBox.MaxLength = this.MaxKeyLength;
            this.KeyTextBox.Width = Unit.Parse(Convert.ToString(Math.Min(Convert.ToInt32(this.MaxKeyLength), 20)) + "em");

            this.SetErrorMessages();
         }
      }

      protected void KeyResourcesGrid_RowDataBound(object sender, GridViewRowEventArgs e)
      {
         GridView keyResourcesGrid = (GridView)sender;
         switch (e.Row.RowType)
         {
            case DataControlRowType.Header:
               foreach (TableCell tc in e.Row.Cells)
                  if (!(String.IsNullOrEmpty(tc.Text)))
                     tc.Text = Localization.GetString(tc.Text + ".Header", LocalResourceFile);
               break;
            case DataControlRowType.DataRow:
               ImageButton editButton = (ImageButton)e.Row.FindControl("EditButton");
               editButton.AlternateText = Localization.GetString("cmdEdit.Text");
               editButton.CommandArgument = ((DataRowView)e.Row.DataItem)["StringKey"].ToString();
               ImageButton deleteButton = (ImageButton)e.Row.FindControl("DeleteButton");
               deleteButton.AlternateText = Localization.GetString("cmdDelete.Text");
               deleteButton.CommandArgument = ((DataRowView)e.Row.DataItem)["StringKey"].ToString();
               deleteButton.OnClientClick = "return confirm('" + String.Format(Localization.GetString("ConfirmDelete.Text", BabelfishBase.CommonResourceFile), ((DataRowView)e.Row.DataItem)["StringKey"].ToString().Replace("'", @"\'")) + "');";
               break;
            case DataControlRowType.EmptyDataRow:
               Label emptyDataLabel = (Label)e.Row.FindControl("EmptyDataLabel");
               string dataItemName;
               if ((!(String.IsNullOrEmpty(this.ExternalResourceFile))) && (!(String.IsNullOrEmpty(Localization.GetString(this.DataItemName + ".Text", this.ExternalResourceFile)))))
                  dataItemName = Localization.GetString(this.DataItemName + ".Text", this.ExternalResourceFile);
               else
                  dataItemName = this.DataItemName;
               emptyDataLabel.Text = String.Format(Localization.GetString("EmptyDataLabel.Text", LocalResourceFile), dataItemName.ToLower(), Localization.GetString("Add"));
               break;
            default:
               break;
         }
      }

      protected void EditButton_Click(object sender, EventArgs e)
      {
         ImageButton editButton = (ImageButton)sender;
         this.CurrentKey = editButton.CommandArgument;
         this.SetMode("Edit");
         this.SetValues();
      }

      protected void LocaleSelector_SelectedIndexChanged(object sender, EventArgs e)
      {
         this.SetValues();
      }

      protected void LocaleSelector_UpdateRequested(object sender, EventArgs e)
      {
         LocaleSelector localeSelector = (LocaleSelector)sender;
         this.Update(localeSelector.ProcessedLocale);
      }

      protected void AddButton_Click(object sender, EventArgs e)
      {
         this.CurrentKey = String.Empty;
         this.SetMode("Edit");
         this.SetValues();
      }

      protected void DeleteButton_Click(object sender, EventArgs e)
      {
         ImageButton deleteButton = (ImageButton)sender;
         this.BabelFishController.DropKey(base.PortalId, this.BabelFishQualifier, deleteButton.CommandArgument);
         this.BindGrid();
      }

      protected void BackButton_Click(object sender, EventArgs e)
      {
         Response.Redirect(Globals.NavigateURL());
      }

      protected void UpdateReturnButton_Click(object sender, EventArgs e)
      {
         if (this.Update(this.LocaleSelector.ProcessedLocale))
         {
            this.CurrentKey = String.Empty;
            this.SetMode("Grid");
         }
      }

      protected void UpdateButton_Click(object sender, EventArgs e)
      {
         this.Update(this.LocaleSelector.ProcessedLocale);
      }

      protected void CancelButton_Click(object sender, EventArgs e)
      {
         this.CurrentKey = String.Empty;
         this.SetMode("Grid");
      }

      private void BindGrid()
      {
         List<BabelFishInfo> keys = this.BabelFishController.GetStrings(base.PortalId, CultureInfo.CurrentCulture.Name, this.BabelFishQualifier, false);

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
         this.KeyResourcesGrid.DataSource = values;
         // this.KeyResourcesGrid.Sort("DisplayValue", SortDirection.Ascending);
         this.KeyResourcesGrid.DataBind();
      }

      private void SetMode(string mode)
      {
         if (mode == "Grid")
         {
            this.GridPanel.Visible = true;
            this.LocaleSelector.Reset();
            this.EditPanel.Visible = false;
            this.BindGrid();
         }
         else
         {
            this.GridPanel.Visible = false;
            this.EditPanel.Visible = true;
         }
      }

      private void SetValues()
      {
         if (!(String.IsNullOrEmpty(this.CurrentKey)))
         {
            // We're in edit mode
            BabelFishInfo babelFish = this.BabelFishController.GetString(base.PortalId, this.LocaleSelector.CurrentLocale, this.BabelFishQualifier, this.CurrentKey);
            this.KeyTextBox.Text = this.CurrentKey;
            this.KeyTextBox.Enabled = false;
            if (babelFish == null)
            {
               this.KeyResourceTextBox.Text = String.Empty;
               this.KeyResourceCommentTextBox.Text = String.Empty;
            }
            else
            {
               this.KeyResourceTextBox.Text = babelFish.StringText;
               this.KeyResourceCommentTextBox.Text = babelFish.StringComment;
            }
         }
         else
         {
            // We're in append mode
            this.KeyTextBox.Text = String.Empty;
            this.KeyTextBox.Enabled = true;
            this.KeyResourceTextBox.Text = String.Empty;
            this.KeyResourceCommentTextBox.Text = String.Empty;
         }
         this.KeyRequiredImage.AlternateText = Localization.GetString("Required.Text");
         this.KeyResourceRequiredImage.AlternateText = Localization.GetString("Required.Text");
      }

      private void SetErrorMessages()
      {
         this.KeyValidator.ErrorMessage = Localization.GetString("KeyValidator.ErrorMessage", LocalResourceFile);
         this.KeyResourceValidator.ErrorMessage = Localization.GetString("KeyResourceValidator.ErrorMessage", LocalResourceFile);
      }

      private bool Update(string locale)
      {
         bool returnValue = false;
         try
         {
            Page.Validate();
            if (Page.IsValid)
            {
               if (this.KeyTextBox.Enabled)
                  this.BabelFishController.Add(this.PortalId, locale, this.BabelFishQualifier, this.KeyTextBox.Text, this.KeyResourceTextBox.Text, this.PortalSecurity.InputFilter(this.KeyResourceCommentTextBox.Text, PortalSecurity.FilterFlag.MultiLine | PortalSecurity.FilterFlag.NoMarkup));
               else
                  this.BabelFishController.Change(this.PortalId, locale, this.BabelFishQualifier, this.KeyTextBox.Text, this.KeyResourceTextBox.Text, this.PortalSecurity.InputFilter(this.KeyResourceCommentTextBox.Text, PortalSecurity.FilterFlag.MultiLine | PortalSecurity.FilterFlag.NoMarkup));
               this.CurrentKey = this.KeyTextBox.Text;
               this.KeyTextBox.Enabled = false;
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
