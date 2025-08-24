using Zenject;

public static class InstallerExtensions
{
    public static void BindViewModel<T>(this DiContainer container) where T : class
    {
        container.BindInterfacesAndSelfTo<T>().AsSingle().NonLazy();
    }

    public static void AssertNotNull(this DiContainer container, object obj, string fieldName, string context)
    {
#if UNITY_EDITOR
        if (obj == null)
            throw new System.InvalidOperationException($"[{context}] —сылка '{fieldName}' не установлена в инспекторе.");
#endif
    }
}