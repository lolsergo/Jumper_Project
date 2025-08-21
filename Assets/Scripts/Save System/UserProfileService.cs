using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UserProfileService : IUserProfileService
{
    // === Constants ===
    private const string ActiveProfileKey = "ActiveProfile";

    // === Reactive Data ===
    private readonly ReactiveProperty<SaveData> _currentSave = new();
    public IReadOnlyReactiveProperty<SaveData> CurrentSave => _currentSave;

    private readonly ReactiveCollection<string> _profiles = new();
    public IReadOnlyReactiveCollection<string> Profiles => _profiles;

    // === Constructor ===
    public UserProfileService()
    {
        LoadProfilesList();
        LoadActiveProfileIfExists();
    }

    // === Public API ===
    public void CreateProfile(string profileName)
    {
        ValidateProfileName(profileName, nameof(profileName));

        var save = new SaveData
        {
            profileName = profileName,
            maxDistanceReached = 0,
            totalPlayTime = 0,
            tries = 0
        };

        SaveSystem.Save(save);
        SetActiveProfile(profileName);

        _currentSave.Value = save;
        LoadProfilesList();
    }

    public void LoadProfile(string profileName)
    {
        ValidateProfileName(profileName, nameof(profileName));

        var save = SaveSystem.Load(profileName);
        SetActiveProfile(profileName);

        _currentSave.Value = save;
    }

    public void DeleteProfile(string profileName)
    {
        ValidateProfileName(profileName, nameof(profileName));

        SaveSystem.DeleteProfile(profileName);
        LoadProfilesList();

        if (_currentSave.Value != null &&
            _currentSave.Value.profileName == profileName)
        {
            ClearActiveProfile();
        }
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

    // === Private Helpers ===
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
        if (!PlayerPrefs.HasKey(ActiveProfileKey))
            return;

        var activeName = PlayerPrefs.GetString(ActiveProfileKey);
        if (!string.IsNullOrEmpty(activeName))
        {
            var save = SaveSystem.Load(activeName);
            _currentSave.Value = save;
        }
    }

    private static void ValidateProfileName(string profileName, string paramName)
    {
        if (string.IsNullOrWhiteSpace(profileName))
            throw new ArgumentException("Имя профиля не может быть пустым", paramName);
    }
}