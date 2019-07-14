﻿/*
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
using DotNetNuke.Entities.Modules;
using System.Data;
using System;
using DotNetNuke.Common.Utilities;
using System.Collections;
using System.Collections.Generic;

namespace helferlein.DNN.Modules.BabelFish.Business
{
   public class BabelFishInfo : IHydratable
   {
#region Constructors
      public BabelFishInfo() { }

      public BabelFishInfo(int portalID, string locale, string qualifier, string stringKey, string stringText, string stringComment)
      {
         PortalID = portalID;
         Locale = locale;
         Qualifier = qualifier;
         StringKey = stringKey;
         StringText = stringText;
         StringComment = stringComment;
      }
#endregion

#region Public Properties
      public int PortalID { get; set; }
      public int ID { get; set; }
      public string Locale { get; set; }
      public string Qualifier { get; set; }
      public string StringKey { get; set; }
      public string StringText { get; set; }
      public string StringComment { get; set; }
      public string FallBack { get; set; }
      public string FallBackComment { get; set; }

      public string DisplayValue
      {
         get { return (string.IsNullOrEmpty(StringText) ? FallBack : StringText); }
      }
#endregion

#region IHydratable Member
      public void Fill(IDataReader dr)
      {
         PortalID = Convert.ToInt32(dr["PortalID"]);
         ID = Convert.ToInt32(dr["ID"]);
         Locale = Convert.ToString(dr["Locale"]);
         Qualifier = Convert.ToString(dr["Qualifier"]);
         StringKey = Convert.ToString(dr["StringKey"]);
         StringText = Convert.ToString(Null.SetNull(dr["StringText"], StringText));
         StringComment = Convert.ToString(Null.SetNull(dr["StringComment"], StringComment));
         FallBack = Convert.ToString(Null.SetNull(dr["FallBack"], FallBack));
         FallBackComment = Convert.ToString(Null.SetNull(dr["FallBackComment"], FallBackComment));
      }

      public int KeyID
      {
         get { return ID; }
         set { ID = value; }
      }
#endregion
   }

   public class BabelFishComparer : IComparer<BabelFishInfo>
   {
      public int Compare(BabelFishInfo x, BabelFishInfo y)
      {
         return x.ID.CompareTo(y.ID);
      }

      public int CompareByDisplayValue(BabelFishInfo x, BabelFishInfo y)
      {
         return x.DisplayValue.CompareTo(y.DisplayValue);
      }

      public int CompareByStringKey(BabelFishInfo x, BabelFishInfo y)
      {
         return x.StringKey.CompareTo(y.StringKey);
      }
   }
}