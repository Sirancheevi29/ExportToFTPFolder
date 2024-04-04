using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeVendorFTPExport.Model;

namespace TradeVendorFTPExport.BusinessLayer
{
    public class ReadData
    {
        public static DataTable GetCategoryData(Category category)
        {
            DataTable dt = new DataTable();
            DataTable SelectedTable = new DataTable();

            try
            {
               // string sql = string.Format("select {0} from ({1})", category.Columns, category.Query);
                dt = KalidoDB.Db.FetchData(category.Query);

                if (dt.Rows.Count > 0)
                {
                    //Selected columns
                    string[] SelectedColumns = category.Columns.Split(',').ToArray();
                    SelectedTable = new DataView(dt).ToTable(false, SelectedColumns);
                }
            }
            catch (Exception ex)
            {
                CLogger.Write("Exception occured GetCategoryData. Error message:" + ex.Message);
            }

            return SelectedTable;
        }
    }
}
