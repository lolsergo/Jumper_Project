using UniRx;

public interface IUserProfileService
{
    IReadOnlyReactiveProperty<SaveData> CurrentSave { get; }
    IReadOnlyReactiveCollection<string> Profiles { get; }

    void CreateProfile(string profileName);
    void LoadProfile(string profileName);
    void DeleteProfile(string profileName);
    void ClearActiveProfile();
    void SaveCurrent();
    void IncrementTries();
    void AddPlayTime(float seconds);
}