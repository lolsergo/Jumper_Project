using Zenject;
using MVVM;
using System;

public class CreateProfileAcceptButtonViewModel
{
    [Data("CreateProfileAcceptButton")]
    public readonly Action CreateProfileAccept;

    private readonly ProfilesSceneUIController _profileSceneController;
    private readonly NewProfileNameInputViewModel _nameVM;

    [Inject]
    public CreateProfileAcceptButtonViewModel(
        ProfilesSceneUIController profileSceneController,
        NewProfileNameInputViewModel nameVM)
    {
        _profileSceneController = profileSceneController;
        _nameVM = nameVM;

        CreateProfileAccept = () => {
            var name = _nameVM.NewProfileName.Value;
            _profileSceneController.ConfirmCreateProfile(name);
        };
    }
}
