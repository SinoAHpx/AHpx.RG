using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using AHpx.RG.Services;
using Avalonia.Controls;
using Manganese.Text;

namespace AHpx.RG.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<LoadedTypeViewModel> LoadedTypes { get; set; }

        #region Dll loading

        public ReactiveCommand<Unit, Unit> BrowseDllCommand { get; set; }

        private async Task<Unit> BrowseDll()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select your compiled CLR dll file",
                AllowMultiple = false,
                Filters = new List<FileDialogFilter>()
                {
                    new()
                    {
                        Extensions = new List<string>
                        {
                            "dll"
                        }
                    }
                }
            };
            var file = await dialog.ShowAsync(ServiceProvider.GetMainWindow());

            if (file?.Any() is true)
            {
                CompiledDllPath = file.First();
            }

            return Unit.Default;
        }

        private string _compiledDllPath;

        public string CompiledDllPath
        {
            get => _compiledDllPath;
            set => this.RaiseAndSetIfChanged(ref _compiledDllPath, value);
        }

        #endregion

        #region XmlLoading

        public ReactiveCommand<Unit, Unit> BrowseXmlCommand { get; set; }

        private async Task<Unit> BrowseXml()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select your xml file that generated automatically when build",
                AllowMultiple = false,
                Filters = new List<FileDialogFilter>()
                {
                    new()
                    {
                        Extensions = new List<string>
                        {
                            "xml"
                        }
                    }
                }
            };
            var file = await dialog.ShowAsync(ServiceProvider.GetMainWindow());

            if (file?.Any() is true)
            {
                XmlDocumentationPath = file.First();
            }

            return Unit.Default;
        }

        private string _xmlDocumentationPath;

        public string XmlDocumentationPath
        {
            get => _xmlDocumentationPath;
            set => this.RaiseAndSetIfChanged(ref _xmlDocumentationPath, value);
        }

        #endregion

        public MainWindowViewModel()
        {
            BrowseDllCommand = ReactiveCommand.CreateFromTask(BrowseDll);
            BrowseXmlCommand = ReactiveCommand.CreateFromTask(BrowseXml);

            this.WhenAnyValue(x => x.CompiledDllPath)
                .Throttle(TimeSpan.FromSeconds(1))
                .DistinctUntilChanged()
                .Where(x => !x.IsNullOrEmpty())
                .Subscribe(x =>
                {
                    LoadedTypes?.Clear();

                    LoadedTypes = new ObservableCollection<LoadedTypeViewModel>
                    {
                        new()
                    };
                });
        }
    }
}
