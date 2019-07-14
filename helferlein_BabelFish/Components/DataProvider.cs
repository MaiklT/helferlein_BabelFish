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
using System.Data;
using DotNetNuke.Framework;

namespace helferlein.DNN.Modules.BabelFish.Data
{
   public abstract class DataProvider
   {
      static DataProvider provider = null;
      public static DataProvider Instance()
      {
         if (provider == null)
            provider = (DataProvider)Reflection.CreateObject("data", "helferlein.DNN.Modules.BabelFish.Data", "");
         return provider;
      }
#region BabelFish Methods
      public abstract IDataReader GetQualifiers(int portalID);
      public abstract IDataReader GetString(int ID);
      public abstract IDataReader GetString(int portalID, string locale, string qualifier, string stringKey, string fallBackLocale);
      public abstract IDataReader GetStrings(int portalID, string locale, string qualifier, string fallBackLocale);
      public abstract IDataReader GetStrings(int portalID, string qualifier, string fallBackLocale);
      public abstract IDataReader GetStringsByKey(int portalID, string qualifier, string stringKey, string fallBackLocale);
      public abstract IDataReader GetNonFallBackStrings(int portalID, string qualifier, string stringKey, string fallBackLocale);
      public abstract int InsertString(int portalID, string locale, string qualifier, string stringKey, string stringText, string stringComment);
      public abstract int UpdateString(int ID, string stringText, string stringComment);
      public abstract int UpdateString(int portalID, string locale, string qualifier, string stringKey, string stringText, string stringComment);
      public abstract int UpdateStrings(string aquarium);
      public abstract void DeleteString(int ID);
      public abstract void DeleteStringKey(int portalID, string qualifier, string stringKey);
      public abstract void DeleteLocale(int portalID, string locale);
      public abstract void DeletePortal(int PortalID);
      public abstract void DeleteQualifier(int portalID, string qualifier);
      public abstract void CopyQualifier(int sourcePortalID, int targetPortalID, string qualifier);
      public abstract void CopyQualifier(int sourcePortalID, int targetPortalID, string locale, string qualifier);
#endregion
   }
}