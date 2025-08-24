public interface IProfilesSceneLogic
{
    string SelectedProfileName { get; }
    void SetSelectedProfile(string profileName);
    bool TryCreateProfile(string profileName, out string error);
    bool TryDeleteSelectedProfile(out string deletedProfileName, out string error);
}
