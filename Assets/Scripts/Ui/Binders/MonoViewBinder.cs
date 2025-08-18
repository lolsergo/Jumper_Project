using MVVM;
using UnityEditor;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

/// <summary> Связывает View и ViewModel через DI-контейнер </summary>
public sealed class MonoViewBinder : MonoBehaviour
{
    /// <summary> Режим привязки для View </summary>
    public enum BindingMode { FromInstance, FromResolve, FromResolveId }

    [Header("View Settings")]
    [Tooltip("Способ получения View")]
    [SerializeField]
    private BindingMode viewBinding;

    [Tooltip("Перетащите сюда объект, если выбран FromInstance")]
    [SerializeField]
    private Object view;

    [Tooltip("Укажите тип скрипта, если выбран FromResolve/FromResolveId")]
    [SerializeField]
    private MonoScript viewType;

    [Tooltip("Идентификатор для DI, если выбран FromResolveId")]
    [SerializeField]
    private string viewId;

    [Header("ViewModel Settings")]
    [Tooltip("Способ получения ViewModel")]
    [SerializeField]
    private BindingMode viewModelBinding;

    [Tooltip("Перетащите сюда объект, если выбран FromInstance")]
    [SerializeField]
    private Object viewModel;

    [Tooltip("Укажите тип скрипта, если выбран FromResolve/FromResolveId")]
    [SerializeField]
    private MonoScript viewModelType;

    [Tooltip("Идентификатор для DI, если выбран FromResolveId")]
    [SerializeField]
    private string viewModelId;

    [Inject]
    private DiContainer _diContainer;
    private IBinder _binder;

    /// <summary> Инициализация биндера при старте </summary>
    private void Awake()
    {
        _binder = CreateBinder();
        _binder.Bind();
    }

    /// <summary> Активация привязки при включении объекта </summary>
    private void OnEnable()
    {
        _binder?.Bind();
    }

    /// <summary> Отключение привязки при выключении объекта </summary>
    private void OnDisable()
    {
        _binder?.Unbind();
    }

    /// <summary> Создает биндер на основе выбранных настроек </summary>
    private IBinder CreateBinder()
    {

        object viewObj = ResolveTarget(
            viewBinding,
            view,
            viewType,
            viewId
        );

        object viewModelObj = ResolveTarget(
            viewModelBinding,
            viewModel,
            viewModelType,
            viewModelId
        );

        return BinderFactory.CreateComposite(viewObj, viewModelObj);

    }

    /// <summary> Решает зависимость в зависимости от выбранного режима </summary>
    private object ResolveTarget(
        BindingMode mode,
        Object instance,
        MonoScript typeScript,
        string id
    )
    {
        switch (mode)
        {
            case BindingMode.FromInstance:
                return instance;

            case BindingMode.FromResolve:
                return _diContainer.Resolve(typeScript.GetClass());

            case BindingMode.FromResolveId:
                return _diContainer.ResolveId(
                    typeScript.GetClass(),
                    id
                );

            default:
                throw new System.Exception($"Unsupported binding mode: {mode}");
        }
    }

#if UNITY_EDITOR
    /// <summary> Автоматическое обновление инспектора при изменении полей </summary>
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (this != null)
                    UnityEditor.EditorUtility.SetDirty(this);
            };
        }
    }
#endif
}
