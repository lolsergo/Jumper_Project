using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RebindActionsPanelSpawner : MonoBehaviour
{
    [SerializeField] private RebindActionButtonView _prefab;
    [SerializeField] private Transform _container;
    [SerializeField] private int _bindingIndex = 0;
    [SerializeField] private bool _onlyExistingInController = true;

    private readonly List<RebindActionButtonViewModel> _createdVMs = new();
    private RebindActionButtonViewModel.Factory _vmFactory;
    private InputController _inputController;
    private DiContainer _di;
    private bool _injected;

    [Inject]
    public void Construct(
        RebindActionButtonViewModel.Factory vmFactory,
        InputController inputController,
        DiContainer di)
    {
        _vmFactory = vmFactory;
        _inputController = inputController;
        _di = di;
        _injected = true;
    }

    private void Awake()
    {
        // Fallback when no SceneContext present (e.g. additive menu scene).
        if (!_injected)
        {
            var project = ProjectContext.Instance.Container;
            project.Inject(this);
            if (_vmFactory == null)
                Debug.LogError("[RebindActionsPanelSpawner] Fallback injection failed (vmFactory null)");
        }
    }

    private void Start()
    {
        if (_prefab == null)
        {
            Debug.LogError("[RebindActionsPanelSpawner] Prefab not assigned");
            return;
        }

        foreach (InputController.InputActionType type in Enum.GetValues(typeof(InputController.InputActionType)))
        {
            if (_onlyExistingInController)
            {
                var actionEvent = _inputController.GetAction(type);
                if (actionEvent?.InputAction?.action == null)
                    continue;
            }

            var vm = _vmFactory.Create(type, _bindingIndex);
            _createdVMs.Add(vm);

            var view = Instantiate(_prefab, _container != null ? _container : transform);
            var runtimeBinder = view.gameObject.AddComponent<RuntimeViewBinder>();
            runtimeBinder.Bind(view, vm);

            vm.Refresh();
        }
    }

    private void OnDestroy()
    {
        foreach (var vm in _createdVMs)
            vm.Dispose();
        _createdVMs.Clear();
    }
}
