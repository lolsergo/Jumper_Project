using UniRx;
using MVVM;

public class ProfileButtonViewModel
{
    [Data("Label")]
    public readonly ReactiveProperty<string> Label = new();

    [Data("OnClick")]
    public readonly ReactiveCommand OnClick = new();

    // prefab можно пробросить в контейнер при создании, чтобы здесь не хранить
    public ProfileButtonViewModel(string label)
    {
        Label.Value = label;
    }
}
