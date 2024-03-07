using System.Diagnostics;
using System.Net;


int numberofTryConnetFail = 0;

static async Task<bool> ISConnected()
{
    try
    {

        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(@"https://aws.amazon.com");
        var request = new HttpRequestMessage(HttpMethod.Head, "");
        var response = await httpClient.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
        {
            return true;
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("Failed connecting ...");
    }
    return false;
}

void ExecuteCommand()
{
    string programFiles = Environment.ExpandEnvironmentVariables("%ProgramW6432%");
    var connectFile = Path.Combine(programFiles, @"Microsoft Visual Studio\2022\Professional\Common7\Tools\AutoConnect.bat");

    string parameters = $"/k \"{connectFile}\"";

    var p = Process.Start("cmd", parameters);

    Thread.Sleep(30000);
    p.Close();
    numberofTryConnetFail++;
    Console.WriteLine("Finished process Connection ...");

    //int ExitCode;
    //ProcessStartInfo ProcessInfo;
    //Process Process;

    //ProcessInfo = new ProcessStartInfo("cmd.exe", "/c " + file);
    //ProcessInfo.CreateNoWindow = false;
    //ProcessInfo.UseShellExecute = false;

    //Process = Process.Start(ProcessInfo);
    //Process.WaitForExit();

    //ExitCode = Process.ExitCode;
    //Process.Close();

}

while (true)
{
    Console.WriteLine("Process connection ...");

    try
    {
        var tryConnect = false;

        try
        {
            var isConnected = await ISConnected();

            if (isConnected)
            {
                numberofTryConnetFail = 0;
         
                Console.WriteLine("Connection found - skip connection...");
                Thread.Sleep(30000);
                continue;
            }
            tryConnect = true;

            if (numberofTryConnetFail == 5)
            {
                var p = System.Diagnostics.Process.Start("shutdown", "/s /t 240");
                p.WaitForExit();
                //p.Close();
                return;
            }

            Process[] chromeInstances = Process.GetProcessesByName("chrome");

            foreach (Process p in chromeInstances)
                p.Kill();
        }
        catch (Exception e)
        {
            Console.WriteLine("Connection not found will try connect...");
            tryConnect = true;
        }

        if(tryConnect)
            ExecuteCommand();
    }
    catch (Exception e)
    {
        Console.WriteLine("Process connection file: " + e.ToString());
    }
}