//using MVVM;
//using UniRx;
//using UnityEngine;

//public sealed class GameObjectToReactivePropertyBinder : IBinder
//{
//    private readonly GameObject _gameObject;
//    private readonly ReactiveProperty<bool> _reactiveProperty;
//    private readonly CompositeDisposable _disposables = new();

//    public GameObjectToReactivePropertyBinder(GameObject gameObject, ReactiveProperty<bool> reactiveProperty)
//    {
//        Debug.Log($"[GameObjectToReactivePropertyBinder] Created: {gameObject.name} <-> ReactiveProperty<bool>");
//        _gameObject = gameObject;
//        _reactiveProperty = reactiveProperty;
//    }

//    public void Bind()
//    {
//        Debug.Log($"[GameObjectToReactivePropertyBinder] Binding {_gameObject.name}");

//        _reactiveProperty
//            .StartWith(_reactiveProperty.Value)
//            .Subscribe(active =>
//            {
//                Debug.Log($"[GameObjectToReactivePropertyBinder] {_gameObject.name}.SetActive({active})");
//                _gameObject.SetActive(active);
//            })
//            .AddTo(_disposables);
//    }

//    public void Unbind()
//    {
//        Debug.Log($"[GameObjectToReactivePropertyBinder] Unbinding {_gameObject.name}");
//        _disposables.Clear();
//    }
//}