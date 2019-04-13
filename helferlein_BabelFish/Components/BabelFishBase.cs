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
using System.Web;
using System;

namespace helferlein.DNN.Modules.BabelFish
{
   public class BabelfishBase
   {
      protected internal const string BABELFISH = "helferlein.BabelFish";
      protected internal const string CACHE_KEY_LIST = BABELFISH + ".CacheList";
      protected internal const string SEPERATOR = "|";
      protected internal const string SP_QUALIFIER = "helferlein_BabelFish_";

      public static string CommonResourceFile
      {
         get
         {
            string appPath = HttpContext.Current.Request.ApplicationPath;
            return  appPath + ((appPath.EndsWith("/")) ? String.Empty : "/") + "DesktopModules/helferlein_BabelFish/App_LocalResources/CommonResources";
         }
      }
   }
}