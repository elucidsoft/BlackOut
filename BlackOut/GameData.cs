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
using Microsoft.Xna.Framework;

namespace BlackOut
{
    public enum LevelLoadedType { None, PreBuilt, Custom };

    public class GameData
    {
        private const string LEVEL_DATA_FILENAME = "levelData.bin";
        private const string GAME_DATA_FILENAME = "gameData.bin";
        private const string GAME_DATA_MEMORY_KEY = "GameData";

        //this wont get serialized in GameData since its not a property
        public List<int[,]> Levels = new List<int[,]>();
        public LevelLoadedType LevelLoadedType = LevelLoadedType.None;

        private GameState gameState = new GameState();
        private GameSettings gameSettings = new GameSettings();

        public GameState GameState
        {
            get { return gameState; }
            set { gameState = value; }
        }

        public GameSettings GameSettings
        {
            get { return gameSettings; }
            set { gameSettings = value; }
        }

        public GameData() { }

        public static GameData LoadGameData()
        {
            GameData gameData = null;
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                //isf.DeleteFile(GAME_DATA_FILENAME);
                if (isf.FileExists(GAME_DATA_FILENAME))
                {
                    using (var stream = isf.OpenFile(GAME_DATA_FILENAME, System.IO.FileMode.Open))
                    {
                        SharpSerializer serializer = new SharpSerializer(true);
                        gameData = (GameData)serializer.Deserialize(stream);
                    }
                }

                gameData = gameData ?? new GameData();
                LoadPreBuiltLevels(gameData);
                return gameData;
            }
        }

        public static void LoadCustomLevels(GameData gameData)
        {
            //don't check if already custom cause their could be changes...just always reload custom
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(GAME_DATA_FILENAME))
                {
                    using (Stream stream = TitleContainer.OpenStream("levels.dat"))
                    {
                        SharpSerializer serializer = new SharpSerializer(true);
                        gameData.Levels = (List<int[,]>)serializer.Deserialize(stream);
                        gameData.LevelLoadedType = LevelLoadedType.Custom;
                    }
                }
            }
            gameData.Levels = gameData.Levels ?? new List<int[,]>();
        }

        public static void LoadPreBuiltLevels(GameData gameData)
        {
            if (gameData.LevelLoadedType != LevelLoadedType.PreBuilt) //prevent reloading pre-built
            {
                using (Stream stream = TitleContainer.OpenStream("levels.dat"))
                {
                    SharpSerializer serializer = new SharpSerializer(true);
                    gameData.Levels = (List<int[,]>)serializer.Deserialize(stream);
                    gameData.LevelLoadedType = LevelLoadedType.PreBuilt;
                }
            }
        }

#if DEBUG

        public static void PushLevelOut()
        {
            byte[] bytes = null;
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var stream = isf.OpenFile(LEVEL_DATA_FILENAME, System.IO.FileMode.Open))
                {
                    bytes = new byte[stream.Length];
                    while (stream.Position < stream.Length)
                    {
                        bytes[stream.Position] = (byte)stream.ReadByte();
                    }
                }
            }

            WP7FileCopyClient.Uploader.Upload(bytes);
        }
#endif

        public static void SaveGameData(GameData gameData)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(GAME_DATA_FILENAME))
                    isf.DeleteFile(GAME_DATA_FILENAME);

             
                    using (var stream = isf.OpenFile(GAME_DATA_FILENAME, System.IO.FileMode.Create))
                    {
                        SharpSerializer serializer = new SharpSerializer(true);
                        serializer.Serialize(gameData, stream);
                    }
                

                using (var stream = isf.OpenFile(LEVEL_DATA_FILENAME, System.IO.FileMode.Create))
                {
                    SharpSerializer serializer = new SharpSerializer(true);
                    serializer.Serialize(gameData.Levels, stream);
                }
            }


        }
    }
}
