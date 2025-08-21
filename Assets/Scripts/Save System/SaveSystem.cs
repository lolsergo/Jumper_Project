using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string FolderPath => Path.Combine(Application.persistentDataPath, "Saves");

    public static void Save(SaveData data)
    {
        EnsureFolderExists();

        string json = JsonUtility.ToJson(data, true);
        string filePath = GetProfilePath(data.profileName);

        File.WriteAllText(filePath, json);
        Debug.Log($"[SaveSystem] Profile '{data.profileName}' saved successfully.");
    }

    public static SaveData Load(string profileName)
    {
        string filePath = GetProfilePath(profileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log($"[SaveSystem] Profile '{profileName}' loaded successfully.");
            return JsonUtility.FromJson<SaveData>(json);
        }

        Debug.LogWarning($"[SaveSystem] Profile '{profileName}' not found.");
        return null;
    }

    public static void DeleteProfile(string profileName)
    {
        string filePath = GetProfilePath(profileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"[SaveSystem] Profile '{profileName}' deleted successfully.");
        }
        else
        {
            Debug.LogWarning($"[SaveSystem] Profile '{profileName}' not found for deletion.");
        }
    }

    public static string[] GetAllProfiles()
    {
        if (!Directory.Exists(FolderPath))
            return new string[0];

        string[] files = Directory.GetFiles(FolderPath, "*.json");

        for (int i = 0; i < files.Length; i++)
            files[i] = Path.GetFileNameWithoutExtension(files[i]);

        return files;
    }

    // === Helpers ===
    private static string GetProfilePath(string profileName) =>
        Path.Combine(FolderPath, profileName + ".json");

    private static void EnsureFolderExists()
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
            Debug.Log("[SaveSystem] Save folder created.");
        }
    }
}
