using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class GameState : ISerializable
{
    public static string FileName { get { return Path.Combine(Application.persistentDataPath, "save.dat"); } }
    public static GameState Current { get; set; }

    public int Stage { get; set; }

    GameState() { }

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(Stage);
    }

    public void Deserialize(BinaryReader reader)
    {
        Stage = reader.ReadInt32();
    }

    public void SaveToFile()
    {
        using (var stream = new FileStream(FileName, FileMode.Create, FileAccess.Write))
        using (var writer = new BinaryWriter(stream))
        {
            Serialize(writer);
        }
    }

    public static GameState LoadFromFile()
    {
        using (var stream = new FileStream(FileName, FileMode.Open, FileAccess.Read))
        using (var reader = new BinaryReader(stream))
        {
            var state = new GameState();
            state.Deserialize(reader);
            return state;
        }
    }

    public static GameState LoadNewGame()
    {
        return new GameState();
    }
}