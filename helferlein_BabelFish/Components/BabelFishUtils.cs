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
using DotNetNuke.Common.Utilities;
using helferlein.DNN.Modules.BabelFish.Data;
using DotNetNuke.Entities.Portals;

namespace helferlein.DNN.Modules.BabelFish.Business
{
   public class BabelFishUtils
   {
      protected static internal void FishToCache(BabelFishInfo fish)
      {
         string cacheKey1 = BabelfishBase.BABELFISH + BabelfishBase.SEPERATOR + "ID" + BabelfishBase.SEPERATOR + Convert.ToString(fish.ID);
         string cacheKey2 = BabelfishBase.BABELFISH + BabelfishBase.SEPERATOR + "PortalID" + BabelfishBase.SEPERATOR + fish.PortalID +
                   BabelfishBase.SEPERATOR + "Locale" + BabelfishBase.SEPERATOR + fish.Locale +
                   BabelfishBase.SEPERATOR + "Qualifier" + BabelfishBase.SEPERATOR + fish.Qualifier +
                   BabelfishBase.SEPERATOR + "StringKey" + BabelfishBase.SEPERATOR + fish.StringKey;
         string cacheKey3 = BabelfishBase.BABELFISH + BabelfishBase.SEPERATOR + "PortalID" + BabelfishBase.SEPERATOR + fish.PortalID +
                   BabelfishBase.SEPERATOR + "Locale" + BabelfishBase.SEPERATOR + fish.Locale +
                   BabelfishBase.SEPERATOR + "Qualifier" + BabelfishBase.SEPERATOR + fish.Qualifier;
         string cacheKey4 = BabelfishBase.BABELFISH + BabelfishBase.SEPERATOR + "PortalID" + BabelfishBase.SEPERATOR + fish.PortalID +
                   BabelfishBase.SEPERATOR + "Qualifier" + BabelfishBase.SEPERATOR + fish.Qualifier;
         string cacheKey5 = BabelfishBase.BABELFISH + BabelfishBase.SEPERATOR + "PortalID" + BabelfishBase.SEPERATOR + fish.PortalID +
                  BabelfishBase.SEPERATOR + "Qualifier" + BabelfishBase.SEPERATOR + fish.Qualifier +
                  BabelfishBase.SEPERATOR + "StringKey" + BabelfishBase.SEPERATOR + fish.StringKey;

         BabelFishController controller = new BabelFishController();

         // This is quite simple...
         DataCache.SetCache(cacheKey1, fish);
         DataCache.SetCache(cacheKey2, fish);

         List<BabelFishInfo> hitchHikersGuide = null;
         if (DataCache.GetCache(cacheKey3) != null)
            // We already have a list
            hitchHikersGuide = (List<BabelFishInfo>)DataCache.GetCache(cacheKey3);
         else
            // Let's read the list from the database
            hitchHikersGuide = CBO.FillCollection<BabelFishInfo>(DataProvider.Instance().GetStrings(fish.PortalID, fish.Locale, fish.Qualifier, PortalController.Instance.GetCurrentPortalSettings().DefaultLanguage));
         bool found = false;
         foreach (BabelFishInfo bfi in hitchHikersGuide)
         {
            if ((bfi.ID == fish.ID) && (bfi.PortalID == fish.PortalID) && (bfi.Locale == fish.Locale) && (bfi.Qualifier == fish.Qualifier) && (bfi.StringKey == fish.StringKey))
            {
               // The fish is in the net, so we update it's stringtext and fallback property
               bfi.StringText = fish.StringText;
               bfi.StringComment = fish.StringComment;
               bfi.FallBack = fish.FallBack;
               found = true;
               break;
            }
         }
         if (!(found))
            // Caught! :-)
            hitchHikersGuide.Add(fish);
         DataCache.SetCache(cacheKey3, hitchHikersGuide);

         hitchHikersGuide = null;
         if (DataCache.GetCache(cacheKey4) != null)
            // We already have a list
            hitchHikersGuide = (List<BabelFishInfo>)DataCache.GetCache(cacheKey4);
         else
            // Let's read the list from the database
            hitchHikersGuide = CBO.FillCollection<BabelFishInfo>(DataProvider.Instance().GetStrings(fish.PortalID, fish.Qualifier, PortalController.Instance.GetCurrentPortalSettings().DefaultLanguage));
         found = false;
         foreach (BabelFishInfo bfi in hitchHikersGuide)
         {
            if ((bfi.ID == fish.ID) && (bfi.PortalID == fish.PortalID) && (bfi.Locale == fish.Locale) && (bfi.Qualifier == fish.Qualifier) && (bfi.StringKey == fish.StringKey))
            {
               // The fish is in the net, so we update it's stringtext and fallback property
               bfi.StringText = fish.StringText;
               bfi.StringComment = fish.StringComment;
               bfi.FallBack = fish.FallBack;
               found = true;
               break;
            }
         }
         if (!(found))
            // Caught! :-)
            hitchHikersGuide.Add(fish);
         DataCache.SetCache(cacheKey4, hitchHikersGuide);

         hitchHikersGuide = null;
         if (DataCache.GetCache(cacheKey5) != null)
            // We already have a list
            hitchHikersGuide = (List<BabelFishInfo>)DataCache.GetCache(cacheKey5);
         else
            // Let's read the list from the database
            hitchHikersGuide = CBO.FillCollection<BabelFishInfo>(DataProvider.Instance().GetStringsByKey(fish.PortalID, fish.Qualifier, fish.StringKey, PortalController.Instance.GetCurrentPortalSettings().DefaultLanguage));
         found = false;
         foreach (BabelFishInfo bfi in hitchHikersGuide)
         {
            if ((bfi.ID == fish.ID) && (bfi.PortalID == fish.PortalID) && (bfi.Locale == fish.Locale) && (bfi.Qualifier == fish.Qualifier) && (bfi.StringKey == fish.StringKey))
            {
               // The fish is in the net, so we update it's stringtext and fallback property
               bfi.StringText = fish.StringText;
               bfi.StringComment = fish.StringComment;
               bfi.FallBack = fish.FallBack;
               found = true;
               break;
            }
         }
         if (!(found))
            // Caught! :-)
            hitchHikersGuide.Add(fish);
         DataCache.SetCache(cacheKey5, hitchHikersGuide);

         // Update CacheList
         List<string> cacheList;

         if (DataCache.GetCache(BabelfishBase.CACHE_KEY_LIST) != null)
            cacheList = (List<string>)DataCache.GetCache(BabelfishBase.CACHE_KEY_LIST);
         else
            cacheList = new List<string>();

         if (!(cacheList.Contains(cacheKey1)))
            cacheList.Add(cacheKey1);
         if (!(cacheList.Contains(cacheKey2)))
            cacheList.Add(cacheKey2);
         if (!(cacheList.Contains(cacheKey3)))
            cacheList.Add(cacheKey3);
         if (!(cacheList.Contains(cacheKey4)))
            cacheList.Add(cacheKey4);
         if (!(cacheList.Contains(cacheKey5)))
            cacheList.Add(cacheKey5);

         DataCache.SetCache(BabelfishBase.CACHE_KEY_LIST, cacheList);
      }

      protected static internal void ClearCache()
      {
         if (DataCache.GetCache(BabelfishBase.CACHE_KEY_LIST) != null)
         {
            List<string> cacheList = (List<string>)DataCache.GetCache(BabelfishBase.CACHE_KEY_LIST);
            foreach(string key in cacheList)
               DataCache.RemoveCache(key);
            DataCache.RemoveCache(BabelfishBase.CACHE_KEY_LIST);
         }
      }
   }
}