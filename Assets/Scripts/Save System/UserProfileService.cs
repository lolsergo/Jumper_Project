using System;
using UniRx;
using System.Collections.Generic;
using UnityEngine;

public interface IUserProfileService
{
    IReadOnlyReactiveProperty<SaveData> CurrentSave { get; }
    IReadOnlyReactiveCollection<string> Profiles { get; }

    void CreateProfile(string profileName);
    void LoadProfile(string profileName);
    void DeleteProfile(string profileName);
    void ClearActiveProfile();
    void SaveCurrent();
}

public class UserProfileService : IUserProfileService
{
    private readonly ReactiveProperty<SaveData> _currentSave = new();
    public IReadOnlyReactiveProperty<SaveData> CurrentSave => _currentSave;

    private readonly ReactiveCollection<string> _profiles = new();
    public IReadOnlyReactiveCollection<string> Profiles => _profiles;

    private const string ActiveProfileKey = "ActiveProfile";

    public UserProfileService()
    {
        LoadProfilesList();
        LoadActiveProfileIfExists();
    }

    public void CreateProfile(string profileName)
    {
        if (string.IsNullOrWhiteSpace(profileName))
            throw new ArgumentException("��� ������� �� ����� ���� ������", nameof(profileName));

        var save = new SaveData
        {
            profileName = profileName,
            maxDistanceReached = 0,
            playTime = 0,
            tries = 0
        };

        SaveSystem.Save(save);
        SetActiveProfile(profileName);

        _currentSave.Value = save;
        LoadProfilesList();
    }

    public void LoadProfile(string profileName)
    {
        if (string.IsNullOrWhiteSpace(profileName))
            throw new ArgumentException("��� ������� �� ����� ���� ������", nameof(profileName));

        var save = SaveSystem.Load(profileName);
        SetActiveProfile(profileName);

        _currentSave.Value = save;
    }

    public void DeleteProfile(string profileName)
    {
        if (string.IsNullOrWhiteSpace(profileName))
            throw new ArgumentException("��� ������� �� �������", nameof(profileName));

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
        if (PlayerPrefs.HasKey(ActiveProfileKey))
        {
            var activeName = PlayerPrefs.GetString(ActiveProfileKey);
            if (!string.IsNullOrEmpty(activeName))
            {
                var save = SaveSystem.Load(activeName);
                _currentSave.Value = save;
            }
        }
    }
}