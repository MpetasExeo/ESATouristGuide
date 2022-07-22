
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

namespace ESATouristGuide.ViewModels
{
    public partial class CollectionViewViewModel : BaseViewModel
    {
        #region Properties


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
                //if (value == true)
                //{
                //    Load();
                //}
            }
        }


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
                SelectedCategories = Categories.Where(x => x.IsSelected == true).ToList();
            }
        }

        private List<Category> _selectedCategories;
        public List<Category> SelectedCategories
        {
            get => _selectedCategories;
            set
            {
                SetAndRaise(ref _selectedCategories , value);
                if (!(POIs is null))
                {
                    Filtered = POIs.Where(x => SelectedCategories.Where(sc => sc.IsSelected == true).Contains(x.Category));
                }
            }
        }

        private ObservableRangeCollection<POI> _filteredResults;
        public ObservableRangeCollection<POI> FilteredResults
        {
            get => _filteredResults;
            set => SetAndRaise(ref _filteredResults , value);
        }

        private IEnumerable<POI> _filtered;
        public IEnumerable<POI> Filtered
        {
            get => _filtered;
            set
            {
                SetAndRaise(ref _filtered , value);
                FilteredResults = new ObservableRangeCollection<POI>(_filtered.ToList());
                RaisePropertyChanged(nameof(IsEmptyList));
            }
            //FilteredResults = new ObservableRangeCollection<City>(SelectedCategories.Where(x => x.IsSelected == true).ToList());
        }

        public bool IsEmptyList { get => FilteredResults.Count == 0; }
        #endregion

        private IGreekCitiesService GreekCitiesService { get; set; }

        private IUserLocationService _userLocationService;

        //private IWeatherService WeatherService { get; set; }
        private CancellationToken _ct = new CancellationToken();
        private ObservableRangeCollection<POI> _pois;

        public ObservableRangeCollection<POI> POIs { get => _pois; set { SetAndRaise(ref _pois , value); } }

        public TaskLoaderNotifier LoaderNotifier { get; set; } = new TaskLoaderNotifier();
        #endregion

        public CollectionViewViewModel()
        {
            PropertiesInit();
        }

        public async Task InitializationTask()
        {
            if (IsLoaded)
            {
                await Task.Delay(2000);
                return;
            }

            if (Settings.Position is null)
            {
                await _userLocationService.GetUserLocationAsync(_ct);
            }

            POIs = await GreekCitiesService.GetGreekCities();

            PopulateCitiesList();

            Categories = new List<Category>(Models.Categories.CategoriesList);

            await Task.Delay(2000);

            IsLoaded = true;
        }

        private Task ApplyFiltersChange()
        {
            SelectedCategories = Categories.Where(x => x.IsSelected == true).ToList();
            return Task.FromResult(Categories);
        }
        private async Task NavigateToDetails(POI poi) { await poi.NavigateToDetailsAsync(); }

        public override void Load()
        {

            LoaderNotifier.Load(_ => InitializationTask());

        }

        private void PropertiesInit()
        {
            OpenDrawerCommand = new AsyncCommand(OpenDrawer);
            _userLocationService = new UserLocationService();
            GreekCitiesService = new GreekCitiesService();
            NavToDetailsCommand = new AsyncCommand<POI>(NavigateToDetails);
            ApplyFiltersChangeCommand = new AsyncCommand(ApplyFiltersChange);
        }

        private Task OpenDrawer()
        {
            IsDrawerOpen = true;
            return Task.FromResult(IsDrawerOpen);
        }

        private void PopulateCitiesList()
        {
            var i = 0;
            var categoryIndex = 0;
            try
            {
                foreach (var city in POIs)
                {
                    city.ImageUrl = new Uri(string.Format("https://picsum.photos/720/480?random={0}" , i));

                    if (categoryIndex > 7)
                    {
                        categoryIndex = 0;
                    }

                    city.Category = Models.Categories.CategoriesList[categoryIndex];
                    categoryIndex++;

                    i++;
                }
            }
            catch (Exception ex)
            {
                var st = ex.Message;
            }
        }
    }
}
