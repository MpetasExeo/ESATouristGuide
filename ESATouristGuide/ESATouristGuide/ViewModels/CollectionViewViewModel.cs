using DevExpress.XamarinForms.Core.Filtering;

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

        private CriteriaOperator _filterExpression;
        public CriteriaOperator FilterExpression
        {
            get => _filterExpression;
            set => SetAndRaise(ref _filterExpression , value);
        }


        private async Task InitializationTask()
        {

            await _userLocationService.GetUserLocationAsync(_ct);

            POIs = await GreekCitiesService.GetGreekCities();

            PopulateCitiesList();

            Categories = new List<Category>(Models.Categories.CategoriesList);
            //FilteredResults = new ObservableRangeCollection<City>();

        }


        List<Category> _categories;
        public List<Category> Categories
        {
            get => _categories;
            set
            {
                SetAndRaise(ref _categories , value);
                SelectedCategories = Categories.Where(x => x.IsSelected == true).ToList();
            }
        }


        List<Category> _selectedCategories;
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

        ObservableRangeCollection<POI> _filteredResults;
        public ObservableRangeCollection<POI> FilteredResults
        {
            get => _filteredResults;
            set => SetAndRaise(ref _filteredResults , value);
        }

        IEnumerable<POI> _filtered;
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

        Task ApplyFiltersChange()
        {
            SelectedCategories = Categories.Where(x => x.IsSelected == true).ToList();
            return Task.FromResult(Categories);
        }

        public override void Load() { LoaderNotifier.Load(_ => InitializationTask()); }

        public ICommand NavToDetailsCommand { get; set; }
        public ICommand ApplyFiltersChangeCommand { get; set; }
        private async Task NavigateToDetails(POI poi) { await poi.NavigateToDetailsAsync(); }

        private void PropertiesInit()
        {
            //Categories = Models.Categories.CategoriesList;
            OpenDrawerCommand = new Command(OpenDrawer);
            _userLocationService = new UserLocationService();
            GreekCitiesService = new GreekCitiesService();
            //WeatherService = new WeatherService();
            NavToDetailsCommand = new AsyncCommand<POI>(NavigateToDetails);
            ApplyFiltersChangeCommand = new AsyncCommand(ApplyFiltersChange);
        }

        //public List<Category> Categories { get; set; } =/*= Models.Categories.CategoriesList;*/

        private bool _isDrawerOpen;

        public bool IsDrawerOpen
        {
            get { return _isDrawerOpen; }
            set => SetAndRaise(ref _isDrawerOpen , value);
        }

        public ICommand OpenDrawerCommand { get; set; }

        private void OpenDrawer() { IsDrawerOpen = true; }

        private bool _isLoaded;

        public bool IsLoaded
        {
            get => _isLoaded;
            set
            {
                SetAndRaise(ref _isLoaded , value);
                if (value == true)
                {
                    Load();
                }
            }
        }


        private void PopulateCitiesList()
        {
            var i = 0;
            int categoryIndex = 0;
            try
            {
                foreach (var city in POIs)
                {
                    city.ImageUrl = new Uri(string.Format("https://picsum.photos/1080/720?random={0}" , i));

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
