using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.ApplicationModel;
using Windows.Management.Deployment;

namespace VampireSurvivorsSaveTransfer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int VampireSurvivorsSteamAppId = 1794680;
        public const string VampireSurvivorsXboxName = "poncle.VampireSurvivors_9pv5cyp4vwdsr";

        public bool SteamInstalled { get; set; }
        public string SteamInstallPath { get; set; }
        public bool SteamVampireSurvivorsInstalled { get; set; }
        public string SteamVampireSurvivorsInstallPath { get; set; }

        public bool XboxInstalled { get; set; }
        public string XboxInstallPath { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonCopySaveFromXboxToSteam_OnClick(object sender, RoutedEventArgs e)
        {
            if (!SteamInstalled)
            {
                MessageBox.Show("Steam is not installed on this computer. Please install Steam and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!SteamVampireSurvivorsInstalled)
            {
                MessageBox.Show("Vampire Survivors from Steam is not installed on this computer. Please install Vampire Survivors from Steam and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!XboxInstalled)
            {
                MessageBox.Show("Vampire Survivors from the Microsoft Store is not installed on this computer. Please install Vampire Survivors from the Microsoft Store and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MainWindow_OnInitialized(object sender, EventArgs e)
        {
            // Read a value from the registry
            var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\Valve\Steam");
            var value = key.GetValue("InstallPath");
            var path = value.ToString();

            if (path != null)
            {
                SteamInstallPath = path;
                SteamInstalled = true;

                string[] lines = File.ReadAllLines(SteamInstallPath + "\\steamapps\\libraryfolders.vdf");
                foreach (string line in lines)
                {
                    if (line.Contains("path"))
                    {
                        string[] split = line.Split('"');
                        string path2 = split[3];
                        if (File.Exists(path2 + "\\steamapps\\appmanifest_" + VampireSurvivorsSteamAppId + ".acf"))
                        {
                            SteamVampireSurvivorsInstalled = true;
                            SteamVampireSurvivorsInstallPath = path2;
                            break;
                        }
                    }
                }

                lines = File.ReadAllLines(SteamInstallPath + "\\config\\loginusers.vdf");

                Regex regexSteamId = new Regex(@"^\""\d+\""");
                foreach (string line in lines)
                {
                    if (regexSteamId.IsMatch(line.Trim()))
                    {
                        string accountId = line.Trim().Replace("\"", "");
                        Console.WriteLine(accountId);
                        Console.WriteLine(ConvertSteamID64ToSteamID(long.Parse(accountId)));
                        Console.WriteLine(ConvertSteamID64ToAccountId(long.Parse(accountId)));
                    }
                }
            }
            
            XboxInstalled = IsXboxInstalled();


        }

        private bool IsXboxInstalled()
        {
            var packageManager = new PackageManager();
            var packageList = packageManager.FindPackages();

            foreach (Package package in packageList)
            {
                if (package.Id.FamilyName == VampireSurvivorsXboxName)
                {
                    XboxInstallPath = package.InstalledLocation.Path;
                    return true;
                }
            }

            return false;
        }

        private string ConvertSteamID64ToSteamID(long source)
        {
            long authServer = (source - 76561197960265728) & 1;
            long authId = (source - 76561197960265728 - authServer) / 2;
            
            return $"STEAM_0:{authServer}:{authId}";
        }

        private long ConvertSteamID64ToAccountId(long source)
        {
            long id = Convert.ToInt64(source.ToString().Substring(3));

            return id - 61197960265728;
        }
    }
}
