using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeVendorFTPExport.BusinessLayer
{
    public class FTPUtlity
    {
        public static void Push(string HostServer, string FTPUserName, string FTPPassword, FileInfo[] LocalFiles, string RemotePath, string LocalFilePath)
        {
            string host = HostServer;
            string username = FTPUserName;
            string password = FTPPassword;
            string remoteFilePath = RemotePath;

            using (var sftp = new SftpClient(host, username, password))
            {
                try
                {
                    sftp.Connect();
                    CLogger.Write("FTP location connected successfully");

                    //CleanUp exisitng files
                    var files = sftp.ListDirectory(RemotePath);

                    CLogger.Write(String.Format("Total number of files found in FTP location: {0}", files.Count()));
                    foreach (var file in files)
                    {
                        if (file.IsRegularFile && file.Name.EndsWith(".xlsx"))
                        {
                            sftp.DeleteFile(file.FullName);
                            CLogger.Write(string.Format("FTP file deleted: {0}", file.Name));
                        }
                    }

                    CLogger.Write(String.Format("Total number of files found in Local location: {0}", LocalFiles.Count()));
                    foreach (FileInfo file in LocalFiles)
                    {

                        //File path
                        string FilePath = $"{LocalFilePath}{file.Name}";
                        string RemoteFile = $"{RemotePath}\\{file.Name}";
                        CLogger.Write(string.Format("Preparing file :{0}", FilePath));
                        if (file.Exists)
                        {
                            using (var fileStream = new FileStream(FilePath, FileMode.Open))
                            {
                                sftp.UploadFile(fileStream, RemoteFile);
                                CLogger.Write(string.Format("File uploaded to FTP folder. File Name: {0}", file.FullName));
                            }
                        }
                        else
                        {
                            CLogger.Write(string.Format("ERROR: File didn't exist. File Name: {0}", file.Name));
                        }
                    }
                    CLogger.Write("Files are moved to FTP folder successfully");

                }
                catch (Exception e)
                {
                    CLogger.Write(string.Format("An error occurred: {0}", e.Message));
                }
                finally
                {
                    sftp.Disconnect();
                    CLogger.Write("FTP folder is disconnected");
                }
            }
        }

        public static bool LocalToFTP(IConfigurationRoot config)
        {
            try
            {
                string HostServer = config["FTPServices:0:URL"].ToString();
                string userName = config["FTPServices:0:UserName"].ToString();
                string Password = config["FTPServices:0:Password"].ToString();
                string LocalFilePath = config["LocalFilePath"].ToString();
                int Port = Convert.ToInt32(config["FTPServices:0:Port"]);
                string RemotePath = config["FTPServices:0:RemotePath"].ToString();

                DirectoryInfo di = new DirectoryInfo(LocalFilePath);
                FileInfo[] LocalFiles = di.GetFiles("*.xlsx");

                CLogger.Write($"Funcation called to move files from Local folder to FTP location {HostServer}{RemotePath}");
                Push(HostServer, userName, Password, LocalFiles, RemotePath, LocalFilePath);

                return true;
            }
            catch (Exception ex)
            {
                CLogger.Write(string.Format("LocalToFTP file error:{0}", ex.Message));
                return false;
            }
        }
    }
}
