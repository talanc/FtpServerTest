using System;
using System.IO;
using System.Net.FtpClient;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FubarDev.FtpServer;
using FubarDev.FtpServer.AccountManagement;
using FubarDev.FtpServer.FileSystem.DotNet;

namespace FtpTest
{
    [TestClass]
    public class FtpTests
    {

        [TestMethod]
        public void TestGNU()
        {
            VerifyListing("ftp.gnu.org", "anonymous", "test@example.com");
        }

        //[TestMethod]
        //public void TestLocal()
        //{
        //    // Assumes a running FTP
        //    VerifyListing("127.0.0.1", "anonymous", "test@example.com");
        //}

        [TestMethod]
        public void TestCreated()
        {
            var host = "127.0.0.1";

            // allow only anonymous logins
            var membershipProvider = new AnonymousMembershipProvider();

            // use %TEMP%/TestFtpServer as root folder
            var fsProvider = new DotNetFileSystemProvider(Path.Combine(Path.GetTempPath(), "TestFtpServer"), false);

            // Initialize the FTP server
            var ftpServer = new FtpServer(fsProvider, membershipProvider, host);

            // Start the FTP server
            ftpServer.Start();

            VerifyListing(host, "anonymous", "test@example.com");

            //Console.WriteLine("Press ENTER/RETURN to close the test application.");
            //Console.ReadLine();

            // Stop the FTP server
            ftpServer.Stop();
        }

        public void VerifyListing(string host, string username, string password)
        {
            var ftpClient = new FtpClient();

            ftpClient.Host = host;
            ftpClient.Credentials = new NetworkCredential(username, password);
            ftpClient.Connect();
            var listing = ftpClient.GetListing();
            Assert.IsNotNull(listing);
        }
    }
}
