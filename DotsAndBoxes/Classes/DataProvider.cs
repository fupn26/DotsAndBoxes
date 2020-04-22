using DotsAndBoxes.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace DotsAndBoxes.Classes
{
    public static class DataProvider
    {
        private readonly static string fileName = "saved_game.json";
        public static List<GameState> ReadJson()
        {
            if (File.Exists(fileName))
            {
                List<GameState> prevGameStates;
                string jsonString = File.ReadAllText(fileName);
                prevGameStates = JsonSerializer.Deserialize<List<GameState>>(jsonString);

                return prevGameStates;
            }
            else
            {
                return null;
            }
        }

        public static void WriteJson(List<GameState> prevGameStates)
        {
            string jsonString;
            string v = JsonSerializer.Serialize<List<GameState>>(prevGameStates);
            jsonString = v;
            File.WriteAllText(fileName, jsonString);
        }
    }
}
