using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using MVVM;

public class ProfileButtonView : MonoBehaviour
{
    [Data("Label")]
    public TMP_Text Label;

    [Data("OnClick")]
    public Button Button;

    [Data("IsDeleteMode")]
    public GameObject DeleteIcon;

    private CompositeDisposable _disposables = new();

    // Метод для ручного биндинга
    public void BindIsDeleteMode(ReactiveProperty<bool> isDeleteMode)
    {
        isDeleteMode
            .Subscribe(active => DeleteIcon.SetActive(active))
            .AddTo(_disposables);
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}