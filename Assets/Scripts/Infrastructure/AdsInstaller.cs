using Zenject;

public class AdsInstaller : Installer<string, AdsInstaller>
{
    private readonly string _rewardedId;

    public AdsInstaller(string rewardedId) => _rewardedId = rewardedId;

    public override void InstallBindings()
    {
        BindService();
        BindAutoInitializer();
    }

    private void BindService()
    {
        Container.Bind<IAdService>()
            .To<AdMobService>()
            .AsSingle()
            .WithArguments(_rewardedId);
    }

    private void BindAutoInitializer()
    {
        Container.BindInterfacesTo<AdsBootstrap>().AsSingle().NonLazy();
    }

    private sealed class AdsBootstrap : IInitializable
    {
        private readonly IAdService _ads;
        public AdsBootstrap(IAdService ads) => _ads = ads;
        public void Initialize() => _ads.Initialize();
    }
}
