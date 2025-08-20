using MVVM;

public class ProfilesInitializer
{
    public ProfilesInitializer(ProfilesListView view, ProfilesListViewModel viewModel)
    {
        var composite = BinderFactory.CreateComposite(view, viewModel);
        composite.Bind(); // <- вот это должно быть
    }
}