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
using System.Globalization;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.UserControls;

namespace helferlein.DNN.Modules.BabelFish.UI
{
   public partial class LocaleSelector : PortalModuleBase
   {
#region Public Events
      public EventHandler SelectedIndexChanged;
      public event EventHandler UpdateRequested;
#endregion

#region Public Properties
      public string ProcessedLocale
      {
         get { return this.processedLocale; }
      }

      public string CurrentLocale
      {
         get { return this.LocalesDropDownList.SelectedValue; }
      }

      public string CurrentLocaleName
      {
         get { return this.LocalesDropDownList.SelectedItem.Text; }
      }

      public ListItemCollection Items
      {
         get { return this.LocalesDropDownList.Items; }
      }

      public bool DisabledLanguages
      {
         get { return this.DisabledLocalesCheckBox.Checked; }
         set { this.DisabledLocalesCheckBox.Checked = value; }
      }

      public bool AutoSave
      {
         get { return this.AutoSaveCheckBox.Checked; }
         set { this.AutoSaveCheckBox.Checked = value; }
      }

      public string ValidationGroup
      {
         get { return Convert.ToString(ViewState["ValidationGroup"]); }
         set
         {
            ViewState["ValidationGroup"] = value;
            this.LocalesDropDownList.ValidationGroup = value;
         }
      }

      public bool ShowDisabledLocales
      {
         get { return Convert.ToBoolean(ViewState["ShowDisabledLocales"]); }
         set
         {
            ViewState["ShowDisabledLocales"] = value;
            this.DisabledLocalesCheckBox.Visible = value;
            this.DisabledLocalesLabel.Visible = value;
         }
      }

      public bool ShowAutoSave
      {
         get { return Convert.ToBoolean(ViewState["ShowAutoSave"]); }
         set
         {
            ViewState["ShowAutoSave"] = value;
            this.AutoSaveCheckBox.Visible = value;
            this.AutoSaveLabel.Visible = value;
         }
      }

      public bool ShowUpdateRequestPanel
      {
         get
         {
            object o = ViewState["ShowUpdateRequestPanel"];
            if (o == null) return true;
            else return Convert.ToBoolean(o);
         }
         set { ViewState["ShowUpdateRequestPanel"] = value; }
      }

      public int Count
      {
         get
         {
            return this.LocalesDropDownList.Items.Count;
         }
      }

      public bool HideWhenOnlyOneLanguageIsAvailable
      {
         get
         {
            object o = ViewState["HideWhenOnlyOneLanguageIsAvailable"];
            if (o == null)
               return false;
            else
               return Convert.ToBoolean(o);
         }
         set { ViewState["HideWhenOnlyOneLanguageIsAvailable"] = value; }
      }
#endregion

#region Private Properties
      private LocaleController _localeController = null;
#endregion

#region Protected Properties
      protected string processedLocale
      {
         get { return Convert.ToString(ViewState["ProcessedLocale"]); }
         set { ViewState["ProcessedLocale"] = value; }
      }

      protected LocaleController LocaleController
      {
         get
         {
            if (_localeController == null)
               _localeController = new LocaleController();
            return _localeController;
         }
      }

      protected Dictionary<string, string> LocaleList
      {
         get
         {
            object o = base.ViewState["LocaleList"];
            if (o == null)
            {
               Dictionary<string, Locale> locales = this.LocaleController.GetLocales(this.DisabledLocalesCheckBox.Checked ? Null.NullInteger : base.PortalId);
               Dictionary<string, string> localeList = new Dictionary<string, string>();
               foreach (KeyValuePair<string, Locale> locale in locales)
               {
                  localeList.Add(locale.Key, locale.Value.Text);
               }
               base.ViewState["LocaleList"] = localeList;
            }
            return (Dictionary<string, string>)base.ViewState["LocaleList"];
         }
      }

      protected bool OnlyOneLanguageAvailable
      {
         get
         {
            object o = base.ViewState["OnlyOneLanguageAvailable"];
            bool result;
            if (o == null)
            {
               result = (this.LocaleController.GetLocales(Null.NullInteger).Count == 1);
            }
            else
            {
               result = Convert.ToBoolean(o);
            }
            base.ViewState["OnlyOneLanguageAvailable"] = result;
            return result;
         }
      }
#endregion

#region Event Handlers
      protected override void OnInit(EventArgs e)
      {
         this.InitializeComponent();
         base.OnInit(e);
      }

      protected void Page_Init(object sender, EventArgs e)
      {
         this.LocalResourceFile = base.ControlPath + "/App_LocalResources/CommonResources";
         this.DisabledLocalesCheckBox.CheckedChanged += new EventHandler(this.DisabledLocalesCheckBox_CheckedChanged);
         this.AutoSaveCheckBox.CheckedChanged += new EventHandler(this.AutoSaveCheckBox_CheckedChanged);
      }

      protected void Page_Load(object sender, EventArgs e)
      {
         if (!(Page.IsPostBack))
         {
            this.BindLocalesDropDownList();
            this.LocalesDropDownList.SelectedValue = CultureInfo.CurrentCulture.Name;
            this.processedLocale = this.CurrentLocale;
            if ((this.HideWhenOnlyOneLanguageIsAvailable) && (this.OnlyOneLanguageAvailable)) // (Localization.GetSupportedLocales().AllValues.Length == 1))
               this.Visible = false;
         }
      }

      protected void LocalesDropDownList_SelectedIndexChanged(object sender, EventArgs e)
      {
         this.OnSelectedIndexChanged(e);
         this.processedLocale = this.CurrentLocale;
      }

      protected void DisabledLocalesCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         base.ViewState.Remove("LocaleList");
         this.BindLocalesDropDownList();
         if (this.processedLocale != this.CurrentLocale)
         {
            this.OnSelectedIndexChanged(e);
            this.processedLocale = this.CurrentLocale;
         }
      }

      protected void AutoSaveCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         CheckBox autoSaveCheckBox = (CheckBox)sender;
         this.LocalesDropDownList.CausesValidation = autoSaveCheckBox.Checked;
         if (this.LocalesDropDownList.CausesValidation)
            this.LocalesDropDownList.ValidationGroup = this.ValidationGroup;
      }
#endregion

#region Public Methods
      public void Reset()
      {
         this.LocalesDropDownList.SelectedValue = base.PortalSettings.DefaultLanguage;
         this.DisabledLocalesCheckBox.Checked = false;
         this.AutoSaveCheckBox.Checked = false;
      }
#endregion

#region Private Methods
      private void InitializeComponent()
      {
         this.LocalesDropDownList.SelectedIndexChanged += new EventHandler(LocalesDropDownList_SelectedIndexChanged);
         this.Load += new EventHandler(this.Page_Load);
      }

      private void OnSelectedIndexChanged(EventArgs e)
      {
         this.CheckUpdateRequest(e);
         if (this.SelectedIndexChanged != null)
            this.SelectedIndexChanged(this, e);
      }

      private void BindLocalesDropDownList()
      {
         this.LocalesDropDownList.DataSource = this.LocaleList;
         this.LocalesDropDownList.DataTextField = "Value";
         this.LocalesDropDownList.DataValueField = "Key";
         this.LocalesDropDownList.DataBind();
         try
         {
            this.LocalesDropDownList.SelectedValue = this.ProcessedLocale;
         }
         catch
         {
            this.LocalesDropDownList.SelectedValue = base.PortalSettings.DefaultLanguage;
         }
      }

      private void CheckUpdateRequest(EventArgs e)
      {
         if ((this.AutoSaveCheckBox.Checked) && (this.UpdateRequested != null))
         {
            try
            {
               this.UpdateRequested(this, e);
               this.UpdateRequestImage.ImageUrl = "~/images/green-ok.gif";
               this.UpdateRequestLabel.Text = String.Format(Localization.GetString("UpdateRequestLabel.Success.Text", LocalResourceFile), this.ProcessedLocale);
            }
            catch (Exception ex)
            {
               this.UpdateRequestImage.ImageUrl = "~/images/red-error.gif";
               this.UpdateRequestLabel.Text = String.Format(Localization.GetString("UpdateRequestLabel.Failure.Text", LocalResourceFile), this.ProcessedLocale, ex.Message);
            }
            this.UpdateRequestPanel.Visible = this.ShowUpdateRequestPanel;
         }
      }
#endregion
   }
}
