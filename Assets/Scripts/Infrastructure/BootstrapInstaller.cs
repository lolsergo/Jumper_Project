using UnityEngine;
using Zenject;
using MVVM;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private AudioCoreInstaller _audioInstaller;

    public override void InstallBindings()
    {
        // 1. ����������� ������� �������� (��� � ��� ����)
        BinderFactory.RegisterBinder<ButtonBinder>();
        BinderFactory.RegisterBinder<AudioBinder>();

        // 2. ��������� ������������
        Container.BindInstance(_audioInstaller).AsSingle();
        _audioInstaller.InstallBindings();

        // 3. ����������� ViewModel (��� ������ ������� ������������)
        Container.Bind<AudioButtonViewModel>().AsTransient();
    }
}
