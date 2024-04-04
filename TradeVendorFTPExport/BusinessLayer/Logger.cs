using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeVendorFTPExport.BusinessLayer
{
    class CLogger
    {
        // Create a writer and open the file:
        StreamWriter log = null;
        String date = null;

        static CLogger logger = null;

        CLogger()
        {
            //Console.WriteLine("in cons");
            date = String.Format("{0:yyyyMMdd}", DateTime.Now);
            //Console.WriteLine("in cons "+date);
            createLog();
        }

        public static void Write(String content)
        {
            //Console.WriteLine(logger);
            if (logger == null)
            {
                logger = new CLogger();
            }

            //Console.WriteLine(logger+"  Write content");
            logger.WriteContent(content);
        }

        public void createLog()
        {

            String logname = ConfigurationManager.AppSettings["LogPath"] + "Logs" + date + ".txt";
            Console.WriteLine("in createlog " + logname);
            if (log != null)
            {
                log.Close();
            }

            if (!File.Exists(logname))
            {
                log = new StreamWriter(logname);
            }
            else
            {
                log = File.AppendText(logname);
            }
        }

        Boolean compareDates()
        {
            String currentdate = String.Format("{0:yyyyMMdd}", DateTime.Now);
            //Console.WriteLine(currentdate + "  " + date + " " + currentdate.Equals(date));
            if (!currentdate.Equals(date))
            {
                date = String.Format("{0:yyyyMMdd}", DateTime.Now);
                createLog();
            }
            return true;
        }

        public void WriteContent(String content)
        {
            if (compareDates())
            {

                String currentdate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
                // Write to the file:
                log.WriteLine("[" + currentdate + "] " + content);
                log.AutoFlush = true;
                log.Flush();
            }
        }

        public void close()
        {
            if (log != null)
            {
                log.Close();
            }

            log = null;
        }

        public static void closeLogger()
        {
            if (logger != null)
            {
                logger.close();
            }

            logger = null;
        }

    }
}
