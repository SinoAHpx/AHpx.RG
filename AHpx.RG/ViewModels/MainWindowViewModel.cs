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
using DynamicData;
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
        private List<string> _libraryExtensions = new()
        {
            "dll", "so", "dylib"
        };

        public ObservableCollection<LoadedTypeViewModel> LoadedTypes { get; set; } = new();

        private int _loadedTypesBridge;

        public int LoadedTypesBridge
        {
            get => _loadedTypesBridge;
            set => this.RaiseAndSetIfChanged(ref _loadedTypesBridge, value);
        }

        #region Dll Loading

        public ReactiveCommand<Unit, Unit> BrowseDllCommand { get; set; }

        private async Task<Unit> BrowseDll()
        {
            await Task.Run(async () =>
            {
                var dialog = new OpenFileDialog
                {
                    Title = "Select your compiled CLR library file",
                    AllowMultiple = false,
                    Filters = new List<FileDialogFilter>()
                    {
                        new()
                        {
                            Extensions = _libraryExtensions
                        }
                    }
                };
                var file = await dialog.ShowAsync(ServiceProvider.GetMainWindow());

                if (file?.Any() is true)
                {
                    CompiledDllPath = file.First();
                }
            });

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

        private async Task<Unit> RefreshLoadedTypes(string? path)
        {
            await Task.Run(() =>
            {
                var types = ReflectionUtils.GetTypes();

                LoadedTypes.Clear();

                types.ForEach(x => LoadedTypes.Add(new LoadedTypeViewModel(x!)));
            });
            
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
            await Task.Run(async () =>
            {
                if (_libraryExtensions.Any(e => CompiledDllPath?.EndsWith(e) is true)
                    && XmlDocumentationPath?.EndsWith(".xml") is true)
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
            });
            
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

        #region Dependencies

        private string _dependencyPath;

        public string DependencyPath
        {
            get => _dependencyPath;
            set => this.RaiseAndSetIfChanged(ref _dependencyPath, value);
        }

        public ReactiveCommand<Unit, Unit> BrowseDependencyCommand { get; set; }

        public ObservableCollection<string> LoadedLibraries { get; set; }


        private async Task BrowseDependency()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select your compiled CLR library file",
                AllowMultiple = false,
                Filters = new List<FileDialogFilter>()
                {
                    new()
                    {
                        Extensions = _libraryExtensions
                    }
                }
            };
            var file = await dialog.ShowAsync(ServiceProvider.GetMainWindow());

            if (file?.Any() is true)
                LoadedLibraries.Add(file.First());

        }

        #endregion

        private readonly ReadmeGeneratorCore _core = new();
        
        public MainWindowViewModel()
        {
            InitializeCommands();

            this.WhenAnyValue(x => x.CompiledDllPath)
                .ObserveOn(RxApp.MainThreadScheduler)
                .DistinctUntilChanged()
                .Where(x => !x.IsNullOrEmpty())
                .Subscribe(async s =>
                {
                    _core.CompileLibraryPath = s;
                    await RefreshLoadedTypes(s);
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
                .Subscribe(async _ => await RefreshPreviewer());

            // this.WhenAnyValue(x => x.LoadedTypesBridge)
            //     .ObserveOn(RxApp.MainThreadScheduler)
            //     .DistinctUntilChanged()
            //     .Subscribe(async _ => await RefreshPreviewer());

            MessageBus.Current
                .Listen<LoadedTypeViewModel>()
                .Subscribe(async _ => await RefreshPreviewer());
        }

        private void InitializeCommands()
        {
            BrowseDllCommand = ReactiveCommand.CreateFromTask(BrowseDll);
            BrowseXmlCommand = ReactiveCommand.CreateFromTask(BrowseXml);

            RefreshLoadedTypesCommand = ReactiveCommand.CreateFromTask<string, Unit>(RefreshLoadedTypes);
            ToggleAllLoadedTypesCommand = ReactiveCommand.Create(ToggleAllLoadedTypes);
            RefreshPreviewerCommand = ReactiveCommand.CreateFromTask(RefreshPreviewer);

            BrowseDependencyCommand = ReactiveCommand.CreateFromTask(BrowseDependency);
        }
    }
}
