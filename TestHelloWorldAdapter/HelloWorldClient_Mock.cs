using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;

namespace CaptureCenter.HelloWorld
{
    public class HelloWorldClient_Mock : IHelloWorldClient
    {
        public CultureInfo Culture { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public void SetPassword(string password) { Password = password; }

        public string[] GetLogicalDrives()
        {
            return Directory.GetLogicalDrives();
        }

        public HelloWorldDirectoyInfo GetDirectoryInfo(string drive)
        {
            return new HelloWorldDirectoyInfo() { DirectoryInfo = new DirectoryInfo(drive) };
        }

        // Write text file
        public delegate void WriteTxtFile_Function(string filename, string text);
        public static WriteTxtFile_Function WriteTxtFile_CallBack = null;
        public void WriteTxtFile(string filename, string text)
        {
            if (WriteTxtFile_CallBack == null)
                throw new Exception("WriteTxtFile nnot support by mocked client");
            WriteTxtFile_CallBack(filename, text);
        }

        // Write pdf file
        public delegate void WritePDF_Function(string filename, string occFilename);
        public static WritePDF_Function WritePDF_CallBack = null;
        public void WritePDF(string filename, string occFilename)
        {
            if (WritePDF_CallBack == null)
                throw new Exception("WritePDF nnot support by mocked client");
            WritePDF_CallBack(filename, occFilename);
        }
    }
}
