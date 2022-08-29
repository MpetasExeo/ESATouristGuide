
using ESATouristGuide.Helpers;
using ESATouristGuide.Interfaces;
using ESATouristGuide.Models;
using ESATouristGuide.Services;

using MvvmHelpers;
using MvvmHelpers.Commands;

using Sharpnado.TaskLoaderView;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Essentials;

namespace ESATouristGuide.ViewModels
{
    public partial class CollectionViewViewModel : BaseViewModel
    {
        #region Properties

        ICommand loadMoreCommand = null;
        public ICommand LoadMoreCommand
        {
            get { return loadMoreCommand; }
            set
            {
                if (loadMoreCommand != value)
                {
                    loadMoreCommand = value;
                    RaisePropertyChanged("LoadMoreCommand");
                }
            }
        }

        bool isRefreshing = false;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                SetAndRaise(ref isRefreshing , value);
            }
        }

        private int Page { get; set; } = 1;

        bool _canLoadMore = true;
        public bool CanLoadMore
        {
            get
            {
                return _canLoadMore;
            }
            set
            {
                SetAndRaise(property: ref _canLoadMore , value);
            }
        }

        private bool _isDrawerOpen;
        public bool IsDrawerOpen
        {
            get { return _isDrawerOpen; }
            set => SetAndRaise(ref _isDrawerOpen , value);
        }
        private bool _isLoaded;

        public bool IsLoaded
        {
            get => _isLoaded;
            set
            {
                SetAndRaise(ref _isLoaded , value);
            }
        }


        private CancellationToken _ct = new CancellationToken();
        public ICommand OpenDrawerCommand { get; set; }
        public ICommand NavToDetailsCommand { get; set; }
        public ICommand ApplyFiltersChangeCommand { get; set; }

        #region Filtered Results properties

        private List<Category> _categories;

        public List<Category> Categories
        {
            get => _categories;
            set
            {
                SetAndRaise(ref _categories , value);
                SelectedCategories = Categories.Where(x => x.IsSelected == true).Select(x => x.Id).ToList();
            }
        }

        private List<int> _selectedCategories;
        public List<int> SelectedCategories
        {
            get => _selectedCategories;
            set
            {
                SetAndRaise(ref _selectedCategories , value);
            }
        }

        private ObservableRangeCollection<POISlim> _filteredResults;
        public ObservableRangeCollection<POISlim> FilteredResults
        {
            get => _filteredResults;
            set
            {
                SetAndRaise(ref _filteredResults , value);
                //RaisePropertyChanged(nameof(IsEmptyList));
            }
        }

        int _itemsCount;
        public int ItemsCount
        {
            get => _itemsCount;
            set
            {
                SetAndRaise(ref _itemsCount , value);
                RaisePropertyChanged(nameof(IsEmptyList));
            }
        }
        private IContentService _contentService { get; set; }

        private IUserLocationService _userLocationService;

        public bool IsEmptyList
        {
            get { return ItemsCount == 0; }
        }
        #endregion

        //private IWeatherService WeatherService { get; set; }
        public TaskLoaderNotifier LoaderNotifier { get; set; } = new TaskLoaderNotifier();

        public int MaxPage { get; set; } = 0;
        #endregion

        public CollectionViewViewModel()
        {
            PropertiesInit();
        }

        public async Task InitializationTask()
        {
            if (IsLoaded)
            {
                //await Task.Delay(2000);
                return;
            }

            if (Settings.Position is null)
            {
                await _userLocationService.GetUserLocationAsync(_ct).ConfigureAwait(false);
            }

            Categories = new List<Category>(Models.Categories.CategoriesList);
            CanLoadMore = true;

            var data = _contentService.GetPagedListItem(1 , page: Page , category: SelectedCategories.ToArray());

            MaxPage = data.TotalPages;
            ItemsCount = data.TotalItems;

            var intermediary = FilteredResults;
            FilteredResults.Clear();

            foreach (var item in data.Data)
            {
                intermediary.Add(item);
            }

            FilteredResults = new ObservableRangeCollection<POISlim>(intermediary);

            Page++;

            MainThread.BeginInvokeOnMainThread(() => IsRefreshing = false);

            await Task.Delay(2000);

            IsLoaded = true;
        }

        public void ExecuteLoadMoreCommand()
        {

            if (SelectedCategories.Any() && Page <= MaxPage)
            {
                CanLoadMore = true;
            }
            else
            {
                CanLoadMore = false;
            }

            if (CanLoadMore)
            {
                var data = _contentService.GetPagedListItem(1 , page: Page , category: SelectedCategories.ToArray());

                MaxPage = data.TotalPages;
                ItemsCount = data.TotalItems;

                foreach (var item in data.Data)
                {
                    FilteredResults.Add(item);
                }

                Page++;
            }


            if (!SelectedCategories.Any())
            {
                ItemsCount = 0;
            }

            IsRefreshing = false;
        }

        private Task ApplyFiltersChange()
        {
            CanLoadMore = true;
            SelectedCategories = Categories.Where(x => x.IsSelected).Select(x => x.Id).ToList();
            FilteredResults = new ObservableRangeCollection<POISlim>();
            Page = 1;
            return Task.FromResult(Categories);
        }
        private async Task NavigateToDetails(POISlim poi)
        {
            await poi.NavigateToDetailsAsync();
        }

        public override void Load()
        {
            LoaderNotifier.Load(_ => InitializationTask());
        }

        private void PropertiesInit()
        {
            OpenDrawerCommand = new AsyncCommand(OpenDrawer);
            _userLocationService = new UserLocationService();
            _contentService = new ContentService();
            NavToDetailsCommand = new AsyncCommand<POISlim>(NavigateToDetails);
            ApplyFiltersChangeCommand = new AsyncCommand(ApplyFiltersChange);
            LoadMoreCommand = new Xamarin.Forms.Command(ExecuteLoadMoreCommand);
            FilteredResults = new ObservableRangeCollection<POISlim>();
            SelectedCategories = new List<int>();
        }

        private Task OpenDrawer()
        {
            IsDrawerOpen = true;
            return Task.FromResult(IsDrawerOpen);
        }

    }
}
