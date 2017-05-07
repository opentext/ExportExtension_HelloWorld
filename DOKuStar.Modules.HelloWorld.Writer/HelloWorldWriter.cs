using System;
using RightDocs.Common;
using ExportExtensionCommon;
using CaptureCenter.HelloWorld;

namespace DOKuStar.HelloWorld
{
    [CustomExportDestinationDescription("HelloWorldWriter", "ExportExtensionInterface", "SIEE based Writer for HelloWorld Export", "OpenText")]
    public class HelloWorldWriter: EECExportDestination
    {
        public HelloWorldWriter() : base()
        {
            Initialize(new HelloWorldFactory());
        }
    }
}