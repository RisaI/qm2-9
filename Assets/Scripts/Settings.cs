using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Settings : ISerializable
{
    public const string FileName = "settings.dat";
    
    public static Settings Current { get; private set; }

    static Settings()
    {
        Current = LoadFromFile();
    }

    public bool Finished { get; set; }

    Settings() { }

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(Finished);
    }

    public void Deserialize(BinaryReader reader)
    {
        Finished = reader.ReadBoolean();
    }

    public void SaveToFile()
    {
        SaveToFile(Path.Combine(Application.persistentDataPath, FileName));
    }

    public void SaveToFile(string path)
    {
        using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
        using (var writer = new BinaryWriter(stream))
        {
            Serialize(writer);
        }
    }

    static Settings LoadFromFile()
    {
        var path = Path.Combine(Application.persistentDataPath, FileName);

        if (File.Exists(path))
            return LoadFromFile(path);
        else
        {
            var defaults = LoadDefaults();
            defaults.SaveToFile(path);
            return defaults;
        }
    }

    static Settings LoadFromFile(string path)
    {
        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        using (var reader = new BinaryReader(stream))
        {
            var set = new Settings();
            set.Deserialize(reader);
            return set;
        }
    }

    static Settings LoadDefaults()
    {
        return new Settings();
    }
}