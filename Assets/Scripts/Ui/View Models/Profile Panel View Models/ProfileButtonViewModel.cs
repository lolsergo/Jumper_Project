using UniRx;
using MVVM;

public class ProfileButtonViewModel
{
    [Data("Label")]
    public readonly ReactiveProperty<string> Label = new();

    [Data("OnClick")]
    public readonly ReactiveCommand OnClick = new();

    [Data("IsDeleteMode")]
    public readonly ReactiveProperty<bool> IsDeleteMode = new(false);

    public ProfileButtonViewModel(string label)
    {
        Label.Value = label;
    }
}
