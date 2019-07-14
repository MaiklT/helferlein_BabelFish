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

namespace helferlein.DNN.Modules.BabelFish.Business
{
   public class BabelFishController : IUpgradeable
   {
      /// <summary>
      /// Gets the fallback locale (which is the portal's default language)
      /// </summary>
      public string FallBackLocale
      {
         get { return PortalController.Instance.GetCurrentPortalSettings().DefaultLanguage; }
      }

      /// <summary>
      /// Gets a BabelFishInfo object by it's ID
      /// </summary>
      /// <param name="ID">The ID of the BabelFishInfo object</param>
      /// <returns>BabelFishInfo object from the database (not cached)</returns>
      public BabelFishInfo GetString(int ID)
      {
         return GetString(ID, true);
      }

      /// <summary>
      /// Gets a BabelFishInfo object by it's ID, optionally from Cache
      /// </summary>
      /// <param name="ID">The ID of the BabelFishInfo object</param>
      /// <param name="fromCache">Use cache</param>
      /// <returns>BabelFishInfo object from the database or cache</returns>
      /// <remarks>If the BabelFishInfo comes from the database, it will be written to the cache anyway</remarks>
      public BabelFishInfo GetString(int ID, bool fromCache)
      {
         string cacheKey = BabelfishBase.BABELFISH + BabelfishBase.SEPERATOR + "ID" + BabelfishBase.SEPERATOR + ID.ToString();

         if (fromCache)
         {
            if (DataCache.GetCache(cacheKey) != null)
               // Object was found in the cache
               return (BabelFishInfo)DataCache.GetCache(cacheKey);
            else
            {
               // read it from the database and cache it
               BabelFishInfo fish = (BabelFishInfo)CBO.FillObject<BabelFishInfo>(DataProvider.Instance().GetString(ID));
               if (fish != null)
                  BabelFishUtils.FishToCache(fish);
               return fish;
            }
         }
         else
         {
            // read it from the database and cache it
            BabelFishInfo fish = (BabelFishInfo)CBO.FillObject<BabelFishInfo>(DataProvider.Instance().GetString(ID));
            if (fish != null)
               BabelFishUtils.FishToCache(fish);
            return fish;
         }
      }

      public BabelFishInfo GetString(int portalID, string locale, string qualifier, string stringKey)
      {
         return GetString(portalID, locale, qualifier, stringKey, true);
      }

      public BabelFishInfo GetString(int portalID, string locale, string qualifier, string stringKey, bool fromCache)
      {
         string cacheKey = BabelfishBase.BABELFISH + BabelfishBase.SEPERATOR + "PortalID" + portalID + 
                           BabelfishBase.SEPERATOR + "Locale" + BabelfishBase.SEPERATOR + locale +
                           BabelfishBase.SEPERATOR + "Qualifier" + BabelfishBase.SEPERATOR + qualifier +
                           BabelfishBase.SEPERATOR + "StringKey" + BabelfishBase.SEPERATOR + stringKey;

         if (fromCache)
         {
            if (DataCache.GetCache(cacheKey) != null)
               // Object was found in the cache
               return (BabelFishInfo)DataCache.GetCache(cacheKey);
            else
            {
               // read it from the database and cache it
               BabelFishInfo fish = CBO.FillObject<BabelFishInfo>(DataProvider.Instance().GetString(portalID, locale, qualifier, stringKey, FallBackLocale));
               if (fish != null)
                  BabelFishUtils.FishToCache(fish);
               return fish;
            }
         }
         else
         {
            // read it from the database and cache it
            BabelFishInfo fish = CBO.FillObject<BabelFishInfo>(DataProvider.Instance().GetString(portalID, locale, qualifier, stringKey, FallBackLocale));
            if (fish != null)
               BabelFishUtils.FishToCache(fish);
            return fish;
         }
      }

      public List<BabelFishInfo> GetStrings(int portalID, string locale, string qualifier)
      {
         return GetStrings(portalID, locale, qualifier, true);
      }

      public List<BabelFishInfo> GetStrings(int portalID, string locale, string qualifier, bool fromCache)
      {
         string cacheKey = BabelfishBase.BABELFISH + BabelfishBase.SEPERATOR + "PortalID" + BabelfishBase.SEPERATOR + portalID +
                           BabelfishBase.SEPERATOR + "Locale" + BabelfishBase.SEPERATOR + locale +
                           BabelfishBase.SEPERATOR + "Qualifier" + BabelfishBase.SEPERATOR + qualifier;

         if (fromCache)
         {
            if (DataCache.GetCache(cacheKey) != null)
               // Object was found in the cache
               return (List<BabelFishInfo>)DataCache.GetCache(cacheKey);
            else
            {
               // read it from the database and cache it
               List<BabelFishInfo> aquarium = CBO.FillCollection<BabelFishInfo>(DataProvider.Instance().GetStrings(portalID, locale, qualifier, FallBackLocale));
               if (aquarium != null)
                  DataCache.SetCache(cacheKey, aquarium);
               return aquarium;
            }
         }
         else
         {
            // read it from the database and cache it
            List<BabelFishInfo> aquarium = CBO.FillCollection<BabelFishInfo>(DataProvider.Instance().GetStrings(portalID, locale, qualifier, FallBackLocale));
            if (aquarium != null)
               DataCache.SetCache(cacheKey, aquarium);
            return aquarium;
         }
      }

      public List<BabelFishInfo> GetStrings(int portalID, string qualifier)
      {
         return GetStrings(portalID, qualifier, true);
      }

      public List<BabelFishInfo> GetStrings(int portalID, string qualifier, bool fromCache)
      {
         string cacheKey = BabelfishBase.BABELFISH + BabelfishBase.SEPERATOR + "PortalID" + BabelfishBase.SEPERATOR + portalID +
                           BabelfishBase.SEPERATOR + "Qualifier" + BabelfishBase.SEPERATOR + qualifier;

         if (fromCache)
         {
            if (DataCache.GetCache(cacheKey) != null)
               return (List<BabelFishInfo>)DataCache.GetCache(cacheKey);
            else
            {
               List<BabelFishInfo> aquarium = CBO.FillCollection<BabelFishInfo>(DataProvider.Instance().GetStrings(portalID, qualifier, FallBackLocale));
               if (aquarium != null)
                  DataCache.SetCache(cacheKey, aquarium);
               return aquarium;
            }
         }
         else
         {
            List<BabelFishInfo> aquarium = CBO.FillCollection<BabelFishInfo>(DataProvider.Instance().GetStrings(portalID, qualifier, FallBackLocale));
            if (aquarium != null)
               DataCache.SetCache(cacheKey, aquarium);
            return aquarium;
         }
      }

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
            fish.FallBack = GetString(id, false).FallBack;
            BabelFishUtils.FishToCache(fish);
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
               fish.FallBack = GetString(id, false).FallBack;
               BabelFishUtils.FishToCache(fish);
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
               BabelFishInfo f = GetString(fish.PortalID, fish.Locale, fish.Qualifier, fish.StringKey, false);
               BabelFishUtils.FishToCache(f);
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
            BabelFishInfo fish = GetString(ID, false);
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
            BabelFishUtils.ClearCache();
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
            BabelFishInfo fish = GetString(portalID, locale, qualifier, stringKey, false);
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
               BabelFishUtils.ClearCache();
            }
         }
         else
            throw new Exception("NO_HTTP_CONTEXT");
      }

      public void DropKey(int portalID, string qualifier, string stringKey)
      {
         DataProvider.Instance().DeleteStringKey(portalID, qualifier, stringKey);
         BabelFishUtils.ClearCache();
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
            BabelFishUtils.ClearCache();
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

#region IUpgradeable Member
      public string UpgradeModule(string version)
      {
         return string.Format("Upgrading to version {0}", version);
      }
#endregion
   }

   public class QualifierController
   {
      public void Drop(int portalID, string qualifier)
      {
         DataProvider.Instance().DeleteQualifier(portalID, qualifier);
      }

      public void Copy(int sourcePortalID, int targetPortalID, string qualifier)
      {
         DataProvider.Instance().CopyQualifier(sourcePortalID, targetPortalID, qualifier);
      }

      public void Copy(int sourcePortalID, int targetPortalID, string locale, string qualifier)
      {
         DataProvider.Instance().CopyQualifier(sourcePortalID, targetPortalID, locale, qualifier);
      }
   }
}