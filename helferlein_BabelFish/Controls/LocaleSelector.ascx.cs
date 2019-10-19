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

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace helferlein.DNN.Modules.BabelFish.Controls
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
         get { return processedLocale; }
      }

      public string CurrentLocale
      {
         get { return LocalesDropDownList.SelectedValue; }
      }

      public string CurrentLocaleName
      {
         get { return LocalesDropDownList.SelectedItem.Text; }
      }

      public ListItemCollection Items
      {
         get { return LocalesDropDownList.Items; }
      }

      public bool DisabledLanguages
      {
         get { return DisabledLocalesCheckBox.Checked; }
         set { DisabledLocalesCheckBox.Checked = value; }
      }

      public bool AutoSave
      {
         get { return AutoSaveCheckBox.Checked; }
         set { AutoSaveCheckBox.Checked = value; }
      }

      public string ValidationGroup
      {
         get { return Convert.ToString(ViewState["ValidationGroup"]); }
         set
         {
            ViewState["ValidationGroup"] = value;
            LocalesDropDownList.ValidationGroup = value;
         }
      }

      public bool ShowDisabledLocales
      {
         get { return Convert.ToBoolean(ViewState["ShowDisabledLocales"]); }
         set
         {
            ViewState["ShowDisabledLocales"] = value;
            DisabledLocalesPanel.Visible = value;
         }
      }

      public bool ShowAutoSave
      {
         get { return Convert.ToBoolean(ViewState["ShowAutoSave"]); }
         set
         {
            ViewState["ShowAutoSave"] = value;
            AutoSavePanel.Visible = value;
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
            return LocalesDropDownList.Items.Count;
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
            object o = ViewState["LocaleList"];
            if (o == null)
            {
               Dictionary<string, Locale> locales = LocaleController.GetLocales(DisabledLocalesCheckBox.Checked ? Null.NullInteger : PortalId);
               Dictionary<string, string> localeList = new Dictionary<string, string>();
               foreach (KeyValuePair<string, Locale> locale in locales)
               {
                  localeList.Add(locale.Key, locale.Value.Text);
               }
               ViewState["LocaleList"] = localeList;
            }
            return (Dictionary<string, string>)ViewState["LocaleList"];
         }
      }

      protected bool OnlyOneLanguageAvailable
      {
         get
         {
            object o = ViewState["OnlyOneLanguageAvailable"];
            bool result;
            if (o == null)
            {
               result = (LocaleController.GetLocales(Null.NullInteger).Count == 1);
            }
            else
            {
               result = Convert.ToBoolean(o);
            }
            ViewState["OnlyOneLanguageAvailable"] = result;
            return result;
         }
      }
#endregion

#region Event Handlers
      protected override void OnInit(EventArgs e)
      {
         InitializeComponent();
         base.OnInit(e);
      }

      protected void Page_Init(object sender, EventArgs e)
      {
         LocalResourceFile = string.Format("/DesktopModules/{0}/{1}/{2}", DesktopModuleController.GetDesktopModuleByFriendlyName("helferlein BabelFish").FolderName, "App_LocalResources", "SharedResources");
         DisabledLocalesCheckBox.CheckedChanged += new EventHandler(DisabledLocalesCheckBox_CheckedChanged);
         AutoSaveCheckBox.CheckedChanged += new EventHandler(AutoSaveCheckBox_CheckedChanged);
      }

      protected void Page_Load(object sender, EventArgs e)
      {
         if (!(Page.IsPostBack))
         {
            BindLocalesDropDownList();
            LocalesDropDownList.SelectedValue = CultureInfo.CurrentCulture.Name;
            processedLocale = CurrentLocale;
            if ((HideWhenOnlyOneLanguageIsAvailable) && (OnlyOneLanguageAvailable)) // (Localization.GetSupportedLocales().AllValues.Length == 1))
               Visible = false;
         }
      }

      protected void LocalesDropDownList_SelectedIndexChanged(object sender, EventArgs e)
      {
         OnSelectedIndexChanged(e);
         processedLocale = CurrentLocale;
      }

      protected void DisabledLocalesCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         ViewState.Remove("LocaleList");
         BindLocalesDropDownList();
         if (processedLocale != CurrentLocale)
         {
            OnSelectedIndexChanged(e);
            processedLocale = CurrentLocale;
         }
      }

      protected void AutoSaveCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         CheckBox autoSaveCheckBox = (CheckBox)sender;
         LocalesDropDownList.CausesValidation = autoSaveCheckBox.Checked;
         if (LocalesDropDownList.CausesValidation)
            LocalesDropDownList.ValidationGroup = ValidationGroup;
      }
#endregion

#region Public Methods
      public void Reset()
      {
         LocalesDropDownList.SelectedValue = PortalSettings.DefaultLanguage;
         DisabledLocalesCheckBox.Checked = ShowDisabledLocales;
         AutoSaveCheckBox.Checked = false;
      }
#endregion

#region Private Methods
      private void InitializeComponent()
      {
         LocalesDropDownList.SelectedIndexChanged += new EventHandler(LocalesDropDownList_SelectedIndexChanged);
         Load += new EventHandler(Page_Load);
      }

      private void OnSelectedIndexChanged(EventArgs e)
      {
         CheckUpdateRequest(e);
         if (SelectedIndexChanged != null)
            SelectedIndexChanged(this, e);
      }

      private void BindLocalesDropDownList()
      {
         LocalesDropDownList.DataSource = LocaleList;
         LocalesDropDownList.DataTextField = "Value";
         LocalesDropDownList.DataValueField = "Key";
         LocalesDropDownList.DataBind();
         try
         {
            LocalesDropDownList.SelectedValue = ProcessedLocale;
         }
         catch
         {
            LocalesDropDownList.SelectedValue = PortalSettings.DefaultLanguage;
         }
      }

      private void CheckUpdateRequest(EventArgs e)
      {
         if ((AutoSaveCheckBox.Checked) && (UpdateRequested != null))
         {
            try
            {
               UpdateRequested(this, e);
               UpdateRequestImage.ImageUrl = "~/images/green-ok.gif";
               UpdateRequestLabel.Text = string.Format(Localization.GetString("UpdateRequestLabel.Success.Text", LocalResourceFile), ProcessedLocale);
            }
            catch (Exception ex)
            {
               UpdateRequestImage.ImageUrl = "~/images/red-error.gif";
               UpdateRequestLabel.Text = string.Format(Localization.GetString("UpdateRequestLabel.Failure.Text", LocalResourceFile), ProcessedLocale, ex.Message);
            }
            UpdateRequestPanel.Visible = ShowUpdateRequestPanel;
         }
      }
#endregion
   }
}
