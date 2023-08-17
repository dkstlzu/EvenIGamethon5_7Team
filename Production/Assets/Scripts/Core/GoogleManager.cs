using System;
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

        private PlayGamesClientConfiguration _clientConfiguration;

        public ProgressSaveData ProgressData
        {
            get => GameManager.ProgressSaveData;
            set => GameManager.ProgressSaveData = value;
        }

        public QuestSaveData QuestData
        {
            get => QuestManager.SaveData;
            set => QuestManager.SaveData = value;
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
                byte[] questLength = BitConverter.GetBytes(progressTemp.Length);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(progressLength);
                    Array.Reverse(questLength);
                }

                Array.Copy(progressLength, 0, totalData, 0, sizeof(Int32));
                Array.Copy(questLength, 0, totalData, 4, sizeof(Int32));
                Array.Copy(progressTemp, 0, totalData, 8, progressTemp.Length);
                Array.Copy(questTemp, 0, totalData, progressTemp.Length, questTemp.Length);

                _loadedByte = aes.CreateEncryptor().TransformFinalBlock(totalData, 0, totalData.Length);
            }

            SelectUIMethod = SelectUIMethod.Save;
            ShowSelectUI();
        }

        public void Load()
        {
            SelectUIMethod = SelectUIMethod.Load;
            ShowSelectUI();
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
#endif
        }

        public void Logout()
        {
#if UNITY_EDITOR
#else
            SelectUIMethod = SelectUIMethod.Save;
            ShowSelectUI();
            PlayGamesPlatform.Instance.SignOut();
#endif
        }

        private void PlayGamesAuthentication(SignInStatus status)
        {
            MoonBunnyLog.print($"PlayGamesPlatform Authentication {status}");

            switch (status)
            {
                case SignInStatus.Success:
                    ShowSelectUI();
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

        void ShowSelectUI()
        {
            uint maxNumToDisplay = 5;
            bool allowCreateNew = true;
            bool allowDelete = true;

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            MoonBunnyLog.print($"ShowSelectUI savedGameClient type {savedGameClient.GetType()}", "GoogleManager");
            savedGameClient.ShowSelectSavedGameUI("Select saved game", maxNumToDisplay, allowCreateNew, allowDelete, OnSavedGameSelected);
        }

        void OnSavedGameSelected(SelectUIStatus status, ISavedGameMetadata game)
        {
            MoonBunnyLog.print($"SelectUI Status : {status}, metadata type {game.GetType()}", "GoogleManager");

            if (status == SelectUIStatus.SavedGameSelected)
            {
                // handle selected game save
                switch (SelectUIMethod)
                {
                    case SelectUIMethod.Load:
                        OpenSavedGame(game.Filename);
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

        #region Load

        void OpenSavedGame(string filename)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            MoonBunnyLog.print($"OpenSavedGame with file {filename}", "GoogleManager");

            savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
        }

        void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            MoonBunnyLog.print($"OnSavedGameOpend with status {status}", "GoogleManager");

            if (status == SavedGameRequestStatus.Success)
            {
                // handle reading or writing of saved game.
                LoadGameData(game);
            } else
            {
                // handle error
            }
        }

        #endregion

        #region Save

        private Texture2D savedImage;

        void SaveGame(ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime)
        {
            MoonBunnyLog.print($"SaveGame at {game.Filename} with playtime {totalPlaytime}", "GoogleManager");

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
            builder = builder.WithUpdatedPlayedTime(totalPlaytime).WithUpdatedDescription("Saved game at " + DateTime.Now);

            savedImage = getScreenshot();
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

        public Texture2D getScreenshot()
        {
            // Create a 2D texture that is 1024x700 pixels from which the PNG will be
            // extracted
            Texture2D screenShot = new Texture2D(1024, 700);

            // Takes the screenshot from top left hand corner of screen and maps to top
            // left hand corner of screenShot texture
            screenShot.ReadPixels(new Rect(0, 0, Screen.width, (Screen.width / 1024f) * 700), 0, 0);
            return screenShot;
        }

        #endregion

        #region Read

        public event Action OnDataLoadSuccess;
        public event Action OnDataLoadFail;
        public bool DataIsLoaded;
        private byte[] _loadedByte;

        void LoadGameData(ISavedGameMetadata game)
        {
            MoonBunnyLog.print($"LoadGameData with {game.Filename}", "GoogleManager");

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
        }

        void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
        {
            MoonBunnyLog.print($"Read Game Data {status}", "GoogleManager");

            if (status == SavedGameRequestStatus.Success)
            {
                // handle processing the byte array data
                MoonBunnyLog.print($"Read Game Data length : {data.Length}", "GoogleManager");
                _loadedByte = data;

                int progressDataLength = BitConverter.ToInt32(_loadedByte, 0);
                int questDataLength = BitConverter.ToInt32(_loadedByte, 4);

                byte[] progressData = new byte[progressDataLength];
                byte[] questData = new byte[questDataLength];

                Array.Copy(_loadedByte, 8, progressData, 0, progressDataLength);
                Array.Copy(_loadedByte, 8 + progressDataLength, questData, 0, questDataLength);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = SaveLoadSystem.AesEncryptionKeyByte;
                    aes.IV = SaveLoadSystem.AesEncryptionIVByte;

                    var encryptor = aes.CreateDecryptor();

                    byte[] decryptedProgressData = encryptor.TransformFinalBlock(progressData, 0, progressDataLength);
                    byte[] decryptedQuestData = encryptor.TransformFinalBlock(questData, 0, questDataLength);

                    string progressDataStr = Encoding.UTF8.GetString(decryptedProgressData);
                    string questDataStr = Encoding.UTF8.GetString(decryptedQuestData);

                    ProgressData = JsonUtility.FromJson<ProgressSaveData>(progressDataStr);
                    QuestData = JsonUtility.FromJson<QuestSaveData>(questDataStr);
                }

                DataIsLoaded = true;
                OnDataLoadSuccess?.Invoke();
            } else
            {
                DataIsLoaded = false;
                OnDataLoadFail?.Invoke();
            }
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