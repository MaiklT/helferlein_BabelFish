﻿/*
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
using DotNetNuke.Framework.Providers;
using DotNetNuke.Common.Utilities;
using System;
using System.Configuration;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace helferlein.DNN.Modules.BabelFish.Data
{
   public class SqlDataProvider : DataProvider
   {
#region Private Members
      private ProviderConfiguration providerConfiguration = null;
      private string connectionString;
      private string providerPath;
      private string objectQualifier;
      private string databaseOwner;
#endregion

#region Protected Properties
      protected ProviderConfiguration ProviderConfiguration
      {
         get
         {
            if (providerConfiguration == null)
               providerConfiguration = ProviderConfiguration.GetProviderConfiguration("data");
            return providerConfiguration;
         }
      }
#endregion

#region Private Methods
      private string GetFullyQualifiedName(string objectName)
      {
         return this.DatabaseOwner + this.ObjectQualifier + BabelfishBase.SP_QUALIFIER + objectName;
      }

      private object GetNull(object field)
      {
         return Null.GetNull(field, DBNull.Value);
      }
#endregion

#region Public Properties
      public string ConnectionString
      {
         get { return connectionString; }
      }

      public string ProviderPath
      {
         get { return providerPath; }
      }

      public string ObjectQualifier
      {
         get { return objectQualifier; }
      }

      public string DatabaseOwner
      {
         get { return databaseOwner; }
      }
#endregion

#region Constructor
      public SqlDataProvider()
      {
         try
         {
            Provider provider = (Provider)ProviderConfiguration.Providers[ProviderConfiguration.DefaultProvider];

            // This uses the legacy string...
            // if ((provider.Attributes["connectionStringName"] != string.Empty) && (ConfigurationManager.AppSettings[provider.Attributes["connectionStringName"]] != string.Empty))
            //    connectionString = ConfigurationManager.AppSettings[provider.Attributes["connectionStringName"]];
            // else
            //    connectionString = provider.Attributes["connectionString"];

            connectionString = Config.GetConnectionString();
            providerPath = provider.Attributes["providerPath"];

            objectQualifier = provider.Attributes["objectQualifier"];
            if ((objectQualifier != string.Empty) && (!(objectQualifier.EndsWith("_"))))
               objectQualifier += "_";

            databaseOwner = provider.Attributes["databaseOwner"];
            if ((databaseOwner != string.Empty) && (!(databaseOwner.EndsWith("."))))
               databaseOwner += ".";
         }
         catch (Exception ex)
         {
            throw ex;
         }
      }
#endregion

#region BabelFish Methods
      public override IDataReader GetQualifiers(int portalID)
      {
         return (IDataReader)SqlHelper.ExecuteReader(this.ConnectionString, this.GetFullyQualifiedName("GetQualifiers"), new object[] { portalID });
      }

      public override IDataReader GetString(int ID)
      {
         return (IDataReader)SqlHelper.ExecuteReader(this.ConnectionString, this.GetFullyQualifiedName("GetStringByID"), new object[] { ID });
      }

      public override IDataReader GetString(int portalID, string locale, string qualifier, string stringKey, string fallBackLocale)
      {
         return (IDataReader)SqlHelper.ExecuteReader(this.ConnectionString, this.GetFullyQualifiedName("GetString"), new object[] { portalID, locale, qualifier, stringKey, fallBackLocale });
      }

      public override IDataReader GetStrings(int portalID, string locale, string qualifier, string fallBackLocale)
      {
         return (IDataReader)SqlHelper.ExecuteReader(this.ConnectionString, this.GetFullyQualifiedName("GetStrings"), new object[] { portalID, locale, qualifier, fallBackLocale });
      }

      public override IDataReader GetStrings(int portalID, string qualifier, string fallBackLocale)
      {
         return (IDataReader)SqlHelper.ExecuteReader(this.ConnectionString, this.GetFullyQualifiedName("GetStringsByQualifier"), new object[] { portalID, qualifier, fallBackLocale });
      }

      public override IDataReader GetStringsByKey(int portalID, string qualifier, string stringKey, string fallBackLocale)
      {
         return (IDataReader)SqlHelper.ExecuteReader(this.ConnectionString, this.GetFullyQualifiedName("GetStringsByQualifierAndStringKey"), new object[] { portalID, qualifier, stringKey, fallBackLocale });
      }

      public override IDataReader GetNonFallBackStrings(int portalID, string qualifier, string stringKey, string fallBackLocale)
      {
         return (IDataReader)SqlHelper.ExecuteReader(this.ConnectionString, this.GetFullyQualifiedName("GetNonFallBackStrings"), new object[] { portalID, qualifier, stringKey, fallBackLocale });
      }

      public override int InsertString(int portalID, string locale, string qualifier, string stringKey, string stringText, string stringComment)
      {
         return Convert.ToInt32(SqlHelper.ExecuteScalar(this.ConnectionString, this.GetFullyQualifiedName("InsertString"), new object[] { portalID, locale, qualifier, stringKey, this.GetNull(stringText), this.GetNull(stringComment) }));
      }

      public override int UpdateString(int ID, string stringText, string stringComment)
      {
         return Convert.ToInt32(SqlHelper.ExecuteScalar(this.ConnectionString, this.GetFullyQualifiedName("UpdateStringByID"), new object[] { ID, this.GetNull(stringText), this.GetNull(stringComment) }));
      }

      public override int UpdateString(int portalID, string locale, string qualifier, string stringKey, string stringText, string stringComment)
      {
         return Convert.ToInt32(SqlHelper.ExecuteScalar(this.ConnectionString, this.GetFullyQualifiedName("UpdateString"), new object[] { portalID, locale, qualifier, stringKey, this.GetNull(stringText), this.GetNull(stringComment) }));
      }

      public override int UpdateStrings(string aquarium)
      {
         return Convert.ToInt32(SqlHelper.ExecuteScalar(this.ConnectionString, this.GetFullyQualifiedName("UpdateStrings"), new object[] { aquarium }));
      }

      public override void DeleteString(int ID)
      {
         SqlHelper.ExecuteNonQuery(this.ConnectionString, this.GetFullyQualifiedName("DeleteString"), new object[] { ID });
      }

      public override void DeleteStringKey(int portalID, string qualifier, string stringKey)
      {
         SqlHelper.ExecuteNonQuery(this.ConnectionString, this.GetFullyQualifiedName("DeleteStringKey"), new object[] { portalID, qualifier, stringKey });
      }

      public override void DeleteLocale(int portalID, string locale)
      {
         SqlHelper.ExecuteNonQuery(this.ConnectionString, this.GetFullyQualifiedName("DeleteLocale"), new object[] { portalID, locale });
      }

      public override void DeletePortal(int portalID)
      {
         SqlHelper.ExecuteNonQuery(this.ConnectionString, this.GetFullyQualifiedName("DeletePortal"), new object[] { portalID });
      }

      public override void DeleteQualifier(int portalID, string qualifier)
      {
         SqlHelper.ExecuteNonQuery(this.ConnectionString, this.GetFullyQualifiedName("DeleteQualifier"), new object[] { portalID, qualifier });
      }

      public override void CopyQualifier(int sourcePortalID, int targetPortalID, string qualifier)
      {
         SqlHelper.ExecuteNonQuery(this.ConnectionString, this.GetFullyQualifiedName("CopyQualifier"), new object[] { sourcePortalID, targetPortalID, qualifier });
      }

      public override void CopyQualifier(int sourcePortalID, int targetPortalID, string locale, string qualifier)
      {
         SqlHelper.ExecuteNonQuery(this.ConnectionString, this.GetFullyQualifiedName("CopyQualifierByLocale"), new object[] { sourcePortalID, targetPortalID, locale, qualifier });
      }
#endregion
   }
}