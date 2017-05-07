using System;
using System.IO;
using System.Collections.Generic;
using ExportExtensionCommon;
using DOKuStar.Diagnostics.Tracing;

namespace CaptureCenter.HelloWorld
{
    public class HelloWorldExport : SIEEExport
    {
        private IHelloWorldClient helloWorldClient;

        public HelloWorldExport(IHelloWorldClient helloWorldClient)
        {
            this.helloWorldClient = helloWorldClient;
        }

        public override void ExportDocument(SIEESettings settings, SIEEDocument document, string name, SIEEFieldlist fieldlist)
        {
            HelloWorldSettings mySettings = settings as HelloWorldSettings;
            string fieldname = "SampleField";
            string folderName = mySettings.GetFolderName();
            SIEEField field = fieldlist.GetFieldByName(fieldname);
            string username = mySettings.Username;
            string password = PasswordEncryption.Decrypt(mySettings.Password);

            helloWorldClient.WriteTxtFile(Path.Combine(folderName, name + ".txt"), 
                "Fieldname=" + fieldname + 
                "\nValue=" + field.Value +
                "\nFilename=" + name +
                "\nUsername=" + username
            );
            helloWorldClient.WritePDF(Path.Combine(folderName, name) + ".pdf", document.PDFFileName);
        }
    }
}
