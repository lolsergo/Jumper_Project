using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace MVVM
{
    public static class BinderFactory
    {
        private static readonly List<Type> _concreteBinders = new();

        // Новый контейнер (явно задаётся из инсталлеров)
        public static DiContainer DiContainer { get; private set; }
        public static void SetDiContainer(DiContainer container) => DiContainer = container;

        public static void RegisterBinder(Type type) => _concreteBinders.Add(type);
        public static void RegisterBinder<T>() where T : IBinder => _concreteBinders.Add(typeof(T));

        public static BinderComposite CreateComposite(object view, object model)
        {
            List<IBinder> children = new();

            if (TryCreateConcrete(view, model, out IBinder binder))
                children.Add(binder);

            IReadOnlyDictionary<object, MemberInfo> viewMembers = Scanner.ScanMembers(view);
            IReadOnlyDictionary<object, MemberInfo> modelMembers = Scanner.ScanMembers(model);

            foreach ((object id, MemberInfo viewMember) in viewMembers)
            {
                if (modelMembers.TryGetValue(id, out MemberInfo modelMember))
                {
                    if (TryResolve(viewMember, modelMember, view, model, out IBinder childBinder))
                        children.Add(childBinder);
                }
            }

            return new BinderComposite(children);
        }

        private static bool TryResolve(
            MemberInfo viewMember,
            MemberInfo modelMember,
            object view,
            object model,
            out IBinder binder
        )
        {
            bool matchesChildren = Resolver.TryResolve(
                viewMember,
                modelMember,
                view, model,
                out var childView,
                out var childModel
            );

            if (!matchesChildren)
            {
                binder = default;
                return false;
            }

            if (TryCreateConcrete(childView, childModel, out binder))
                return true;

            Debug.LogWarning($"Can't create binder for View: {childView.GetType().Name} and ViewModel: {childModel.GetType().Name}");
            return false;
        }

        public static bool TryCreateConcrete(object view, object model, out IBinder binder)
        {
            foreach (Type binderType in _concreteBinders)
            {
                var ctors = binderType.GetConstructors(
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    BindingFlags.DeclaredOnly
                );

                foreach (var constructor in ctors)
                {
                    ParameterInfo[] parameters = constructor.GetParameters();
                    if (parameters.Length < 2)
                        continue;

                    bool directOrder =
                        parameters[0].ParameterType.IsInstanceOfType(view) &&
                        parameters[1].ParameterType.IsInstanceOfType(model);

                    bool reverseOrder =
                        parameters[0].ParameterType.IsInstanceOfType(model) &&
                        parameters[1].ParameterType.IsInstanceOfType(view);

                    if (!directOrder && !reverseOrder)
                        continue;

                    object[] args = new object[parameters.Length];
                    if (directOrder)
                    {
                        args[0] = view;
                        args[1] = model;
                    }
                    else
                    {
                        args[0] = model;
                        args[1] = view;
                    }

                    if (parameters.Length > 2)
                    {
                        if (DiContainer == null)
                        {
                            Debug.LogError($"[BinderFactory] DiContainer not set. Call BinderFactory.SetDiContainer() before bindings that need DI (binder: {binderType.Name}).");
                            continue;
                        }

                        bool failed = false;
                        for (int i = 2; i < parameters.Length; i++)
                        {
                            try
                            {
                                args[i] = DiContainer.Resolve(parameters[i].ParameterType);
                            }
                            catch (Exception e)
                            {
                                Debug.LogError($"[BinderFactory] Failed to resolve param #{i} ({parameters[i].ParameterType.Name}) for {binderType.Name}: {e}");
                                failed = true;
                                break;
                            }
                        }
                        if (failed)
                            continue;
                    }

                    binder = Activator.CreateInstance(binderType, args) as IBinder;
                    if (binder != null)
                        return true;
                }
            }

            binder = null;
            return false;
        }
    }
}