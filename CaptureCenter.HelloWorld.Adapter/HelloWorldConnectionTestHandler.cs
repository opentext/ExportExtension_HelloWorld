using System;
using System.Net.NetworkInformation;
using ExportExtensionCommon;

namespace CaptureCenter.HelloWorld
{
    public class HelloWorldConnectionTestHandler : ConnectionTestHandler
    {
        public HelloWorldConnectionTestHandler(VmTestResultDialog vmTestResultDialog) : base(vmTestResultDialog)
        {
            TestList.Add(new TestFunctionDefinition()
                { Name = "Try to reach Server (ping)", Function = TestFunction_Ping, ContinueOnError = true });
            TestList.Add(new TestFunctionDefinition()
                { Name = "Try to log in", Function = TestFunction_Login });
            TestList.Add(new TestFunctionDefinition()
                { Name = "Try to read some information", Function = TestFunction_Read });
        }

        #region The test fucntions
        private bool TestFunction_Ping(ref string errorMsg)
        {
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(((HelloWorldViewModel_CT)CallingViewModel).Servername);
                if (reply.Status != IPStatus.Success)
                {
                    errorMsg = "Return status = " + reply.Status.ToString();
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                if (e.InnerException != null) errorMsg += "\n" + e.InnerException.Message;
                return false;
            }
        }

        private bool TestFunction_Login(ref string errorMsg)
        {
            //testPsc = vmConnection.GetConnector();
            try
            {
                //testPsc.Login(vmConnection.Username, vmConnection.Password);
            }
            catch (Exception e)
            {
                errorMsg = "Could not log in. \n" + e.Message;
                if (e.InnerException != null)
                    errorMsg += "\n" + e.InnerException.Message;
                return false;
            }
            return true;
        }

        private bool TestFunction_Read(ref string errorMsg)
        {
            //ProcessSuiteInstance psi = new ProcessSuiteInstance(testPsc);
            try
            {
                //List<ProcessSuiteSolution> solutions = psi.GetSolutions();
                //errorMsg = solutions.Count.ToString() + " Solutions found";
                return true;
            }
            catch (Exception e)
            {
                errorMsg = "Could not read solutions\n" + e.Message;
                if (e.InnerException != null)
                    errorMsg += "\n" + e.InnerException.Message;
                return false;
            }
        }
        #endregion
    }
}
