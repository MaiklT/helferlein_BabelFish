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
using helferlein.DNN.Modules.BabelFish.Data;
using System.Collections.Generic;
using System;
using DotNetNuke.Entities.Modules;
using System.Web;
using DotNetNuke.Entities.Portals;
using System.Text;
using DotNetNuke.Data;
using DataProvider = helferlein.DNN.Modules.BabelFish.Data.DataProvider;
using System.Data;

namespace helferlein.DNN.Modules.BabelFish.Business
{
   public class BabelFishController
   {
      /// <summary>
      /// Gets the fallback locale (which is the portal's default language)
      /// </summary>
      public string FallBackLocale
      {
         get { return PortalController.Instance.GetCurrentPortalSettings().DefaultLanguage; }
      }
#region Data Access
      /// <summary>
      /// Gets a BabelFishInfo object by it's ID
      /// </summary>
      /// <param name="ID">The ID of the BabelFishInfo object</param>
      /// <returns>BabelFishInfo object from the database (not cached)</returns>
      public BabelFishInfo GetString(int id)
      {
         BabelFishInfo babelFish;
         using (IDataContext ctx = DataContext.Instance())
         {
            var rep = ctx.GetRepository<BabelFishInfo>();
            babelFish = rep.GetById(id);
         }
         return babelFish;
      }

      public BabelFishInfo GetString(int portalID, string locale, string qualifier, string stringKey)
      {
         BabelFishInfo babelFish;
         using (IDataContext ctx = DataContext.Instance())
         {
            babelFish = (BabelFishInfo)ctx.ExecuteQuery<BabelFishInfo>(CommandType.StoredProcedure,
               "{databaseOwner}{objectQualifier}helferlein_BabelFish_GetString",
               new object[] { portalID, locale, qualifier, stringKey, FallBackLocale });
         }
         return babelFish;
      }

      public List<BabelFishInfo> GetStrings(int portalID, string locale, string qualifier)
      {
         List<BabelFishInfo> aquarium;
         using (IDataContext ctx = DataContext.Instance())
         {
            aquarium = (List<BabelFishInfo>)ctx.ExecuteQuery<BabelFishInfo>(CommandType.StoredProcedure,
               "{databaseOwner}{objectQualifier}helferlein_BabelFish_GetStrings",
               new object[] { portalID, locale, qualifier, FallBackLocale });
         }
         return aquarium;
      }

      public List<BabelFishInfo> GetStrings(int portalID, string qualifier)
      {
         List<BabelFishInfo> aquarium;
         using (IDataContext ctx = DataContext.Instance())
         {
            aquarium = (List<BabelFishInfo>)ctx.ExecuteQuery<BabelFishInfo>(CommandType.StoredProcedure,
               "{databaseOwner}{objectQualifier}helferlein_BabelFish_GetStringsByQualifier",
               new object[] { portalID, qualifier, FallBackLocale });
         }
         return aquarium;
      }
#endregion

      public List<BabelFishInfo> GetStringsByKey(int portalID, string qualifier, string stringKey)
      {
         return GetStringsByKey(portalID, qualifier, stringKey, true);
      }

      public List<BabelFishInfo> GetStringsByKey(int portalID, string qualifier, string stringKey, bool fromCache)
      {
         string cacheKey = BabelfishBase.BABELFISH + BabelfishBase.SEPERATOR + "PortalID" + BabelfishBase.SEPERATOR + portalID +
                           BabelfishBase.SEPERATOR + "Qualifier" + BabelfishBase.SEPERATOR + qualifier +
                           BabelfishBase.SEPERATOR + "StringKey" + BabelfishBase.SEPERATOR + stringKey;

         if (fromCache)
         {
            if (DataCache.GetCache(cacheKey) != null)
               return (List<BabelFishInfo>)DataCache.GetCache(cacheKey);
            else
            {
               List<BabelFishInfo> aquarium = CBO.FillCollection<BabelFishInfo>(DataProvider.Instance().GetStringsByKey(portalID, qualifier, stringKey, FallBackLocale));
               if (aquarium != null)
                  DataCache.SetCache(cacheKey, aquarium);
               return aquarium;
            }
         }
         else
         {
            List<BabelFishInfo> aquarium = CBO.FillCollection<BabelFishInfo>(DataProvider.Instance().GetStringsByKey(portalID, qualifier, stringKey, FallBackLocale));
            if (aquarium != null)
               DataCache.SetCache(cacheKey, aquarium);
            return aquarium;
         }
      }

      public List<BabelFishInfo> GetNonFallBackStrings(int portalID, string qualifier, string stringKey)
      {
         return CBO.FillCollection<BabelFishInfo>(DataProvider.Instance().GetNonFallBackStrings(portalID, qualifier, stringKey, FallBackLocale));
      }

      public int Add(int portalID, string locale, string qualifier, string stringKey, string stringText)
      {
         return Add(portalID, locale, qualifier, stringKey, stringText, string.Empty, true);
      }

      public int Add(int portalID, string locale, string qualifier, string stringKey, string stringText, string stringComment)
      {
         return Add(new BabelFishInfo(portalID, locale, qualifier, stringKey, stringText, stringComment), true);
      }

      public int Add(int portalID, string locale, string qualifier, string stringKey, string stringText, string stringComment, bool updateCache)
      {
         return Add(new BabelFishInfo(portalID, locale, qualifier, stringKey, stringText, stringComment), updateCache);
      }

      public int Add(BabelFishInfo fish, bool updateCache)
      {
         int id = Convert.ToInt32(DataProvider.Instance().InsertString(fish.PortalID, fish.Locale, fish.Qualifier, fish.StringKey, fish.StringText, fish.StringComment));
         if (updateCache)
         {
            fish.ID = id;
            fish.FallBack = GetString(id).FallBack;
         }
         return id;
      }

      public int Change(int ID, string stringText)
      {
         return Change(ID, stringText, string.Empty, true);
      }

      public int Change(int portalID, string locale, string qualifier, string stringKey, string stringText)
      {
         return Change(portalID, locale, qualifier, stringKey, stringText, string.Empty);
      }

      public int Change(int ID, string stringText, string stringComment, bool updateCache)
      {
         BabelFishInfo fish = GetString(ID);
         fish.StringText = stringText;
         fish.StringComment = stringComment;
         return Change(fish, updateCache);
      }

      public int Change(int portalID, string locale, string qualifier, string stringKey, string stringText, string stringComment)
      {
         return Change(new BabelFishInfo(portalID, locale, qualifier, stringKey, stringText, stringComment), true);
      }

      public int Change(BabelFishInfo fish, bool updateCache)
      {
         int id = 0;
         if (!(string.IsNullOrEmpty(fish.StringText)))
         {
            id = Convert.ToInt32(DataProvider.Instance().UpdateString(fish.PortalID, fish.Locale, fish.Qualifier, fish.StringKey, fish.StringText, fish.StringComment));
            if (updateCache)
            {
               fish.ID = id;
               fish.FallBack = GetString(id).FallBack;
            }
         }
         else
            Drop(fish.PortalID, fish.Locale, fish.Qualifier, fish.StringKey);
         return id;
      }

      public int Change(List<BabelFishInfo> aquarium)
      {
         return Change(aquarium, true);
      }

      public int Change(List<BabelFishInfo> aquarium, bool updateCache)
      {
         int result = 0;
         result = Convert.ToInt32(DataProvider.Instance().UpdateStrings(AquariumToXml(aquarium)));
         if (updateCache)
         {
            foreach (BabelFishInfo fish in aquarium)
            {
               BabelFishInfo f = GetString(fish.PortalID, fish.Locale, fish.Qualifier, fish.StringKey);
            }
         }
         return result;
      }

      public void Drop(int ID)
      {
         // If this is the portal default locale, deletion is allowed only if this is the last remaining
         // portalID-qualifier-stringKey combination
         if (HttpContext.Current != null)
         {
            BabelFishInfo fish = GetString(ID);
            if (fish.Locale == FallBackLocale)
            {
               List<BabelFishInfo> aliens = GetNonFallBackStrings(fish.PortalID, fish.Qualifier, fish.StringKey);
               if (aliens.Count == 0)
                  DataProvider.Instance().DeleteString(ID);
               else
                  throw new Exception("CANNOT_DELETE_FALLBACKVALUE_WHEN_OTHER_LOCALES_EXIST");
            }
            else
            {
               DataProvider.Instance().DeleteString(ID);
            }
         }
         else
            throw new Exception("NO_HTTP_CONTEXT");
      }

      public void Drop(int portalID, string locale, string qualifier, string stringKey)
      {
         // If this is the portal default locale, deletion is allowed only if this is the last remaining
         // portalID-qualifier-stringKey combination
         if (HttpContext.Current != null)
         {
            BabelFishInfo fish = GetString(portalID, locale, qualifier, stringKey);
            if (fish != null)
            {
               if (fish.Locale == FallBackLocale)
               {
                  List<BabelFishInfo> aliens = GetNonFallBackStrings(fish.PortalID, fish.Qualifier, fish.StringKey);
                  if (aliens.Count == 0)
                     DataProvider.Instance().DeleteString(fish.ID);
                  else
                     throw new Exception("CANNOT_DELETE_FALLBACKVALUE_WHEN_OTHER_LOCALES_EXIST");
               }
               else
               {
                  DataProvider.Instance().DeleteString(fish.ID);
               }
            }
         }
         else
            throw new Exception("NO_HTTP_CONTEXT");
      }

      public void DropKey(int portalID, string qualifier, string stringKey)
      {
         DataProvider.Instance().DeleteStringKey(portalID, qualifier, stringKey);
      }

      public void DropLocale(int portalID, string locale)
      {
         // If this is the portal default locale, all other locales
         // will be deleted as well...
         if (HttpContext.Current != null)
         {
            if (locale == FallBackLocale)
               DataProvider.Instance().DeletePortal(portalID);
            else
               DataProvider.Instance().DeleteLocale(portalID, locale);
         }
         else
            throw new Exception("NO_HTTP_CONTEXT");
      }

      public string AquariumToXml(List<BabelFishInfo> aquarium)
      {
         StringBuilder xmlString = new StringBuilder();
         xmlString.Append("<Aquarium>");
         foreach (BabelFishInfo fish in aquarium)
         {
            xmlString.Append("<BabelFish>");
            xmlString.Append(string.Format("<PortalID>{0}</PortalID>", fish.PortalID));
            xmlString.Append(string.Format("<Locale>{0}</Locale>", fish.Locale));
            xmlString.Append(string.Format("<Qualifier>{0}</Qualifier>", fish.Qualifier));
            xmlString.Append(string.Format("<StringKey>{0}</StringKey>", fish.StringKey));
            xmlString.Append(string.Format("<StringText>{0}</StringText>", fish.StringText));
            xmlString.Append(string.Format("<StringComment>{0}</StringComment>", fish.StringComment));
            xmlString.Append("</BabelFish>");
         }
         xmlString.Append("</Aquarium>");
         return xmlString.ToString();
      }
   }
}