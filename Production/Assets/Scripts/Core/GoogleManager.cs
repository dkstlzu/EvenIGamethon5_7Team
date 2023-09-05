using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using dkstlzu.Utility;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using MoonBunny.Dev;
using MoonBunny.UIs;
using UnityEngine;

namespace MoonBunny
{
    public enum SelectUIMethod
    {
        None = -1,
        Load,
        Save,
        Delete,
    }

    public class GoogleManager : Singleton<GoogleManager>
    {
        public LoadingUI LoadingUI;

        public bool UseSelectUI;

        private PlayGamesClientConfiguration _clientConfiguration;

        private SaveLoadSystem _legacySaveLoadSystemChecker;

        private const string SAVE_FILE_NAME = "JumpingBunnySave";
        private const string SAVE_FILE_NAME_TEST = "Test2";

        public ProgressSaveData ProgressData
        {
            get => GameManager.SaveData;
            set => GameManager.instance.SaveLoadSystem.ProgressSaveData = value;
        }

        public QuestSaveData QuestData
        {
            get => QuestManager.SaveData;
            set => QuestManager.instance.SaveLoadSystem.QuestSaveData = value;
        }

        private IPlayGamesClient _client;

        public event Action<SignInStatus> OnAuthentication;

        private void Awake()
        {
#if UNITY_EDITOR
#else
            _clientConfiguration = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.InitializeInstance(_clientConfiguration);
#endif
        }

        private void Start()
        {
            Login();
        }

        private void OnApplicationQuit()
        {
            Logout();
        }

        public void Save()
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = SaveLoadSystem.AesEncryptionKeyByte;
                aes.IV = SaveLoadSystem.AesEncryptionIVByte;

                byte[] progressTemp = Encoding.UTF8.GetBytes(JsonUtility.ToJson(ProgressData));
                byte[] questTemp = Encoding.UTF8.GetBytes(JsonUtility.ToJson(QuestData));

                byte[] totalData = new byte[progressTemp.Length + questTemp.Length + 8];

                byte[] progressLength = BitConverter.GetBytes(progressTemp.Length);
                byte[] questLength = BitConverter.GetBytes(questTemp.Length);
                
                Array.Copy(progressLength, 0, totalData, 0, sizeof(Int32));
                Array.Copy(questLength, 0, totalData, 4, sizeof(Int32));
                Array.Copy(progressTemp, 0, totalData, 8, progressTemp.Length);
                Array.Copy(questTemp, 0, totalData, 8 + progressTemp.Length, questTemp.Length);

                _loadedByte = aes.CreateEncryptor().TransformFinalBlock(totalData, 0, totalData.Length);
                
                MoonBunnyLog.print($"Save data length : {progressTemp.Length} {questTemp.Length}, lengthData {progressLength.Length} {questLength.Length}, total {totalData.Length} {_loadedByte.Length}", "GoogleManager");
            }

            SelectUIMethod = SelectUIMethod.Save;

#if DEVELOPMENT_BUILD
            OpenSavedGame(SAVE_FILE_NAME_TEST);
#else
            OpenSavedGame(SAVE_FILE_NAME);
#endif
        }

        public void Load()
        {
            SelectUIMethod = SelectUIMethod.Load;

#if DEVELOPMENT_BUILD
            OpenSavedGame(SAVE_FILE_NAME_TEST);
#else
            OpenSavedGame(SAVE_FILE_NAME);
#endif
        }

        private bool _gpgsLoginTriedBefore = false;
        public void Login()
        {
#if UNITY_EDITOR
            Social.localUser.Authenticate(ProcessAuthentication);
#else
            if (_gpgsLoginTriedBefore)
            {
                PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, PlayGamesAuthentication);
            } else
            {
                PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, PlayGamesAuthentication);
            }
            
            _gpgsLoginTriedBefore = true;
#endif
        }

        public void Logout()
        {
#if UNITY_EDITOR
#else
            PlayGamesPlatform.Instance.SignOut();
#endif
        }

        private void PlayGamesAuthentication(SignInStatus status)
        {
            MoonBunnyLog.print($"PlayGamesPlatform Authentication {status}");

            switch (status)
            {
                case SignInStatus.Success:
                    if (!GameManager.instance.useSaveSystem)
                        Load();
                    break;
                case SignInStatus.UiSignInRequired: break;
                case SignInStatus.DeveloperError: break;
                case SignInStatus.NetworkError: break;
                case SignInStatus.InternalError: break;
                case SignInStatus.Canceled: break;
                case SignInStatus.AlreadyInProgress: break;
                case SignInStatus.Failed: break;
                case SignInStatus.NotAuthenticated: break;
            }

            OnAuthentication?.Invoke(status);
        }

        private void ProcessAuthentication(bool success, string result)
        {
            MoonBunnyLog.print($"GoogleManager Authentication Success {success}, Result {result}");

            OnAuthentication?.Invoke(success ? SignInStatus.Success : SignInStatus.Canceled);
        }

        #region SelectSaveUI

        public event Action<SelectUIStatus> OnSelectUIUnselected;

        public SelectUIMethod SelectUIMethod = SelectUIMethod.Load;

        // Save Load with Select UI
        void ShowSelectUI()
        {
            uint maxNumToDisplay = 5;
            bool allowCreateNew = true;
            bool allowDelete = true;
        
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ShowSelectSavedGameUI("Select saved game", maxNumToDisplay, allowCreateNew, allowDelete, OnSavedGameSelected);
        }
        
        void OnSavedGameSelected(SelectUIStatus status, ISavedGameMetadata game)
        {
            MoonBunnyLog.print($"SelectUI Status : {status}", "GoogleManager");
        
            if (status == SelectUIStatus.SavedGameSelected)
            {
                // handle selected game save
                switch (SelectUIMethod)
                {
                    case SelectUIMethod.Load:
                        LoadGameData(game);
                        break;
                    case SelectUIMethod.Save:
                        SaveGame(game, _loadedByte, game.TotalTimePlayed);
                        break;
                    case SelectUIMethod.Delete:
                        DeleteGameData(game.Filename);
                        break;
                }
            } else { OnSelectUIUnselected?.Invoke(status); }
        }

        #endregion

        #region Get Save Data

        private bool _isOpeningSavedGame = false;
        void OpenSavedGame(string filename)
        {
            if (_isOpeningSavedGame)
            {
                MoonBunnyLog.print($"Already Opening SavedGame", "GoogleManager");
                return;
            }

            _isOpeningSavedGame = true;
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            MoonBunnyLog.print($"OpenSavedGame with file {filename}", "GoogleManager");

            if (UseSelectUI)
            {
                ShowSelectUI();
            } else
            {
                savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
            }
        }

        void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            _isOpeningSavedGame = false;
            MoonBunnyLog.print($"OnSavedGame Opened with status {status}", "GoogleManager");

            if (status == SavedGameRequestStatus.Success)
            {
                switch (SelectUIMethod)
                {
                    case SelectUIMethod.Load:
                        LoadGameData(game);
                        break;
                    case SelectUIMethod.Save:
                        SaveGame(game, _loadedByte, game.TotalTimePlayed);
                        break;
                    case SelectUIMethod.Delete:
                        DeleteGameData(game.Filename);
                        break;
                }
            } else
            {

            }
        }

        #endregion

        #region Save

        private Texture2D savedImage;

        /// <summary>
        /// Save Game with Select UI
        /// </summary>
        void SaveGame(ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime)
        {
            MoonBunnyLog.print($"SaveGame at {game.Filename} with playtime {totalPlaytime}", "GoogleManager");

            StartCoroutine(SaveWithScreenShot(game, savedData, totalPlaytime));
        }
        
        IEnumerator SaveWithScreenShot(ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
            builder = builder.WithUpdatedPlayedTime(totalPlaytime).WithUpdatedDescription("Saved game at " + DateTime.Now);
            
            yield return new WaitForEndOfFrame();
            
            // Create a 2D texture that is 1024x700 pixels from which the PNG will be
            // extracted
            savedImage = new Texture2D(1024, 700);

            // Takes the screenshot from top left hand corner of screen and maps to top
            // left hand corner of screenShot texture
            savedImage.ReadPixels(new Rect(0, 0, Screen.width, (Screen.width / 1024f) * 700), 0, 0);
            
            if (savedImage != null)
            {
                // This assumes that savedImage is an instance of Texture2D
                // and that you have already called a function equivalent to
                // getScreenshot() to set savedImage
                // NOTE: see sample definition of getScreenshot() method below
                byte[] pngData = savedImage.EncodeToPNG();
                builder = builder.WithUpdatedPngCoverImage(pngData);
            }

            SavedGameMetadataUpdate updatedMetadata = builder.Build();
            savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
        }

        void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            MoonBunnyLog.print($"SaveGame Result {status} at {game.Filename}", "GoogleManager");

            if (status == SavedGameRequestStatus.Success)
            {
                // handle reading or writing of saved game.
            } else
            {
                // handle error
            }
        }

        #endregion

        #region Read

        public event Action OnDataLoadSuccess;
        public event Action OnDataLoadFail;
        private byte[] _loadedByte;

        void LoadGameData(ISavedGameMetadata game)
        {
            MoonBunnyLog.print($"Get GameData with {game.Filename}", "GoogleManager");

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
        }

        void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
        {
            MoonBunnyLog.print($"Read Game Data {status}", "GoogleManager");

            if (status != SavedGameRequestStatus.Success)
            {
                GameManager.instance.SaveLoadSystem.Validation = SaveLoadSystem.DataValidation.Invalidate;
                QuestManager.instance.SaveLoadSystem.Validation = SaveLoadSystem.DataValidation.Invalidate;
                OnDataLoadFail?.Invoke();
                return;
            }
            
            // handle processing the byte array data
            MoonBunnyLog.print($"Read Game Data length : {data.Length}", "GoogleManager");

            try
            {
                if (data.Length <= 8)
                {
                    // Encrypt version of save load system
                    try
                    {
                        _legacySaveLoadSystemChecker = new SaveLoadSystem("Saves", "Save");
                        _legacySaveLoadSystemChecker.ProgressSaveData = _legacySaveLoadSystemChecker.LoadEncrypted<ProgressSaveData>(_legacySaveLoadSystemChecker.PersistenceFilePath);
                        
                        _legacySaveLoadSystemChecker.Init("Saves", "Quest", "txt");
                        _legacySaveLoadSystemChecker.QuestSaveData = _legacySaveLoadSystemChecker.LoadEncrypted<QuestSaveData>(_legacySaveLoadSystemChecker.PersistenceFilePath);
                        
                        _legacySaveLoadSystemChecker.Validation = SaveLoadSystem.DataValidation.Legacy;
                        
                        // Use Legacy SaveData Compatibility
                        MoonBunnyLog.print($"There is no cloud save data but, legacy save data and use it", "GoogleManager");

                        ProgressData = _legacySaveLoadSystemChecker.ProgressSaveData;
                        QuestData = _legacySaveLoadSystemChecker.QuestSaveData;
                    }
                    catch (Exception e)
                    {
                        MoonBunnyLog.print($"Finding legacy save data failed {e}", "GoogleManager");
                        _legacySaveLoadSystemChecker.Validation = SaveLoadSystem.DataValidation.Invalidate;
                        
                        // First time to make save file
                        MoonBunnyLog.print($"First Time to get save file -> make default", "GoogleManager");

                        ProgressData = ProgressSaveData.GetDefaultSaveData();
                        QuestData = QuestSaveData.GetDefaultSaveData();
                    }
                } else
                {
                    _loadedByte = data;

                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = SaveLoadSystem.AesEncryptionKeyByte;
                        aes.IV = SaveLoadSystem.AesEncryptionIVByte;

                        var encryptor = aes.CreateDecryptor();

                        byte[] decryptedData = encryptor.TransformFinalBlock(_loadedByte, 0, _loadedByte.Length);

                        byte[] progressLength = new byte[4];
                        byte[] questLength = new byte[4];

                        Array.Copy(decryptedData, 0, progressLength, 0, 4);
                        Array.Copy(decryptedData, 4, questLength, 0, 4);

                        int progressDataLength = BitConverter.ToInt32(progressLength);
                        int questDataLength = BitConverter.ToInt32(questLength);

                        byte[] progressData = new byte[progressDataLength];
                        byte[] questData = new byte[questDataLength];

                        Array.Copy(decryptedData, 8, progressData, 0, progressDataLength);
                        Array.Copy(decryptedData, 8 + progressDataLength, questData, 0, questDataLength);

                        MoonBunnyLog.print(
                            $"Load data length : {progressData.Length} {questData.Length}, lengthData {progressLength.Length} {questLength.Length}, total {_loadedByte.Length} {decryptedData.Length}",
                            "GoogleManager");

                        string progressDataStr = Encoding.UTF8.GetString(progressData);
                        string questDataStr = Encoding.UTF8.GetString(questData);

                        ProgressData = JsonUtility.FromJson<ProgressSaveData>(progressDataStr);
                        QuestData = JsonUtility.FromJson<QuestSaveData>(questDataStr);
                    }
                }
            }
            catch (Exception e)
            {
                // Save Data is Corrupted
                MoonBunnyLog.print($"Save Data is Corrupted so make new one", "GoogleManager");
                MoonBunnyLog.print(e);

                ProgressData = ProgressSaveData.GetDefaultSaveData();
                QuestData = QuestSaveData.GetDefaultSaveData();
            }
            
            GameManager.instance.SaveLoadSystem.Validation = SaveLoadSystem.DataValidation.Validate;
            QuestManager.instance.SaveLoadSystem.Validation = SaveLoadSystem.DataValidation.Validate;
            OnDataLoadSuccess?.Invoke();
        }

        #endregion

        #region Delete

        void DeleteGameData(string filename)
        {
            MoonBunnyLog.print($"Delete Game Data {filename}", "GoogleManager");

            // Open the file to get the metadata.
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, DeleteSavedGame);
        }

        void DeleteSavedGame(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            MoonBunnyLog.print($"Delete Game Data status {status}", "GoogleManager");

            if (status == SavedGameRequestStatus.Success)
            {
                ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
                savedGameClient.Delete(game);
            } else
            {
                // handle error
            }
        }

        #endregion
    }
}