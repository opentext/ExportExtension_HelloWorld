using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using DOKuStar.Diagnostics.Tracing;
using ExportExtensionCommon;

namespace CaptureCenter.HelloWorld
{

    [TestClass]
    public class TestHelloWorldAdapter
    {
        #region Document tab handling
        [TestMethod]
        [TestCategory("HelloWorld adapter test")]
        public void t01_DocumentNameTab()
        {
            string targetDir = Path.GetTempPath();
            string document = Path.Combine(targetDir, "Test_HelloWorldExport.pdf");

            IHelloWorldClient helloWorldClient = new HelloWorldClient_Mock();

            HelloWorldExport export = new HelloWorldExport(helloWorldClient);
            HelloWorldSettings settings = new HelloWorldSettings();
            settings.Servername = "localhost";
            settings.Username = "Johannes";
            settings.Password = PasswordEncryption.Encrypt("My Passowrd");
            HelloWorldViewModel viewModel = new HelloWorldViewModel(settings, helloWorldClient);
            viewModel.FT.ActivateTab();
            TVIViewModel tvm =  viewModel.FT.Folders.FindNodeInTree(targetDir);
            settings.SerializedFolderPath = tvm.GetSerializedPath();

            // Create batch
            SIEEBatch batch = new SIEEBatch();
            SIEEDocument doc = new SIEEDocument();
            batch.Add(doc);
            doc.Fieldlist = new SIEEFieldlist();
            SIEEField field = new SIEEField("SampleField", "ext", "SomeValue");
            doc.Fieldlist.Add(field);
            doc.PDFFileName = document;
            doc.BatchId = "4711";

            var td = new[]
            {
                new { n=01, result="abc", annotation="abc", filename= "xyz", spec=""},
                new { n=02, result="xyz", annotation="", filename= "xyz", spec=""},
                new { n=03, result="4711", annotation="", filename= "xyz", spec="<BATCHID>"},
                new { n=04, result="SomeValue", annotation="", filename= "xyz", spec="<:SampleField>"},
           };
            int doOnly = 0;

            for (int i = 0; i != td.Length; i++)
            {
                if (doOnly != 0 && td[i].n != doOnly) continue;

                doc.ScriptingName = (td[i].annotation == "") ? null : td[i].annotation;
                doc.InputFileName = td[i].filename;
                if (td[i].spec != "")
                {
                    viewModel.DT.UseInputFileName = false;
                    viewModel.DT.UseSpecification = true;
                    viewModel.DT.Specification = td[i].spec;
                }
                else
                {
                    viewModel.DT.UseInputFileName = true;
                    viewModel.DT.UseSpecification = false;
                    viewModel.DT.Specification = null;
                }

                string txtFilename = null;
                string pdfFilename = null;
                HelloWorldClient_Mock.WriteTxtFile_CallBack = (filename, text) =>
                {
                    txtFilename = filename;
                };
                HelloWorldClient_Mock.WritePDF_CallBack = (filename, content) =>
                {
                    pdfFilename = filename;
                };
                export.ExportBatch(viewModel.Settings, batch);
                
                Assert.AreEqual(Path.Combine(targetDir, td[i].result) + ".txt", txtFilename);
                Assert.AreEqual(Path.Combine(targetDir, td[i].result) + ".pdf", pdfFilename);
            }
            Assert.AreEqual(doOnly, 0);
        }
        #endregion
    }
}
