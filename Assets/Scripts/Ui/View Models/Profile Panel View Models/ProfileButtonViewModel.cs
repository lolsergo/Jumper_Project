using UniRx;
using MVVM;

public class ProfileButtonViewModel
{
    [Data("Label")]
    public readonly ReactiveProperty<string> Label = new();

    [Data("OnClick")]
    public readonly ReactiveCommand OnClick = new();

    // prefab ����� ���������� � ��������� ��� ��������, ����� ����� �� �������
    public ProfileButtonViewModel(string label)
    {
        Label.Value = label;
    }
}
