using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeVendorFTPExport.Model
{
    public class Category
    {
        public string Name { get; set; }
        public string TableName { get; set; }
        public string Query { get; set; }
        public string Columns { get; set; }
    }
}
