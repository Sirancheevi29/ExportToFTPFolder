using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeVendorFTPExport.BusinessLayer;

namespace TradeVendorFTPExport
{
    class Program
    {
        static void Main(string[] args)
        {
            CLogger.Write(string.Format("Trade Vendor export started at : {0}", DateTime.Now));

            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var config = builder.Build();

            bool Halt = Convert.ToBoolean(config["Halt"]);

            if (Halt)
            {
                CLogger.Write("Trade Vendor export Stopped");
                return;
            }

            string ScheduledToRun =config["ScheduledHourToRun"];
            string TimeNow = DateTime.Now.ToString("HH:mm");

            if (ScheduledToRun != TimeNow)
            {
                CLogger.Write(string.Format("Trade vendor export is not scheduled at: {0} ", TimeNow));
                return;
            }

            Process.Start(config);

            CLogger.Write("Trade Vendor export completed");
        }
    }
}
