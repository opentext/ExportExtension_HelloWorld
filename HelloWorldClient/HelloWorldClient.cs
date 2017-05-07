using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;

namespace CaptureCenter.HelloWorld
{
    #region Interface
    public interface IHelloWorldClient
    {
        CultureInfo Culture { get; set; }
        string Username { get; set; }
        string Password { get;  }
        void SetPassword(string password);
        string[] GetLogicalDrives();
        HelloWorldDirectoyInfo GetDirectoryInfo(string drive);
        void WriteTxtFile(string filename, string text);
        void WritePDF(string filename, string occFilename);
    }
    #endregion

    #region Exchange types
    public class HelloWorldDirectoyInfo
    {
        public DirectoryInfo DirectoryInfo { get; set; }
    }
    #endregion

    #region Implementation
    public class HelloWorldClient : IHelloWorldClient
    {
        public CultureInfo Culture { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public void SetPassword(string password)
        {
            Password = password;
        }

        public string[] GetLogicalDrives()
        {
            return Directory.GetLogicalDrives();
        }

       public HelloWorldDirectoyInfo GetDirectoryInfo(string drive)
        {
            return new HelloWorldDirectoyInfo() { DirectoryInfo = new DirectoryInfo(drive) };
        }

        public void WriteTxtFile(string filename, string text)
        {
            File.WriteAllText(filename, text);
        }

        public void WritePDF(string filename, string occFilename)
        {
            File.WriteAllBytes(filename, File.ReadAllBytes(occFilename));
        }
    }
    #endregion
}
