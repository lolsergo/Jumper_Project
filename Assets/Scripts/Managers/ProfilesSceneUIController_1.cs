using System;

public sealed class ProfilesSceneLogic : IProfilesSceneLogic
{
    private readonly IUserProfileService _profileService;
    public string SelectedProfileName { get; private set; }

    public ProfilesSceneLogic(IUserProfileService profileService)
    {
        _profileService = profileService;
    }

    public void SetSelectedProfile(string profileName)
    {
        SelectedProfileName = profileName;
    }

    public bool TryCreateProfile(string profileName, out string error)
    {
        error = null;
        var name = profileName?.Trim();
        if (string.IsNullOrEmpty(name))
        {
            error = "Имя профиля не может быть пустым.";
            return false;
        }

        try
        {
            _profileService.CreateProfile(name);
            return true;
        }
        catch (Exception ex)
        {
            error = $"Ошибка создания профиля: {ex.Message}";
            return false;
        }
    }

    public bool TryDeleteSelectedProfile(out string deletedProfileName, out string error)
    {
        error = null;
        deletedProfileName = null;

        if (string.IsNullOrEmpty(SelectedProfileName))
        {
            error = "Профиль не выбран.";
            return false;
        }

        try
        {
            deletedProfileName = SelectedProfileName;
            _profileService.DeleteProfile(SelectedProfileName);
            SelectedProfileName = null;
            return true;
        }
        catch (Exception ex)
        {
            error = $"Ошибка удаления профиля: {ex.Message}";
            return false;
        }
    }
}