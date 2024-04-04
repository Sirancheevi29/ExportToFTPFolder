using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeVendorFTPExport.BusinessLayer;
using TradeVendorFTPExport.Model;

namespace TradeVendorFTPExport
{
    public class Process
    {
        public static void Start(IConfigurationRoot config)
        {
            try
            {
                List<Category> categories = ReadXML.Categories(config["ConfigurationPath"].ToString());
                string LocalPath = config["LocalFilePath"].ToString();

                if (categories.Count <= 0 || categories == null)
                {
                    CLogger.Write("Read configuration returns 0 entries");
                    return;
                }

                CLogger.Write(string.Format("Total categories loaded:{0}", categories.Count));

                //CleanUp existing files in Local folder
                ExcelUtility.CleanUpFolder(LocalPath);

                //Generate excel files in LocalFolder
                foreach (Category item in categories)
                {
                    CLogger.Write(string.Format("Export started for category: {0}", item.Name));

                    //Get data from database
                    DataTable dt = ReadData.GetCategoryData(item);

                    //File Name 
                    string FileName = string.Format("{0}{1}.xlsx", LocalPath, item.Name);

                    if (dt.Rows.Count > 0)
                    {
                        //Convert Category data to Excel file and save to LocalFiles Folder
                        ExcelUtility.ConvertToExcel(dt, FileName, item);
                    }
                    else
                    {
                        CLogger.Write(string.Format("Category {0} has zero records returned from database.", item.Name));
                    }
                }

                // Checking local folder files
                DirectoryInfo LocalDirectroy = new DirectoryInfo(LocalPath);
                FileInfo[] Files = LocalDirectroy.GetFiles("*.xlsx");

                if (Files.Length > 0)
                {
                    //Push files to FTP
                    FTPUtlity.LocalToFTP(config);
                }
                else
                {
                    CLogger.Write("Local folder has no new files to move to FTP location");
                }
            }
            catch (Exception e)
            {
                CLogger.Write("Process start error message:" + e.Message);
            }
        }
    }
}
