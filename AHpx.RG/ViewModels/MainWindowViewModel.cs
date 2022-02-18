using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using AHpx.RG.Core.Core;
using AHpx.RG.Core.Utils;
using AHpx.RG.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input.Platform;
using Manganese.Text;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;

namespace AHpx.RG.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<LoadedTypeViewModel> LoadedTypes { get; set; } = new();

        #region Dll Loading

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

        private string _compiledDllPath = null!;

        public string CompiledDllPath
        {
            get => _compiledDllPath;
            set => this.RaiseAndSetIfChanged(ref _compiledDllPath, value);
        }

        #endregion

        #region Xml Loading

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

        private string _xmlDocumentationPath = null!;

        public string XmlDocumentationPath
        {
            get => _xmlDocumentationPath;
            set => this.RaiseAndSetIfChanged(ref _xmlDocumentationPath, value);
        }

        #endregion

        #region Viewing types

        public ReactiveCommand<string, Unit>? RefreshLoadedTypesCommand { get; set; }

        private Unit RefreshLoadedTypes(string? path)
        {
            var types = ReflectionUtils.GetTypes();

            LoadedTypes.Clear();

            types.ForEach(x => LoadedTypes.Add(new LoadedTypeViewModel(x!)));

            return Unit.Default;
        }

        public ReactiveCommand<Unit, Unit> ToggleAllLoadedTypesCommand { get; set; }

        private void ToggleAllLoadedTypes()
        {
            if (LoadedTypes.Any(x => x.LoadedTypeSelected))
            {
                foreach (var model in LoadedTypes)
                {
                    model.LoadedTypeSelected = false;
                }
            }
            else
            {
                foreach (var model in LoadedTypes)
                {
                    model.LoadedTypeSelected = true;
                }
            }
        }

        #endregion

        #region Previewer

        public ReactiveCommand<Unit, Unit> RefreshPreviewerCommand { get; set; }

        private string _previewerMarkdown;

        public string PreviewerMarkdown
        {
            get => _previewerMarkdown;
            set => this.RaiseAndSetIfChanged(ref _previewerMarkdown, value);
        }

        private async Task<Unit> RefreshPreviewer()
        {
            if (CompiledDllPath?.EndsWith(".dll") is true && XmlDocumentationPath?.EndsWith(".xml") is true)
            {
                try
                {
                    var raw = LoadedTypes.Where(s => s.LoadedTypeSelected)
                        .Select(x => x.LoadedType);
                    var markdown = _core.GetDocument(raw, RepositoryLink);
                
                    PreviewerMarkdown = markdown;
                }
                catch (Exception e)
                {
                    var result = await MessageBoxManager.GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            WindowStartupLocation = WindowStartupLocation.CenterOwner,
                            ContentHeader = "Oops",
                            ContentTitle = "An exception occurred",
                            ContentMessage = e.ToString(),
                            MaxHeight = 300,
                            MaxWidth = 600,
                            SizeToContent = SizeToContent.Height, 
                            ButtonDefinitions = new[]
                            {
                                new ButtonDefinition { Name = "Copy" },
                                new ButtonDefinition { Name = "OK" },
                            }
                        })
                        .ShowDialog(ServiceProvider.GetMainWindow());

                    if (result == "Copy")
                    {
                        await Application.Current!.Clipboard!.SetTextAsync(e.ToString());
                    }
                }
            }

            return Unit.Default;
        }

        #endregion

        #region Repository link

        private string _repositoryLink;

        public string RepositoryLink
        {
            get => _repositoryLink;
            set => this.RaiseAndSetIfChanged(ref _repositoryLink, value);
        }

        #endregion

        private ReadmeGeneratorCore _core = new();
        
        public MainWindowViewModel()
        {
            InitializeCommands();

            this.WhenAnyValue(x => x.CompiledDllPath)
                .ObserveOn(RxApp.MainThreadScheduler)
                .DistinctUntilChanged()
                .Where(x => !x.IsNullOrEmpty())
                .Subscribe(s =>
                {
                    _core.CompileLibraryPath = s;
                    RefreshLoadedTypes(s);
                });

            this.WhenAnyValue(x => x.XmlDocumentationPath)
                .ObserveOn(RxApp.MainThreadScheduler)
                .DistinctUntilChanged()
                .Where(x => !x.IsNullOrEmpty())
                .Subscribe(s => _core.XmlDocumentationPath = s);

            this.WhenAnyValue(x => x.CompiledDllPath,
                    z => z.XmlDocumentationPath,
                    y => y.RepositoryLink)
                .ObserveOn(RxApp.MainThreadScheduler)
                .DistinctUntilChanged()
                .Subscribe(_ => RefreshPreviewer());
        }

        private void InitializeCommands()
        {
            BrowseDllCommand = ReactiveCommand.CreateFromTask(BrowseDll);
            BrowseXmlCommand = ReactiveCommand.CreateFromTask(BrowseXml);

            RefreshLoadedTypesCommand = ReactiveCommand.Create<string, Unit>(RefreshLoadedTypes);
            ToggleAllLoadedTypesCommand = ReactiveCommand.Create(ToggleAllLoadedTypes);
            RefreshPreviewerCommand = ReactiveCommand.CreateFromTask(RefreshPreviewer);
        }
    }
}
