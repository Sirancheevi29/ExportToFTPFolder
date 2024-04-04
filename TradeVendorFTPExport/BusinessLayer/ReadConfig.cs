using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TradeVendorFTPExport.BusinessLayer
{
    public class ReadConfig
    {
        public static string ReadSQL(string tag)
        {
            string SQLQuery = string.Empty;
            try
            {
                string SQLJson = File.ReadAllText(ConfigurationManager.AppSettings.Get("ConfigSQL"));

                JObject obj = JObject.Parse(SQLJson);
                SQLQuery = (string)obj.SelectToken(tag);
            }
            catch (Exception ex)
            {
                SQLQuery = "";
            }

            return SQLQuery;
        }
    }
}
