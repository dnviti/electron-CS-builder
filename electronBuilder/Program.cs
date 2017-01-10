using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace electronBuilder
{
    class Program
    {
        public static Process nodeInstall = new Process();
        public static Process buildApp = new Process();

        static void Main(string[] args)
        {
            Console.WindowHeight = 5;
            Console.WindowWidth = 40;

            if (ExistsOnPath("node.exe"))
            {
                BuildApplication(nodeInstall, buildApp);
            }
            else
            {
                Console.Write("Node.js NOT Found.");
                startDownload("https://nodejs.org/dist/v4.6.0/node-v4.6.0-x64.msi", "nodejs.msi");
                BuildApplication(nodeInstall, buildApp);
                AutocloseConsole(5);
            }
        }

        private static void AutocloseConsole(int timeS)
        {
            Console.Clear();
            Console.Write("Application will exit in");
            for (int i = timeS; i > -1; i--)
            {
                Console.CursorLeft = 25;
                Console.Write(i);
                Console.CursorLeft = 27;
                Console.Write("Seconds");
                Thread.Sleep(1000);
            }
            Environment.ExitCode = 0;
        }

        public static void InstallNodeJS(Process nodeInstall, Process buildApp)
        {
            Console.Write("Starting Node.js Installer...");
            Process installer = Process.Start("nodejs.msi");
            installer.WaitForExit();
        }
        public static void BuildApplication(Process nodeInstall, Process buildApp)
        {
            Console.Write("Node.js Found");
            nodeInstall = ExecuteCommand("npm install");
            Console.Clear();
            loadingSymbol(nodeInstall, "Installing node dependencies... ");
            buildApp = ExecuteCommand("npm run dist");
            Console.Clear();
            loadingSymbol(buildApp, "Building Application... ");
            Console.Write("Build Succesful!");
            Thread.Sleep(1000);
            AutocloseConsole(5);
        }
        private static void startDownload(string uri, string filename)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(downloadCompleted);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downloadProgressChanged);
            Console.Clear();
            Console.Write("Downloading Node.js... ");
            webClient.DownloadFileAsync(new Uri(uri), filename);
        }
        private static void downloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage > 0 && e.ProgressPercentage < 100)
            {
                Console.CursorLeft = 23;
                Console.Write(e.ProgressPercentage + "%          ");
                Console.CursorLeft = 23;
            }
        }
        private static void downloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Console.Clear();
            Console.Write("Download Completed!");
            Thread.Sleep(1000);
            Console.Clear();
            InstallNodeJS(nodeInstall, buildApp);
        }
        public static bool ExistsOnPath(string fileName)
        {
            return GetFullPath(fileName) != null;
        }
        public static string GetFullPath(string fileName)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(';'))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                    return fullPath;
            }
            return null;
        }
        static Process ExecuteCommand(string command)
        {
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;

            process = Process.Start(processInfo);

            return process;
        }
        static void loadingSymbol(Process process, string text = "")
        {
            int sleepTime = 100;
            while (!process.HasExited)
            {
                Console.Write(text + "|");
                Thread.Sleep(sleepTime);
                Console.Clear();
                Console.Write(text + "/");
                Thread.Sleep(sleepTime);
                Console.Clear();
                Console.Write(text + "-");
                Thread.Sleep(sleepTime);
                Console.Clear();
                Console.Write(text + @"\");
                Thread.Sleep(sleepTime);
                Console.Clear();
                Console.Write(text + "|");
                Thread.Sleep(sleepTime);
                Console.Clear();
                Console.Write(text + "/");
                Thread.Sleep(sleepTime);
                Console.Clear();
                Console.Write(text + "-");
                Thread.Sleep(sleepTime);
                Console.Clear();
                Console.Write(text + @"\");
                Thread.Sleep(sleepTime);
                Console.Clear();
            }
        }
    }
}
