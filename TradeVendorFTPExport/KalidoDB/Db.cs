using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeVendorFTPExport.KalidoDB
{
    public class Db
    {
        public static DataTable FetchData(string sql)
        {
            DataTable dtResult = new DataTable();
            OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["DBconn"].ConnectionString);
            //OracleConfiguration.SqlNetAuthenticationServices = "none";

            try
            {
                OracleCommand cmd = new OracleCommand(sql, connection);
                OracleDataAdapter abt = new OracleDataAdapter(cmd);

                connection.Open();

                abt.Fill(dtResult);

                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return dtResult;
        }

        public static bool InsertData(string sql)
        {
            OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["DBconn"].ConnectionString);
            try
            {
                OracleCommand cmd = new OracleCommand(sql, connection);

                connection.Open();
                var RowSeq = cmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                connection.Close();
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public static void Execute(string sql)
        {
            OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["DBconn"].ConnectionString);
            try
            {
                OracleCommand cmd = new OracleCommand(sql, connection);
                connection.Open();
                var RowSeq = cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

    }
}
