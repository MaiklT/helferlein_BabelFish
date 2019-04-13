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
using DotNetNuke.Entities.Modules;
using helferlein.DNN.Modules.BabelFish.Business;
using System.Globalization;
using System.Collections.Generic;
using DotNetNuke.Services.Localization;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace helferlein.DNN.Modules.BabelFish.UI
{
   public partial class BabelFishSelector : PortalModuleBase
   {
      public EventHandler SelectedIndexChanged;
      private BabelFishController babelFishController = null;

      protected BabelFishController BabelFishController
      {
         get
         {
            if (babelFishController == null)
               babelFishController = new BabelFishController();
            return babelFishController;
         }
      }

      public bool AutoPostBack
      {
         get { return this.BabelFishList.AutoPostBack; }
         set { this.BabelFishList.AutoPostBack = value; }
      }

      public string CssClass
      {
         get { return this.BabelFishList.CssClass; }
         set { this.BabelFishList.CssClass = value; }
      }

      public bool IncludeAll
      {
         get { return Convert.ToBoolean(ViewState["IncludeAll"]); }
         set { ViewState["IncludeAll"] = value; }
      }

      public bool IncludeOthers
      {
         get { return Convert.ToBoolean(ViewState["IncludeOthers"]); }
         set { ViewState["IncludeOthers"] = value; }
      }

      public bool IncludeSelect
      {
         get { return Convert.ToBoolean(ViewState["IncludeSelect"]); }
         set { ViewState["IncludeSelect"] = value; }
      }

      public string Qualifier
      {
         get { return Convert.ToString(ViewState["Qualifier"]); }
         set { ViewState["Qualifier"] = value; }
      }

      public int SelectedIndex
      {
         get { return this.BabelFishList.SelectedIndex; }
         set { this.BabelFishList.SelectedIndex = value; }
      }

      public string SelectedValue
      {
         get { return this.BabelFishList.SelectedValue; }
         set { this.BabelFishList.SelectedValue = value; }
      }

      public ListItem SelectedItem
      {
         get { return this.BabelFishList.SelectedItem; }
      }

      public ListItemCollection Items
      {
         get { return this.BabelFishList.Items; }
      }

      public string Filter
      {
         get { return Convert.ToString(ViewState["Filter"]); }
         set { ViewState["Filter"] = value; }
      }

      protected override void OnInit(EventArgs e)
      {
         this.InitializeComponent();
         base.OnInit(e);
      }

      protected void Page_Init(object sender, EventArgs e)
      {
         this.LocalResourceFile = base.ControlPath + "/App_LocalResources/CommonResources";
      }

      protected void Page_Load(object sender, EventArgs e)
      {
         if (!(Page.IsPostBack))
         {
            ListItem li;
            if (this.IncludeSelect)
            {
               li = new ListItem();
               li.Value = "";
               li.Text = Localization.GetString("Select.Text", LocalResourceFile);
               this.BabelFishList.Items.Add(li);
            }
            if (this.IncludeAll)
            {
               li = new ListItem();
               li.Value = "BABELFISH_SYSTEM_ITEM_ALL";
               li.Text = Localization.GetString("All.Text");
               this.BabelFishList.Items.Add(li);
            }
            List<BabelFishInfo> aquarium = this.BabelFishController.GetStrings(base.PortalId, CultureInfo.CurrentCulture.Name, this.Qualifier);
            if (!(String.IsNullOrEmpty(this.Filter)))
            {
               List<string> filtervalues = new List<string>();
               List<BabelFishInfo> filteredFish = new List<BabelFishInfo>();
               filtervalues.AddRange(this.Filter.Split(new char[] { ';' }));

               foreach (BabelFishInfo fish in aquarium)
               {
                  if (filtervalues.IndexOf(fish.StringKey) > -1)
                     filteredFish.Add(fish);
               }
               aquarium = filteredFish;
            }
            aquarium.Sort(new BabelFishComparer().CompareByDisplayValue);
            this.BabelFishList.DataSource = aquarium;
            this.BabelFishList.DataTextField = "DisplayValue";
            this.BabelFishList.DataValueField = "StringKey";
            this.BabelFishList.DataBind();
            this.SelectedValue = this.BabelFishList.SelectedValue;
            if (this.IncludeOthers)
            {
               li = new ListItem();
               li.Value = "BABELFISH_SYSTEM_ITEM_OTHERS";
               li.Text = Localization.GetString("Others.Text", LocalResourceFile);
               this.BabelFishList.Items.Add(li);
            }
         }
      }

      protected void BabelFishList_SelectedIndexChanged(object sender, EventArgs e)
      {
         this.OnSelectedIndexChanged(e);
      }

      private void InitializeComponent()
      {
         this.BabelFishList.SelectedIndexChanged += new EventHandler(this.BabelFishList_SelectedIndexChanged);
      }

      private void OnSelectedIndexChanged(EventArgs e)
      {
         if (this.SelectedIndexChanged != null)
            this.SelectedIndexChanged(this, e);
      }

   }
}
