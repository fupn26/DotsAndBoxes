using DotsAndBoxes.Structures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;

namespace DotsAndBoxes.Classes
{
    public static class DataProvider
    {
        private readonly static string fileName = "saved_game.json";
        private static List<GameState> _gameStates;

        public static ReadOnlyCollection<GameState> GameStates { 
            get
            {
                return _gameStates.AsReadOnly();
            } 
        }

        static DataProvider()
        {
            ReadJson();
        }
        private static void ReadJson()
        {
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName);
                _gameStates = JsonSerializer.Deserialize<List<GameState>>(jsonString);
            }
            else
            {
                _gameStates = new List<GameState>();
            }
        }

        public static void AddElement(GameState gameState)
        {
            _gameStates.Add(gameState);
        }

        public static void RemoveLastElement()
        {
            _gameStates.RemoveAt(_gameStates.Count - 1);
        }

        public static void CommitChanges()
        {
            WriteJson();
        }
        private static void WriteJson()
        {
            string jsonString;
            string v = JsonSerializer.Serialize<List<GameState>>(_gameStates);
            jsonString = v;
            File.WriteAllText(fileName, jsonString);
        }
    }
}
