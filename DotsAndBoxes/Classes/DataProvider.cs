using DotsAndBoxes.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace DotsAndBoxes.Classes
{
    static class DataProvider
    {
        private readonly static string fileName = "saved_game.json";
        public static List<GameStateSerializable> ReadJson()
        {
            if (File.Exists(fileName))
            {
                List<GameStateSerializable> prevGameStates;
                string jsonString = File.ReadAllText(fileName);
                prevGameStates = JsonSerializer.Deserialize<List<GameStateSerializable>>(jsonString);

                return prevGameStates;
            }
            else
            {
                return null;
            }
        }

        public static void WriteJson(List<GameStateSerializable> prevGameStates)
        {
            string jsonString;
            string v = JsonSerializer.Serialize<List<GameStateSerializable>>(prevGameStates);
            jsonString = v;
            File.WriteAllText(fileName, jsonString);
        }
    }
}
