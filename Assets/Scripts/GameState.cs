using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class GameState : ISerializable
{
    public static string FileName { get { return Path.Combine(Application.persistentDataPath, "save.dat"); } }
    public static GameState Current { get; set; }

    private int _stage, _chckpointIdx;

    public int Stage { get { return _stage; } set { Dirty = _stage != value; _stage = value; } }
    public int CheckpointIndex { get { return _chckpointIdx; } set { Dirty = _chckpointIdx != value; _chckpointIdx = value; } }

    public bool YFlipUnlocked { get { return _stage > 0; } }

    public bool Dirty { get; private set; }

    GameState() { }

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(Stage);
        writer.Write(CheckpointIndex);
    }

    public void Deserialize(BinaryReader reader)
    {
        Stage = reader.ReadInt32();
        CheckpointIndex = reader.ReadInt32();
    }

    public void SaveToFile()
    {
        Dirty = false;

        Debug.Log("Saving game state");

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