namespace AHpx.RG.ViewModels;

public class LoadedTypeViewModel : ViewModelBase
{
    private string _loadedTypeName;

    public string LoadedTypeName
    {
        get => _loadedTypeName;
        set => this.RaiseAndSetIfChanged(ref _loadedTypeName, value);
    }

    private bool _loadedTypeSelected;

    public bool LoadedTypeSelected
    {
        get => _loadedTypeSelected;
        set => this.RaiseAndSetIfChanged(ref _loadedTypeSelected, value);
    }

    public LoadedTypeViewModel(string loadedTypeName = null, bool loadedTypeSelected = default)
    {
        _loadedTypeName = loadedTypeName;
        _loadedTypeSelected = loadedTypeSelected;
    }
}