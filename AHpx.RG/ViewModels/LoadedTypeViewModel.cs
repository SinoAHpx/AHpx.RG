using System;
using AHpx.RG.Services;

namespace AHpx.RG.ViewModels;

public class LoadedTypeViewModel : ViewModelBase
{
    private Type _loadedType;

    public Type LoadedType
    {
        get => _loadedType;
        set => this.RaiseAndSetIfChanged(ref _loadedType, value);
    }

    private bool _loadedTypeSelected;

    public bool LoadedTypeSelected
    {
        get => _loadedTypeSelected;
        set
        {
            this.RaiseAndSetIfChanged(ref _loadedTypeSelected, value);
            ServiceProvider.MessageBus.SendMessage(this);
        }
    }

    public LoadedTypeViewModel(Type loadedType = null, bool loadedTypeSelected = default)
    {
        _loadedType = loadedType;
        _loadedTypeSelected = loadedTypeSelected;
    }
}