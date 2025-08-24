using UnityEngine;
using UniRx;
using MVVM;

public class NewProfileNameInputViewModel
{
    [Data("NewProfileNameInput")]
    public readonly ReactiveProperty<string> NewProfileName = new("");

    public void Initialize() => Debug.Log("[VM] Name input VM initialized");
}
