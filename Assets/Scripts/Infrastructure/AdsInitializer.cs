using Zenject;

public class AdsInitializer : IInitializable
{
    private readonly IAdService _ads;

    public AdsInitializer(IAdService ads)
    {
        _ads = ads;
    }

    public void Initialize()
    {
        _ads.Initialize();
    }
}
