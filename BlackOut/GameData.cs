using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;
using Polenter.Serialization;
using Microsoft.Phone.Shell;

namespace BlackOut
{
    public class GameData
    {
        private const string GAME_DATA_FILENAME = "gameData.bin";
        private const string GAME_DATA_MEMORY_KEY = "GameData";

        public List<int[,]> levels { get; set; }

        private GameState gameState = new GameState();

        public GameState GameState
        {
            get { return gameState; }
            set { gameState = value; }
        }

        public GameData()
        {
            if (levels == null)
                levels = new List<int[,]>();
        }

        private static GameData LoadGameDataFromIsolatedStorage()
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                //isf.DeleteFile(GAME_DATA_FILENAME);
                if (isf.FileExists(GAME_DATA_FILENAME))
                {
                    using (var stream = isf.OpenFile(GAME_DATA_FILENAME, System.IO.FileMode.Open))
                    {
                        SharpSerializer serializer = new SharpSerializer(true);
                        return (GameData)serializer.Deserialize(stream);
                    }
                }

                return new GameData();
            }
        }

        public static GameData LoadGameData()
        {
            return LoadGameDataFromIsolatedStorage();
        }

        public static void SaveGameData(GameData gameData)
        {
            SaveGameDataToIsolatedStorage(gameData);
        }

        private static void SaveGameDataToIsolatedStorage(GameData gameData)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(GAME_DATA_FILENAME))
                    isf.DeleteFile(GAME_DATA_FILENAME);

                using (var stream = isf.OpenFile(GAME_DATA_FILENAME, System.IO.FileMode.CreateNew))
                {
                    SharpSerializer serializer = new SharpSerializer(true);
                    serializer.Serialize(gameData, stream);
                }
            }
        }

        public int[][,] Levels
        {
            get { return levels.ToArray(); }
        }
    }
}
