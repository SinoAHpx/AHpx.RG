using System;

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
            MessageBus.Current.SendMessage(this);
            this.RaiseAndSetIfChanged(ref _loadedTypeSelected, value);
        }
    }

    public LoadedTypeViewModel(Type loadedType = null, bool loadedTypeSelected = default)
    {
        _loadedType = loadedType;
        _loadedTypeSelected = loadedTypeSelected;
    }
}