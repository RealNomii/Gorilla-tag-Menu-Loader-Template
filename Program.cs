using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace MenuLoader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "Menu Loader";

            while (true)
            {
                DisplayAsciiArt();
                DisplayMenu();

                string choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        DownloadFile();
                        break;
                    case "2":
                        OpenGitHub();
                        break;
                    case "3":
                        OpenGame();
                        break;
                    case "4":
                        ExitProgram();
                        return;
                    default:
                        ShowErrorMessage("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine("\nReturning to the menu in 5 seconds...");
                Thread.Sleep(5000);
                Console.Clear();
            }
        }

        private static void DisplayMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n=============================================================");
            Console.WriteLine("Welcome to the AnotherMenu's Mod Manager!");
            Console.WriteLine("=============================================================");
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. Download Plugin");
            Console.WriteLine("2. Open GitHub Repository");
            Console.WriteLine("3. Launch Game");
            Console.WriteLine("4. Exit");
            Console.WriteLine("=============================================================");
            Console.Write("Your choice: ");
            Console.ResetColor();
        }

        private static void DownloadFile()
        {
            string apiEndpoint = "https://api.github.com/repos/User/Repository/releases/latest";
            string pluginDirectory = "C:\\Path\\To\\Plugins";
            string pluginName = "Plugin.dll";

            if (!Directory.Exists(pluginDirectory))
            {
                Console.WriteLine("\n[INFO] Directory not found. Creating: " + pluginDirectory);
                Directory.CreateDirectory(pluginDirectory);
            }

            string pluginPath = Path.Combine(pluginDirectory, pluginName);

            try
            {
                Console.WriteLine("\n[INFO] Fetching the latest release...");

                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers.Add("User-Agent", "MenuLoaderTool");
                    string json = webClient.DownloadString(apiEndpoint);
                    JObject releaseData = JObject.Parse(json);

                    JArray assets = (JArray)releaseData["assets"];
                    string downloadUrl = null;

                    foreach (var asset in assets)
                    {
                        if (asset["name"]?.ToString() == pluginName)
                        {
                            downloadUrl = asset["browser_download_url"]?.ToString();
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(downloadUrl))
                    {
                        ShowErrorMessage($"File '{pluginName}' not found in the latest release.");
                        return;
                    }

                    Console.WriteLine("[INFO] Downloading " + pluginName + "...");
                    webClient.DownloadFile(downloadUrl, pluginPath);
                    ShowSuccessMessage($"Download complete! File saved to {pluginPath}");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("An error occurred: " + ex.Message);
            }
        }

        private static void OpenGitHub()
        {
            string url = "https://github.com/User/Repository";

            try
            {
                Console.WriteLine("\n[INFO] Opening GitHub repository...");
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Failed to open GitHub: " + ex.Message);
            }
        }

        private static void OpenGame()
        {
            string gamePath = "\"C:\\Program Files (x86)\\Steam\\steamapps\\common\\Gorilla Tag\\Gorilla Tag.exe\"";

            if (File.Exists(gamePath))
            {
                try
                {
                    Console.WriteLine("\n[INFO] Launching the game...");
                    Process.Start(gamePath);
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("Failed to launch the game: " + ex.Message);
                }
            }
            else
            {
                ShowErrorMessage("Game not found. Please check the path.");
            }
        }

        private static void ExitProgram()
        {
            Console.Clear();
            ShowSuccessMessage("Exiting the program. Goodbye!");
            Thread.Sleep(2000);
        }

        private static void DisplayAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("  __  __ _____ _   _ _   _      _     ___    _    ____  _____ ____  ");
            Console.WriteLine(" |  \\/  | ____| \\ | | | | |    | |   / _ \\  / \\  |  _ \\| ____|  _ \\ ");
            Console.WriteLine(" | |\\/| |  _| |  \\| | | | |    | |  | | | |/ _ \\ | | | |  _| | |_) |");
            Console.WriteLine(" | |  | | |___| |\\  | |_| |    | |__| |_| / ___ \\| |_| | |___|  _ < ");
            Console.WriteLine(" |_|  |_|_____|_| \\_|\\___/     |_____\\___/_/   \\_\\____/|_____|_| \\_\\");
            Console.ResetColor();
            Console.WriteLine("\n=============================================================");

        }

        private static void ShowErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[ERROR] " + message);
            Console.ResetColor();
        }

        private static void ShowSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[SUCCESS] " + message);
            Console.ResetColor();
        }
    }
}
