using System;
using UniRx;
using UnityEngine;

public class UserProfileService : IUserProfileService
{
    private const string ActiveProfileKey = "ActiveProfile";

    private readonly ReactiveProperty<SaveData> _currentSave = new();
    public IReadOnlyReactiveProperty<SaveData> CurrentSave => _currentSave;

    private readonly ReactiveCollection<string> _profiles = new();
    public IReadOnlyReactiveCollection<string> Profiles => _profiles;

    public UserProfileService()
    {
        LoadProfilesList();
        LoadActiveProfileIfExists();
    }

    public void CreateProfile(string profileName)
    {
        ValidateProfileName(profileName);

        var save = new SaveData { profileName = profileName };
        SaveSystem.Save(save);
        SetActiveProfile(profileName);

        _currentSave.Value = save;
        LoadProfilesList();
    }

    public void LoadProfile(string profileName)
    {
        ValidateProfileName(profileName);
        var save = SaveSystem.Load(profileName);
        SetActiveProfile(profileName);
        _currentSave.Value = save;
    }

    public void DeleteProfile(string profileName)
    {
        ValidateProfileName(profileName);
        SaveSystem.DeleteProfile(profileName);
        LoadProfilesList();

        if (_currentSave.Value != null && _currentSave.Value.profileName == profileName)
            ClearActiveProfile();
    }

    public void ClearActiveProfile()
    {
        _currentSave.Value = null;
        PlayerPrefs.DeleteKey(ActiveProfileKey);
        PlayerPrefs.Save();
    }

    public void SaveCurrent()
    {
        if (_currentSave.Value != null)
            SaveSystem.Save(_currentSave.Value);
    }

    public void IncrementTries()
    {
        if (_currentSave.Value != null)
        {
            _currentSave.Value.tries++;
            SaveCurrent();
        }
    }

    public void AddPlayTime(float seconds)
    {
        if (_currentSave.Value != null)
        {
            _currentSave.Value.totalPlayTime += seconds;
            SaveCurrent();
        }
    }

    // --- Helpers ---
    private void SetActiveProfile(string profileName)
    {
        PlayerPrefs.SetString(ActiveProfileKey, profileName);
        PlayerPrefs.Save();
    }

    private void LoadProfilesList()
    {
        _profiles.Clear();
        foreach (var p in SaveSystem.GetAllProfiles())
            _profiles.Add(p);
    }

    private void LoadActiveProfileIfExists()
    {
        if (!PlayerPrefs.HasKey(ActiveProfileKey)) return;
        var activeName = PlayerPrefs.GetString(ActiveProfileKey);
        if (!string.IsNullOrEmpty(activeName))
            _currentSave.Value = SaveSystem.Load(activeName);
    }

    private void ValidateProfileName(string profileName)
    {
        if (string.IsNullOrWhiteSpace(profileName))
            throw new ArgumentException("Имя профиля не может быть пустым");
    }
}