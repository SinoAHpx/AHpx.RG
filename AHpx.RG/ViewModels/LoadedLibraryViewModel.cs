using System;

namespace AHpx.RG.ViewModels;

public class LoadedLibraryViewModel : ViewModelBase
{
    private bool _loadedLibrarySelected;

    public bool LoadedLibrarySelected
    {
        get => _loadedLibrarySelected;
        set => this.RaiseAndSetIfChanged(ref _loadedLibrarySelected, value);
    }

    private string _loadedLibraryType;

    public string LoadedLibraryType
    {
        get => _loadedLibraryType;
        set => this.RaiseAndSetIfChanged(ref _loadedLibraryType, value);
    }

    public LoadedLibraryViewModel(bool loadedLibrarySelected, string loadedLibraryType)
    {
        _loadedLibrarySelected = loadedLibrarySelected;
        _loadedLibraryType = loadedLibraryType;
    }
}