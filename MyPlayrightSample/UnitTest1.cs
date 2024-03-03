using Microsoft.VisualBasic;
using System;
using System.Net;

namespace MyPlayrightSample
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : PageTest
    {
        [Test]
        public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
        {

            Thread.Sleep(3000);
            var tryConnect = false;

            try
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(@"http://www.google.com/");
                var request = new HttpRequestMessage(HttpMethod.Head, "");
                var response = await httpClient.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
                {
                    return;
                }
                tryConnect = true;
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

               Thread.Sleep(5000);

               // create a locator
               var agreeBtn = Page.Locator("id=chkAgree");

               // Click the get started link.
               await agreeBtn.ClickAsync();

               Thread.Sleep(1000);

               var connectBtn = Page.Locator("id=openaccess");

               await connectBtn.ClickAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                if (e.Message != null && e.Message.Contains(@"net::ERR_INTERNET_DISCONNECTED"))
                {

                    return;
                }

                throw;
            }
        }
    }
}
