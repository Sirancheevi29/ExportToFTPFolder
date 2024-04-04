using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeVendorFTPExport.Model;
using System.Xml;

namespace TradeVendorFTPExport.BusinessLayer
{
    public class ReadXML
    {
        public static List<Category> Categories(string filePath)
        {
            List<Category> categories = new List<Category>();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                XmlNodeList xmlNodeList = doc.DocumentElement.SelectNodes("/VendorQueries/Category");

                foreach (XmlNode node in xmlNodeList)
                {
                    Category category = new Category();

                    category.Name = node.SelectSingleNode("Name").InnerText;
                    category.TableName = node.SelectSingleNode("TableName").InnerText;
                    category.Query = node.SelectSingleNode("Query").InnerText;
                    category.Columns = node.SelectSingleNode("Columns").InnerText;

                    categories.Add(category);
                }
            }
            catch (Exception e)
            {
                CLogger.Write(string.Format("Reading categories failed error message: {0}", e.Message));
            }

            return categories;
        }
    }
}
