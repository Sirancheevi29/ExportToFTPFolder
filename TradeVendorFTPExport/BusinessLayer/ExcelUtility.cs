using OfficeOpenXml;
using OfficeOpenXml.Table;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeVendorFTPExport.Model;

namespace TradeVendorFTPExport.BusinessLayer
{
    public class ExcelUtility
    {
        //public static void ConvertToExcel(DataTable dataTable, string filePath, Category category)
        //{
        //    using (ExcelPackage pck = new ExcelPackage())
        //    {
        //        // File Name
        //        string fileName = string.Format("{0}.xlsx", category.Name);
        //        // Create the worksheet
        //        ExcelWorksheet ws = pck.Workbook.Worksheets.Add(fileName);

        //        // Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
        //        ws.Cells["A1"].LoadFromDataTable(dataTable, true);

        //        // Save the Excel file
        //        FileInfo fi = new FileInfo(filePath);
        //        pck.SaveAs(fi);
        //    }
        //}
        public static bool ConvertToExcel(DataTable dataTable, string filePath, Category category)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                using (ExcelPackage pck = new ExcelPackage())
                {
                    // File Name
                    string fileName = string.Format("{0}.xlsx", category.Name);
                    // Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(category.Name);

                    // Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                    ws.Cells["A1"].LoadFromDataTable(dataTable, true);

                    // Apply table formatting
                    var tableRange = ws.Cells[ws.Dimension.Address];
                    var table = ws.Tables.Add(tableRange, "MyTable");
                    table.TableStyle = TableStyles.Light12;

                    // Enable autofilter for columns
                    table.ShowHeader = true;
                    table.ShowFilter = true;

                    //Auto fit columns
                    ws.Cells.AutoFitColumns();

                    //Freeze columne A
                    ws.View.FreezePanes(1, 2);

                    // Save the Excel file
                    FileInfo fi = new FileInfo(filePath);
                    pck.SaveAs(fi);
                    return true;
                }
            }
            catch (Exception e)
            {
                CLogger.Write("DataTable to Excel convert failed. Error message: " + e.Message);
                return false;
            }
            
        }

        public static bool CleanUpFolder(string path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                if (di.Exists)
                {
                    FileInfo[] files = di.GetFiles("*.xlsx");
                    foreach (FileInfo file in files)
                    {
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                }
                CLogger.Write("All existing files are deleted");
                return true;
            }
            catch(Exception e)
            {
                CLogger.Write(string.Format("Existing folder clean up failed. Error message: {0}", e.Message));
                return false;
            }
        }

    }
}
