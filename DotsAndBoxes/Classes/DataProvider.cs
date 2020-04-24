using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace DotsAndBoxes.Classes
{
    public static class DataProvider
    {
        private const string FileName = "saved_game.json";
        private static List<GameState> _gameStates;

        public static ReadOnlyCollection<GameState> GameStates => _gameStates.AsReadOnly();

        static DataProvider()
        {
            ReadJson();
        }

        public static void AddElement(GameState gameState)
        {
            _gameStates.Add(gameState);
        }

        public static void CommitChanges()
        {
            WriteJson();
        }

        public static void RemoveLastElement()
        {
            _gameStates.RemoveAt(_gameStates.Count - 1);
        }

        private static void ReadJson()
        {
            if (File.Exists(FileName))
            {
                var jsonString = File.ReadAllText(FileName);
                _gameStates = JsonSerializer.Deserialize<List<GameState>>(jsonString);
            }
            else
            {
                _gameStates = new List<GameState>();
            }
        }

        private static void WriteJson()
        {
            var v = JsonSerializer.Serialize(_gameStates);
            var jsonString = v;
            File.WriteAllText(FileName, jsonString);
        }
    }
}