using System;
using Terminals.Security;
using System.IO;

namespace Terminals.Configuration
{
    internal static partial class Settings
    {
        #region Terminals Version

        public static Version ConfigVersion
        {
            get
            {
                string configVersion = GetSection().ConfigVersion;
                if (configVersion != String.Empty)
                    return new Version(configVersion);

                return null;
            }

            set
            {
                GetSection().ConfigVersion = value.ToString();
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region General tab settings

        public static bool NeverShowTerminalsWindow
        {
            get
            {
                return GetSection().NeverShowTerminalsWindow;
            }

            set
            {
                GetSection().NeverShowTerminalsWindow = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool ShowUserNameInTitle
        {
            get
            {
                return GetSection().ShowUserNameInTitle;
            }

            set
            {
                GetSection().ShowUserNameInTitle = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool ShowInformationToolTips
        {
            get
            {
                return GetSection().ShowInformationToolTips;
            }

            set
            {
                GetSection().ShowInformationToolTips = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool ShowFullInformationToolTips
        {
            get
            {
                return GetSection().ShowFullInformationToolTips;
            }

            set
            {
                GetSection().ShowFullInformationToolTips = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool SingleInstance
        {
            get
            {
                return GetSection().SingleInstance;
            }

            set
            {
                GetSection().SingleInstance = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool ShowConfirmDialog
        {
            get
            {
                return GetSection().ShowConfirmDialog;
            }

            set
            {
                GetSection().ShowConfirmDialog = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool SaveConnectionsOnClose
        {
            get
            {
                return GetSection().SaveConnectionsOnClose;
            }

            set
            {
                GetSection().SaveConnectionsOnClose = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool MinimizeToTray
        {
            get
            {
                return GetSection().MinimizeToTray;
            }

            set
            {
                GetSection().MinimizeToTray = value;
                SaveImmediatelyIfRequested();
            }
        }

        // Validate server names
        public static bool ForceComputerNamesAsURI
        {
            get
            {
                return GetSection().ForceComputerNamesAsURI;
            }

            set
            {
                GetSection().ForceComputerNamesAsURI = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool WarnOnConnectionClose
        {
            get
            {
                return GetSection().WarnOnConnectionClose;
            }

            set
            {
                GetSection().WarnOnConnectionClose = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool AutoCaseTags
        {
            get
            {
                return GetSection().AutoCaseTags;
            }

            set
            {
                GetSection().AutoCaseTags = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static string DefaultDesktopShare
        {
            get
            {
                return GetSection().DefaultDesktopShare;
            }

            set
            {
                GetSection().DefaultDesktopShare = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static int PortScanTimeoutSeconds
        {
            get
            {
                return GetSection().PortScanTimeoutSeconds;
            }

            set
            {
                GetSection().PortScanTimeoutSeconds = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Execute Before Connect tab settings

        public static bool ExecuteBeforeConnect
        {
            get
            {
                return GetSection().ExecuteBeforeConnect;
            }

            set
            {
                GetSection().ExecuteBeforeConnect = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static string ExecuteBeforeConnectCommand
        {
            get
            {
                return GetSection().ExecuteBeforeConnectCommand;
            }

            set
            {
                GetSection().ExecuteBeforeConnectCommand = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static string ExecuteBeforeConnectArgs
        {
            get
            {
                return GetSection().ExecuteBeforeConnectArgs;
            }

            set
            {
                GetSection().ExecuteBeforeConnectArgs = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static string ExecuteBeforeConnectInitialDirectory
        {
            get
            {
                return GetSection().ExecuteBeforeConnectInitialDirectory;
            }

            set
            {
                GetSection().ExecuteBeforeConnectInitialDirectory = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool ExecuteBeforeConnectWaitForExit
        {
            get
            {
                return GetSection().ExecuteBeforeConnectWaitForExit;
            }

            set
            {
                GetSection().ExecuteBeforeConnectWaitForExit = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Security

        internal static string MasterPasswordHash
        {
            get
            {
                return GetSection().TerminalsPassword; 
            }
            private set
            {
                string newHash = string.Empty;
                if (!string.IsNullOrEmpty(value))
                    newHash = PasswordFunctions.ComputeMasterPasswordHash(value);
                GetSection().TerminalsPassword = newHash;
            }
        }

        /// <summary>
        /// This updates all stored passwords and assignes new key material in config section.
        /// </summary>
        internal static void UpdateConfigurationPasswords(string newMasterPassword)
        {
            MasterPasswordHash = newMasterPassword;
            UpdateStoredPasswords(newMasterPassword);
            SaveImmediatelyIfRequested();
        }

        private static void UpdateStoredPasswords(string newMasterPassword)
        {
            TerminalsConfigurationSection configSection = GetSection();
            string newKeyMaterial = PasswordFunctions.CalculateMasterPasswordKey(newMasterPassword);
            configSection.EncryptedDefaultPassword = PasswordFunctions.EncryptPassword(DefaultPassword, newKeyMaterial);
            configSection.EncryptedAmazonAccessKey = PasswordFunctions.EncryptPassword(AmazonAccessKey, newKeyMaterial);
            configSection.EncryptedAmazonSecretKey = PasswordFunctions.EncryptPassword(AmazonSecretKey, newKeyMaterial);
            configSection.EncryptedConnectionString = PasswordFunctions.EncryptPassword(ConnectionString, newKeyMaterial);
            configSection.DatabaseMasterPasswordHash = PasswordFunctions.EncryptPassword(DatabaseMasterPassword, newKeyMaterial);
        }

        #endregion

        #region Security tab settings

        public static string DefaultDomain
        {
            get
            {
                return GetSection().DefaultDomain;
            }

            set
            {
                GetSection().DefaultDomain = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static string DefaultUsername
        {
            get
            {
                return GetSection().DefaultUsername;
            }

            set
            {
                GetSection().DefaultUsername = value;
                SaveImmediatelyIfRequested();
            }
        }

        internal static string DefaultPassword
        {
            get
            {
                string encryptedDefaultPassword = GetSection().EncryptedDefaultPassword;
                return PasswordFunctions.DecryptPassword(encryptedDefaultPassword);
            }

            set
            {
                GetSection().EncryptedDefaultPassword = PasswordFunctions.EncryptPassword(value);
                SaveImmediatelyIfRequested();
            }
        }

        public static bool UseAmazon
        {
            get
            {
                return GetSection().UseAmazon;
            }

            set
            {
                GetSection().UseAmazon = value;
                SaveImmediatelyIfRequested();
            }
        }

        internal static string AmazonAccessKey
        {
            get
            {
                string encryptedAmazonAccessKey = GetSection().EncryptedAmazonAccessKey;
                return PasswordFunctions.DecryptPassword(encryptedAmazonAccessKey);
            }

            set
            {
                GetSection().EncryptedAmazonAccessKey = PasswordFunctions.EncryptPassword(value);
                SaveImmediatelyIfRequested();
            }
        }

        internal static string AmazonSecretKey
        {
            get
            {
                string encryptedAmazonSecretKey = GetSection().EncryptedAmazonSecretKey;
                return PasswordFunctions.DecryptPassword(encryptedAmazonSecretKey);
            }

            set
            {
                GetSection().EncryptedAmazonSecretKey = PasswordFunctions.EncryptPassword(value);
                SaveImmediatelyIfRequested();
            }
        }

        public static string AmazonBucketName
        {
            get
            {
                return GetSection().AmazonBucketName;
            }

            set
            {
                GetSection().AmazonBucketName = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Flickr tab settings

        public static string FlickrToken
        {
            get
            {
                return GetSection().FlickrToken;
            }

            set
            {
                GetSection().FlickrToken = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Proxy tab settings

        public static bool UseProxy
        {
            get
            {
                return GetSection().UseProxy;
            }

            set
            {
                GetSection().UseProxy = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static string ProxyAddress
        {
            get
            {
                return GetSection().ProxyAddress;
            }

            set
            {
                GetSection().ProxyAddress = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static int ProxyPort
        {
            get
            {
                return GetSection().ProxyPort;
            }

            set
            {
                GetSection().ProxyPort = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Screen capture tab settings

        public static bool EnableCaptureToClipboard
        {
            get
            {
                return GetSection().EnableCaptureToClipboard;
            }

            set
            {
                GetSection().EnableCaptureToClipboard = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool EnableCaptureToFolder
        {
            get
            {
                return GetSection().EnableCaptureToFolder;
            }

            set
            {
                GetSection().EnableCaptureToFolder = value;
                SaveImmediatelyIfRequested();
            }
        }

        internal static bool EnabledCaptureToFolderAndClipBoard
        {
            get { return EnableCaptureToClipboard || EnableCaptureToFolder; }
        }

        public static bool AutoSwitchOnCapture
        {
            get
            {
                return GetSection().AutoSwitchOnCapture;
            }

            set
            {
                GetSection().AutoSwitchOnCapture = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static string CaptureRoot
        {
            get
            {
                string root = GetSection().CaptureRoot;
                if (string.IsNullOrEmpty(root))
                    root = GetDefaultCaptureRootDirectory();

                return EnsureCaptureDirectory(root);
            }

            set
            {
                GetSection().CaptureRoot = value;
                SaveImmediatelyIfRequested();
            }
        }

        private static string EnsureCaptureDirectory(string root)
        {
            try
            {
                if (!Directory.Exists(root))
                {
                    Logging.Log.Info(string.Format("Capture root folder does not exist:{0}. Lets try to create it now.", root));
                    Directory.CreateDirectory(root);
                }
            }
            catch (Exception exception)
            {
                root = GetDefaultCaptureRootDirectory();
                string logMessage = string.Format("Capture root could not be created, set it to the default value : {0}", root);
                Logging.Log.Error(logMessage, exception);
                SwitchToDefaultDirectory(root);
            }

            return root;
        }

        private static void SwitchToDefaultDirectory(string defaultRoot)
        {
            try
            {
                Directory.CreateDirectory(defaultRoot);
                CaptureRoot = defaultRoot;
            }
            catch (Exception exception)
            {
                Logging.Log.Error(@"Capture root could not be created again. Abort!", exception);
            }
        }

        private static string GetDefaultCaptureRootDirectory()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Terminals Captures");
        }

        #endregion

        #region More tab settings

        public static bool EnableFavoritesPanel
        {
            get
            {
                return GetSection().EnableFavoritesPanel;
            }

            set
            {
                GetSection().EnableFavoritesPanel = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool EnableGroupsMenu
        {
            get
            {
                return GetSection().EnableGroupsMenu;
            }

            set
            {
                GetSection().EnableGroupsMenu = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool AutoExapandTagsPanel
        {
            get
            {
                return GetSection().AutoExapandTagsPanel;
            }

            set
            {
                GetSection().AutoExapandTagsPanel = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static SortProperties DefaultSortProperty
        {
            get
            {
                TerminalsConfigurationSection config = GetSection();
                if (config != null)
                {
                    string dsp = config.DefaultSortProperty;
                    SortProperties prop = (SortProperties)Enum.Parse(typeof(SortProperties), dsp);
                    return prop;
                }

                return SortProperties.ConnectionName;
            }

            set
            {
                GetSection().DefaultSortProperty = value.ToString();
                SaveImmediatelyIfRequested();
            }
        }

        public static bool Office2007BlackFeel
        {
            get
            {
                return GetSection().Office2007BlackFeel;
            }

            set
            {
                GetSection().Office2007BlackFeel = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool Office2007BlueFeel
        {
            get
            {
                return GetSection().Office2007BlueFeel;
            }

            set
            {
                GetSection().Office2007BlueFeel = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Vnc settings

        public static bool VncAutoScale
        {
            get
            {
                return GetSection().VncAutoScale;
            }

            set
            {
                GetSection().VncAutoScale = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool VncViewOnly
        {
            get
            {
                return GetSection().VncViewOnly;
            }

            set
            {
                GetSection().VncViewOnly = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static int VncDisplayNumber
        {
            get
            {
                return GetSection().VncDisplayNumber;
            }

            set
            {
                GetSection().VncDisplayNumber = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Mainform control settings

        public static int FavoritePanelWidth
        {
            get
            {
                return GetSection().FavoritePanelWidth;
            }

            set
            {
                GetSection().FavoritePanelWidth = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool ShowFavoritePanel
        {
            get
            {
                return GetSection().ShowFavoritePanel;
            }

            set
            {
                GetSection().ShowFavoritePanel = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static ToolStripSettings ToolbarSettings
        {
            get
            {
                return ToolStripSettings.Load();
            }
            set
            {
                value.Save();
            }
        }

        public static bool ToolbarsLocked
        {
            get
            {
                return GetSection().ToolbarsLocked;
            }

            set
            {
                GetSection().ToolbarsLocked = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Startup settings

        public static string UpdateSource
        {
            get
            {
                return GetSection().UpdateSource;
            }

            set
            {
                GetSection().UpdateSource = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool ShowWizard
        {
            get
            {
                return GetSection().ShowWizard;
            }

            set
            {
                GetSection().ShowWizard = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static string PsexecLocation
        {
            get
            {
                return GetSection().PsexecLocation;
            }

            set
            {
                GetSection().PsexecLocation = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static string SavedCredentialsLocation
        {
            get
            {
                return GetSection().SavedCredentialsLocation;
            }

            set
            {
                GetSection().SavedCredentialsLocation = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static string SavedFavoritesFileLocation
        {
            get
            {
                return GetSection().SavedFavoritesFileLocation;
            }

            set
            {
                GetSection().SavedFavoritesFileLocation = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region MRU lists

        public static string[] MRUServerNames
        {
            get
            {
                return GetSection().ServersMRU.ToSortedArray();
            }
        }

        public static string[] MRUDomainNames
        {
            get
            {
                return GetSection().DomainsMRU.ToSortedArray();
            }
        }

        public static string[] MRUUserNames
        {
            get
            {
                return GetSection().UsersMRU.ToSortedArray();
            }
        }

        #endregion

        #region Tags/Favorite lists Settings
        
        public static string ExpandedFavoriteNodes
        {
            get
            {
                return GetSection().ExpandedFavoriteNodes;
            }

            set
            {
                GetSection().ExpandedFavoriteNodes = value;
                SaveImmediatelyIfRequested();
            }
        }
        public static string ExpandedHistoryNodes
        {
            get
            {
                return GetSection().ExpandedHistoryNodes;
            }

            set
            {
                GetSection().ExpandedHistoryNodes = value;
                SaveImmediatelyIfRequested();
            }
        }
        #endregion

        #region Persistence File/Sql database

        /// <summary>
        /// Gets or sets encrypted entity framework connection string
        /// </summary>
        internal static string ConnectionString
        {
            get
            {
              string encryptedConnectionString = GetSection().EncryptedConnectionString;
              return PasswordFunctions.DecryptPassword(encryptedConnectionString);
            }

            set
            {
                GetSection().EncryptedConnectionString = PasswordFunctions.EncryptPassword(value);
                SaveImmediatelyIfRequested();
            }
        }

        internal static string DatabaseMasterPassword
        {
            get
            {
                string databaseMasterPasswordHash = GetSection().DatabaseMasterPasswordHash;
                return PasswordFunctions.DecryptPassword(databaseMasterPasswordHash);
            }
            set
            {
                GetSection().DatabaseMasterPasswordHash = PasswordFunctions.EncryptPassword(value);
                SaveImmediatelyIfRequested();
            }
        }

        /// <summary>
        /// Gets or sets the value identifing the persistance.
        /// 0 byfault - file persisted data, 1 - SQL database
        /// </summary>
        internal static byte PersistenceType
        {
            get
            {
                return GetSection().PersistenceType;
            }

            set
            {
                GetSection().PersistenceType = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Public

        public static string ToTitleCase(string name)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
        }

        public static void AddServerMRUItem(string name)
        {
            GetSection().ServersMRU.AddByName(name);
            SaveImmediatelyIfRequested();
        }

        public static void AddDomainMRUItem(string name)
        {
            GetSection().DomainsMRU.AddByName(name);
            SaveImmediatelyIfRequested();
        }

        public static void AddUserMRUItem(string name)
        {
            GetSection().UsersMRU.AddByName(name);
            SaveImmediatelyIfRequested();
        }

        public static void AddConnection(string name)
        {
            GetSection().SavedConnections.AddByName(name);
            SaveImmediatelyIfRequested();
        }

        public static SpecialCommandConfigurationElementCollection SpecialCommands
        {
            get
            {
                return GetSection().SpecialCommands;
            }

            set
            {
                GetSection().SpecialCommands = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static void CreateSavedConnectionsList(string[] names)
        {
            GetSection().SavedConnections.Clear();
            SaveImmediatelyIfRequested();
            foreach (string name in names)
            {
                AddConnection(name);
            }
        }

        public static void ClearSavedConnectionsList()
        {
            GetSection().SavedConnections.Clear();
            SaveImmediatelyIfRequested();
        }

        public static string[] SavedConnections
        {
            get
            {
                return GetSection().SavedConnections.ToList().ToArray();
            }
        }

        public static SSHClient.KeysSection SSHKeys
        {
            get
            {
                SSHClient.KeysSection keys = Config.Sections["SSH"] as SSHClient.KeysSection;
                if (keys == null)
                {
                    // The section wasn't found, so add it.
                    keys = new SSHClient.KeysSection();
                    Config.Sections.Add("SSH", keys);
                }

                return keys;
            }

            ////set
            ////{
            ////    Configuration configuration = Config;
            ////    configuration.Sections["SSH"] = value;
            ////    if(!DelayConfigurationSave) configuration.Save();
            ////}
        }

        #endregion
    }
}
