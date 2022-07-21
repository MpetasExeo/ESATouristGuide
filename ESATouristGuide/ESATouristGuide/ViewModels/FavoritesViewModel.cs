using ESATouristGuide.Database;
using ESATouristGuide.Helpers;
using ESATouristGuide.Interfaces;
using ESATouristGuide.Models;
using ESATouristGuide.Services;

using MvvmHelpers;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;

using Sharpnado.TaskLoaderView;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ESATouristGuide.ViewModels
{
    public class FavoritesViewModel : BaseViewModel
    {
        public FavoritesViewModel()
        {
            PropertiesInit();
            Load();
        }

        #region Properties

        private IUserLocationService _userLocationService;

        //private IWeatherService WeatherService { get; set; }
        private ObservableRangeCollection<POIDatabaseItem> _favourites;
        public ObservableRangeCollection<POIDatabaseItem> Favourites { get => _favourites; set { SetAndRaise(ref _favourites , value); } }

        private ObservableRangeCollection<POI> _favouritesResult;
        public ObservableRangeCollection<POI> FavouritesResult { get => _favouritesResult; set { SetAndRaise(ref _favouritesResult , value); } }

        public TaskLoaderNotifier LoaderNotifier { get; set; } = new TaskLoaderNotifier();
        #endregion

        public IPOIRepository Database { get; set; }

        private async Task InitializationTask()
        {
            Favourites = new ObservableRangeCollection<POIDatabaseItem>(await Database.GetFavoritesAsync());

            foreach (var item in Favourites)
            {
                try
                {
                  FavouritesResult.Add(item.Clone<POIDatabaseItem , POI>());
                }
                catch (Exception)
                {

                    throw;
                }
            }
            
            //await GreekCitiesService.GetGreekCities();
        }

        public override void Load() { LoaderNotifier.Load(_ => InitializationTask()); }

        public IAsyncCommand<POI> NavToDetailsCommand { get; set; }
        public IAsyncCommand ApplyFiltersChangeCommand { get; set; }
        private async Task NavigateToDetails(POI poi) { await poi.NavigateToDetailsAsync(); }

        private void PropertiesInit()
        {
            FavouritesResult = new ObservableRangeCollection<POI>();
            Database = new POIRepository();
            OpenDrawerCommand = new Command(OpenDrawer);
            _userLocationService = new UserLocationService();
            NavToDetailsCommand = new AsyncCommand<POI>(NavigateToDetails);
            //ApplyFiltersChangeCommand = new AsyncCommand(ApplyFiltersChange);
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

    }
}
