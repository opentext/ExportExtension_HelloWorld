using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using ExportExtensionCommon;

namespace CaptureCenter.HelloWorld
{
    [TestClass]
    public class TestHelloWorldClient
    {
        #region Test infrastructure

        class HelloWorldTestSystem
        {
            public bool Active { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        private Dictionary<string, HelloWorldTestSystem> testsystems = new Dictionary<string, HelloWorldTestSystem>()
        {
            { "HeloWorld", new HelloWorldTestSystem()
            {
                Active = true,
                Username = "Hello",
                Password = "World!",
            }},
        };

        private string testDocument;

        public TestHelloWorldClient()
        {
            testDocument = Path.GetTempFileName().Replace(".tmp", ".pdf");
            File.WriteAllBytes(testDocument, Properties.Resources.Document);
        }
        #endregion

        #region Basic file storage
        [TestMethod]
        [TestCategory("HelloWorld client test")]
        public void t01_basicFileStorage()
        {
            foreach (KeyValuePair<string, HelloWorldTestSystem> ts in testsystems)
                if (ts.Value.Active) _t01_basicFileStorage(ts.Value);
        }
        private void _t01_basicFileStorage(HelloWorldTestSystem ts)
        { 
            HelloWorldClient helloWorldClient = createClient(ts);

            string tmpDoc = Path.GetTempFileName();
            helloWorldClient.WritePDF(tmpDoc, testDocument);
            Assert.IsTrue(SIEEUtils.CompareFiles(testDocument, tmpDoc));
            File.Delete(tmpDoc);
        }
        #endregion

        #region Utilities
        private HelloWorldClient createClient(HelloWorldTestSystem ts)
        {
            HelloWorldClient hwc = new HelloWorldClient();
            hwc.Username = ts.Username;
            hwc.Password = ts.Password;
            return hwc;
        }
        #endregion
    }
}
