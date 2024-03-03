using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;

namespace MyPlayrightSample
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : PageTest
    {
        private int retryNr = 3;

        [Test]
        public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
        {

            Thread.Sleep(3000);
            var tryConnect = false;

            try
            {
                var isConnected = await ISConnected();

                if (isConnected)
                {
                    return;
                }

                //var interfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();


                //var isConnected = interfaces.Where(face => face.OperationalStatus == OperationalStatus.Up &&
                //                                 face.NetworkInterfaceType != NetworkInterfaceType.Tunnel &&
                //                                 face.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                //                                 !face.Name.Contains("virtual"))
                //    .Any(f => f.GetIPv4Statistics().BytesReceived > 0 && f.GetIPv4Statistics().BytesSent > 0);

                //if (isConnected)
                //{
                //    return;
                //}


                tryConnect = true;

                Process[] chromeInstances = Process.GetProcessesByName("chrome");

                foreach (Process p in chromeInstances)
                    p.Kill();
            }
            catch (Exception e)
            {
                tryConnect = true;
            }

            try
            {
                if (!tryConnect)
                    return; 
                
                var response = await Page.GotoAsync("http://www.msftconnecttest.com/redirect");

               if (response.Url.Contains(@"https://www.msn.com/da-dk?"))
               {
                   return;
               }

               Thread.Sleep(3000);

               // create a locator
               var agreeBtn = Page.Locator("id=chkAgree");

               // Click the get started link.
               await agreeBtn.ClickAsync();

               Thread.Sleep(1000);

               var connectBtn = Page.Locator("id=openaccess");

               await connectBtn.ClickAsync();

               Thread.Sleep(10000);

               var isConnected = await ISConnected();

               if (!isConnected && retryNr > 0)
               {
                   retryNr--;
                   HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage();
               }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                //if (e.Message != null && e.Message.Contains(@"net::ERR_INTERNET_DISCONNECTED"))
                //{

                //    return;
                //}

                throw;
            }
        }

        private static async Task<bool> ISConnected()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(@"https://aws.amazon.com");
            var request = new HttpRequestMessage(HttpMethod.Head, "");
            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
    }
}
