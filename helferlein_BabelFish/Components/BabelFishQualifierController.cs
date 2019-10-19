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

using helferlein.DNN.Modules.BabelFish.Data;

namespace helferlein_BabelFish.Components
{
   public class BabelFishQualifierController
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