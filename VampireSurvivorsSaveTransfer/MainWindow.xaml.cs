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
using Microsoft.Win32;
using Newtonsoft.Json;
using VampireSurvivorsSaveTransfer.Data;
using Path = System.IO.Path;

namespace VampireSurvivorsSaveTransfer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The Steam App ID for Vampire Survivors
        /// </summary>
        private const int VampireSurvivorsSteamAppId = 1794680;

        /// <summary>
        /// The Xbox App ID for Vampire Survivors
        /// </summary>
        private const string VampireSurvivorsXboxName = "poncle.VampireSurvivors_9pv5cyp4vwdsr";

        /// <summary>
        /// Identifies whether Steam is installed on this computer
        /// </summary>
        private bool SteamInstalled { get; set; }

        /// <summary>
        /// The path to where Steam is installed on this computer
        /// </summary>
        private string SteamInstallPath { get; set; }

        /// <summary>
        /// The 64 bit ID for the Steam account which has played Vampire Survivors
        /// </summary>
        private long SteamId64 { get; set; }

        /// <summary>
        /// The 32 bit ID for the Steam account which has played Vampire Survivors
        /// </summary>
        private string SteamId { get; set; }

        /// <summary>
        /// The ID for the Steam account which has played Vampire Survivors
        /// </summary>
        private long SteamAccountId { get; set; }

        /// <summary>
        /// The name of the Steam account which has played Vampire Survivors
        /// </summary>
        private string SteamAccountName { get; set; }

        /// <summary>
        /// The persona name of the Steam account which has played Vampire Survivors
        /// </summary>
        private string SteamPersonaAccountName { get; set; }

        /// <summary>
        /// Identifies whether Vampire Survivors from Steam is installed on this computer
        /// </summary>
        private bool SteamVampireSurvivorsInstalled { get; set; }

        /// <summary>
        /// The path to where Vampire Survivors from Steam is installed on this computer
        /// </summary>
        private string SteamVampireSurvivorsInstallPath { get; set; }

        /// <summary>
        /// Identifies whether Vampire Survivors from Steam has been played on this computer
        /// </summary>
        private bool SteamPlayedVampireSurvivors { get; set; }

        /// <summary>
        /// The date and time when Vampire Survivors from Steam was last saved on this computer
        /// </summary>
        private DateTime SteamLastSaveDateVampireSurvivors { get; set; }

        /// <summary>
        /// The path to where Vampire Survivors from Steam saves its save data on this computer for the current user
        /// </summary>
        private string SteamSaveDataMainPath { get; set; }

        /// <summary>
        /// The first path in AppData where Vampire Survivors from Steam saves its save data on this computer for the current user
        /// </summary>
        private string SteamSaveDataAppDataPath1 { get; set; }

        /// <summary>
        /// The second path in AppData where Vampire Survivors from Steam saves its save data on this computer for the current user
        /// </summary>
        private string SteamSaveDataAppDataPath2 { get; set; }

        /// <summary>
        /// The third path in AppData where Vampire Survivors from Steam saves its save data on this computer for the current user
        /// </summary>
        private string SteamSaveDataAppDataPath3 { get; set; }

        /// <summary>
        /// Identifies whether Vampire Survivors from the Microsoft Store is installed on this computer
        /// </summary>
        private bool XboxInstalled { get; set; }

        /// <summary>
        /// The path to where Vampire Survivors from the Microsoft Store is installed on this computer
        /// </summary>
        private string XboxInstallPath { get; set; }

        /// <summary>
        /// The name of the Xbox account which has played Vampire Survivors
        /// </summary>
        private string XboxAccountName { get; set; }

        /// <summary>
        /// Identifies whether Vampire Survivors from the Microsoft Store has been played on this computer
        /// </summary>
        private bool XboxPlayedVampireSurvivors { get; set; }

        /// <summary>
        /// The date and time when Vampire Survivors from the Microsoft Store was last saved on this computer
        /// </summary>
        private DateTime XboxLastSaveDateVampireSurvivors { get; set; }

        /// <summary>
        /// The path to where Vampire Survivors from the Microsoft Store saves its save data on this computer for the current user
        /// </summary>
        private string XboxSaveDataPath { get; set; }

        /// <summary>
        /// The identifier used to identify a save data backup
        /// </summary>
        private string SaveDataBackupIdentifier { get; set; }

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

            if (!SteamPlayedVampireSurvivors)
            {
                MessageBox.Show("Vampire Survivors from Steam has not been played on this computer. Please play Vampire Survivors from Steam and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!XboxPlayedVampireSurvivors)
            {
                MessageBox.Show("Vampire Survivors from the Microsoft Store has not been played on this computer. Please play Vampire Survivors from the Microsoft Store and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you would like to copy the save file from Vampire Survivors from the Microsoft Store to Vampire Survivors from Steam?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                SaveDataBackupIdentifier = DateTime.Now.ToString("yyyyMMddHHmmss");
                
                CopyXboxSaveToSteam(SteamSaveDataAppDataPath1);
                CopyXboxSaveToSteam(SteamSaveDataAppDataPath2);
                CopyXboxSaveToSteam(SteamSaveDataAppDataPath3);
                CopyXboxSaveToSteamMainFolder();

                MessageBox.Show("The save file from Vampire Survivors from the Microsoft Store has been copied to Vampire Survivors from Steam.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void MainWindow_OnInitialized(object sender, EventArgs e)
        {
            AcceptancePrompt acceptancePrompt = new AcceptancePrompt();

            if (acceptancePrompt.ShowDialog() != true)
            {
                Close();
                return;
            }

            GetSteamInstallPath();

            if (!string.IsNullOrEmpty(SteamInstallPath))
            {
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
                            SteamVampireSurvivorsInstallPath = path2.Replace("\\\\", "\\");
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
                        long accountId64 = long.Parse(line.Trim().Replace("\"", ""));

                        long accountId = ConvertSteamID64ToAccountId(accountId64);

                        if (Directory.Exists(Path.Combine(SteamInstallPath, $"userdata\\{accountId}\\{VampireSurvivorsSteamAppId}")))
                        {
                            SteamId64 = accountId64;
                            SteamId = ConvertSteamID64ToSteamID(accountId64);
                            SteamAccountId = accountId;

                            if (File.Exists(Path.Combine(SteamInstallPath, $"userdata\\{accountId}\\{VampireSurvivorsSteamAppId}\\remote\\SaveData")))
                            {
                                SaveData saveData = ReadSaveFile(Path.Combine(SteamInstallPath, $"userdata\\{accountId}\\{VampireSurvivorsSteamAppId}\\remote\\SaveData"));

                                SteamPlayedVampireSurvivors = saveData != null;
                            }

                            break;
                        }
                    }
                }

                if (SteamAccountId != 0)
                {
                    bool read = false;

                    foreach (string line in lines)
                    {
                        if (!read)
                        {
                            if (regexSteamId.IsMatch(line.Trim()))
                            {
                                long accountId64 = long.Parse(line.Trim().Replace("\"", ""));

                                if (accountId64 == SteamId64)
                                {
                                    read = true;
                                }
                            }
                        }
                        else
                        {
                            if (regexSteamId.IsMatch(line.Trim()))
                            {
                                read = false;
                            }
                            else
                            {
                                if (line.Contains("PersonaName"))
                                {
                                    string[] split = line.Split('"');
                                    string name = split[3];
                                    SteamPersonaAccountName = name;
                                }
                                else if (line.Contains("AccountName"))
                                {
                                    string[] split = line.Split('"');
                                    string name = split[3];
                                    SteamAccountName = name;
                                }
                            }
                        }
                    }
                }
            }
            
            XboxInstalled = IsXboxInstalled();

            TextBlockXboxInstalled.Text = XboxInstalled ? "Yes" : "No";

            if (XboxInstalled)
            {
                TextBlockXboxInstallPath.Text = XboxInstallPath;

                CheckIfHasPlayedXbox();

                TextBlockXboxPlayed.Text = XboxPlayedVampireSurvivors ? "Yes" : "No";
                TextBlockXboxAccountName.Text = XboxAccountName;
            }
            else
            {
                TextBlockXboxAccountName.Text = "-";
            }
            
            TextBlockSteamInstalled.Text = SteamInstalled ? "Yes" : "No";
            TextBlockSteamInstallPath.Text = SteamInstalled ? SteamInstallPath : "-";

            if (SteamInstalled)
            {
                TextBlockSteamAccountName.Text = !string.IsNullOrEmpty(SteamPersonaAccountName) ? $"{SteamAccountName} ({SteamPersonaAccountName})" : SteamAccountName;
            }
            else
            {
                TextBlockSteamAccountName.Text = "-";
            }

            TextBlockSteamGameInstalled.Text = SteamVampireSurvivorsInstalled ? "Yes" : "No";
            TextBlockSteamGameInstallPath.Text = SteamVampireSurvivorsInstalled ? SteamVampireSurvivorsInstallPath : "-";
            TextBlockSteamPlayed.Text = SteamPlayedVampireSurvivors ? "Yes" : "No";

            if (SteamPlayedVampireSurvivors)
            {
                SteamSaveDataMainPath = Path.Combine(SteamInstallPath, "userdata", SteamAccountId.ToString(), VampireSurvivorsSteamAppId.ToString(), "remote");
                SteamSaveDataAppDataPath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Vampire_Survivors", "saves");
                SteamSaveDataAppDataPath2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"Vampire_Survivors_{SteamAccountId}");
                SteamSaveDataAppDataPath3 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Vampire_Survivors_Data");

                if (!Directory.Exists(SteamSaveDataMainPath))
                {
                    SteamSaveDataMainPath = null;
                }

                if (!Directory.Exists(SteamSaveDataAppDataPath1))
                {
                    SteamSaveDataAppDataPath1 = null;
                }

                if (!Directory.Exists(SteamSaveDataAppDataPath2))
                {
                    SteamSaveDataAppDataPath2 = null;
                }

                if (!Directory.Exists(SteamSaveDataAppDataPath3))
                {
                    SteamSaveDataAppDataPath3 = null;
                }
            }
        }

        /// <summary>
        /// Gets the path to the Steam install location
        /// </summary>
        private void GetSteamInstallPath()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Valve\Steam");
            object value = key?.GetValue("InstallPath");
            string path = value?.ToString();

            SteamInstallPath = path;
        }

        /// <summary>
        /// Reads the contents of the Vampire Survivors save data file
        /// </summary>
        /// <param name="path">The path to the save data file</param>
        /// <returns>Returns the contents of the Vampire Survivors save data file</returns>
        private SaveData ReadSaveFile(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            string data = File.ReadAllText(path);

            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(data);

            return saveData;
        }

        /// <summary>
        /// Copies the Xbox (Microsoft Store) save data to the Steam save data location
        /// </summary>
        /// <param name="steamPath">The path to the Steam save data location</param>
        private void CopyXboxSaveToSteam(string steamPath)
        {
            if (File.Exists(Path.Combine(steamPath, "SaveData.sav")))
            {
                File.Move(Path.Combine(steamPath, "SaveData.sav"), Path.Combine(steamPath, $"{SaveDataBackupIdentifier}_SaveData.sav.old"));
            }

            File.Copy(XboxSaveDataPath, Path.Combine(steamPath, "SaveData.sav"));
        }

        /// <summary>
        /// Copies the Xbox (Microsoft Store) save data to the main Steam save data location
        /// </summary>
        private void CopyXboxSaveToSteamMainFolder()
        {
            if (File.Exists(Path.Combine(SteamSaveDataMainPath, "SaveData")))
            {
                File.Move(Path.Combine(SteamSaveDataMainPath, "SaveData"), Path.Combine(SteamSaveDataMainPath, $"{SaveDataBackupIdentifier}_SaveData.old"));
            }

            File.Copy(XboxSaveDataPath, Path.Combine(SteamSaveDataMainPath, "SaveData"));
        }

        /// <summary>
        /// Checks to see if the Xbox (Microsoft Store) version of Vampire Survivors is installed on this computer
        /// </summary>
        /// <returns>Returns whether the Xbox (Microsoft Store) version of Vampire Survivors is installed on this computer</returns>
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

        /// <summary>
        /// Checks to see if the user has played the Xbox (Microsoft Store) version of Vampire Survivors
        /// </summary>
        private void CheckIfHasPlayedXbox()
        {
            if (!XboxInstalled)
            {
                return; 
            }

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages", VampireSurvivorsXboxName, "SystemAppData", "wgs");

            if (Directory.Exists(path))
            {
                foreach (string child in Directory.GetDirectories(path))
                {
                    string[] files = Directory.GetFiles(child);

                    foreach (string file in files)
                    {
                        FileInfo fileInfo = new FileInfo(file);

                        if (fileInfo.Name == "containers.index")
                        {
                            foreach (string child2 in Directory.GetDirectories(child))
                            {
                                string[] saveFiles = Directory.GetFiles(child2);

                                foreach (string saveFile in saveFiles)
                                {
                                    try
                                    {
                                        string data = File.ReadAllText(saveFile);

                                        SaveData saveData = JsonConvert.DeserializeObject<SaveData>(data);

                                        if (saveData != null)
                                        {
                                            XboxPlayedVampireSurvivors = true;
                                            XboxAccountName = saveData.PlayerNickname;
                                            XboxSaveDataPath = saveFile;

                                            return;
                                        }
                                    }
                                    catch
                                    {
                                        // Do nothing
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Converts a Steam ID 64 to a Steam ID
        /// </summary>
        /// <param name="source">The source Steam ID 64</param>
        /// <returns>Returns the Steam ID</returns>
        private string ConvertSteamID64ToSteamID(long source)
        {
            long authServer = (source - 76561197960265728) & 1;
            long authId = (source - 76561197960265728 - authServer) / 2;
            
            return $"STEAM_0:{authServer}:{authId}";
        }

        /// <summary>
        /// Converts a Steam ID 64 to a Steam account ID
        /// </summary>
        /// <param name="source">The source Steam ID 64</param>
        /// <returns>Returns the Steam account ID</returns>
        private long ConvertSteamID64ToAccountId(long source)
        {
            long id = Convert.ToInt64(source.ToString().Substring(3));

            return id - 61197960265728;
        }
    }
}
