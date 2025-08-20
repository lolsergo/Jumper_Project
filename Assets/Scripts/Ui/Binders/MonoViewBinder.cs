using MVVM;
using UnityEngine;
using Zenject;
using System;

using Object = UnityEngine.Object;

public sealed class MonoViewBinder : MonoBehaviour
{
    public enum BindingMode { FromInstance, FromResolve, FromResolveId }

    [Header("View Settings")]
    [SerializeField] private BindingMode viewBinding;
    [SerializeField] private Object view;
    [SerializeField, HideInInspector] private string viewTypeName;
    [SerializeField] private string viewId;

    [Header("ViewModel Settings")]
    [SerializeField] private BindingMode viewModelBinding;
    [SerializeField] private Object viewModel;
    [SerializeField, HideInInspector] private string viewModelTypeName;
    [SerializeField] private string viewModelId;

    [Inject] private DiContainer _diContainer;
    private IBinder _binder;

    private void Awake()
    {
        _binder = CreateBinder();
        _binder.Bind();
    }

    private void OnEnable() => _binder?.Bind();
    private void OnDisable() => _binder?.Unbind();

    private IBinder CreateBinder()
    {
        object viewObj = ResolveTarget(viewBinding, view, viewTypeName, viewId);
        object viewModelObj = ResolveTarget(viewModelBinding, viewModel, viewModelTypeName, viewModelId);

        return BinderFactory.CreateComposite(viewObj, viewModelObj);
    }

    private object ResolveTarget(BindingMode mode, Object instance, string typeName, string id)
    {
        switch (mode)
        {
            case BindingMode.FromInstance:
                return instance;

            case BindingMode.FromResolve:
                return _diContainer.Resolve(GetTypeFromName(typeName));

            case BindingMode.FromResolveId:
                return _diContainer.ResolveId(GetTypeFromName(typeName), id);

            default:
                throw new Exception($"Unsupported binding mode: {mode}");
        }
    }

    private static Type GetTypeFromName(string typeName)
    {
        if (string.IsNullOrEmpty(typeName))
            throw new Exception("Type name is not set");
        return Type.GetType(typeName);
    }

#if UNITY_EDITOR
    // Эти поля остаются только для инспектора
    [SerializeField] private UnityEditor.MonoScript viewType;
    [SerializeField] private UnityEditor.MonoScript viewModelType;

    private void OnValidate()
    {
        if (viewType != null)
            viewTypeName = viewType.GetClass()?.AssemblyQualifiedName ?? string.Empty;
        else
            viewTypeName = string.Empty;

        if (viewModelType != null)
            viewModelTypeName = viewModelType.GetClass()?.AssemblyQualifiedName ?? string.Empty;
        else
            viewModelTypeName = string.Empty;
    }
#endif
}