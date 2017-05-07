using System;
using System.Drawing;
using ExportExtensionCommon;

namespace CaptureCenter.HelloWorld
{
    public class HelloWorldFactory : SIEEFactory
    {
        public override SIEESettings CreateSettings() { return new HelloWorldSettings(); }
        public override SIEEUserControl CreateWpfControl() { return new HelloWorldControlWPF(); }
        public override SIEEViewModel CreateViewModel(SIEESettings settings)
        {
            return new HelloWorldViewModel(settings, new HelloWorldClient());
        }
        public override SIEEExport CreateExport() { return new HelloWorldExport(new HelloWorldClient()); }
        public override SIEEDescription CreateDescription() { return new HelloWorldDescription(); }
    }

    class HelloWorldDescription : SIEEDescription
    {
        public override string TypeName { get { return "HelloWorld"; } }

        public override string GetLocation(SIEESettings s)
        {
            return ((HelloWorldSettings)s).GetFolderName();
        }

        public override void OpenLocation(string location)
        {
            System.Diagnostics.Process.Start(location);
        }

        public override Image Image { get { return Properties.Resources.Icon; } }
    }
}
