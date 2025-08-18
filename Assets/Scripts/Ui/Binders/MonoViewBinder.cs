using MVVM;
using UnityEditor;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

/// <summary> ��������� View � ViewModel ����� DI-��������� </summary>
public sealed class MonoViewBinder : MonoBehaviour
{
    /// <summary> ����� �������� ��� View </summary>
    public enum BindingMode { FromInstance, FromResolve, FromResolveId }

    [Header("View Settings")]
    [Tooltip("������ ��������� View")]
    [SerializeField]
    private BindingMode viewBinding;

    [Tooltip("���������� ���� ������, ���� ������ FromInstance")]
    [SerializeField]
    private Object view;

    [Tooltip("������� ��� �������, ���� ������ FromResolve/FromResolveId")]
    [SerializeField]
    private MonoScript viewType;

    [Tooltip("������������� ��� DI, ���� ������ FromResolveId")]
    [SerializeField]
    private string viewId;

    [Header("ViewModel Settings")]
    [Tooltip("������ ��������� ViewModel")]
    [SerializeField]
    private BindingMode viewModelBinding;

    [Tooltip("���������� ���� ������, ���� ������ FromInstance")]
    [SerializeField]
    private Object viewModel;

    [Tooltip("������� ��� �������, ���� ������ FromResolve/FromResolveId")]
    [SerializeField]
    private MonoScript viewModelType;

    [Tooltip("������������� ��� DI, ���� ������ FromResolveId")]
    [SerializeField]
    private string viewModelId;

    [Inject]
    private DiContainer _diContainer;
    private IBinder _binder;

    /// <summary> ������������� ������� ��� ������ </summary>
    private void Awake()
    {
        _binder = CreateBinder();
        _binder.Bind();
    }

    /// <summary> ��������� �������� ��� ��������� ������� </summary>
    private void OnEnable()
    {
        _binder?.Bind();
    }

    /// <summary> ���������� �������� ��� ���������� ������� </summary>
    private void OnDisable()
    {
        _binder?.Unbind();
    }

    /// <summary> ������� ������ �� ������ ��������� �������� </summary>
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

    /// <summary> ������ ����������� � ����������� �� ���������� ������ </summary>
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
    /// <summary> �������������� ���������� ���������� ��� ��������� ����� </summary>
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
