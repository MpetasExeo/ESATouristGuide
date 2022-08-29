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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ESATouristGuide.ViewModels
{
    public class FavoritesViewModel : BaseViewModel
    {
        public FavoritesViewModel()
        {
            PropertiesInit();
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
            Database = new POIRepository();
            FavouritesResult.Clear();// = new ObservableRangeCollection<POIDatabaseItem>();
            Favourites = new ObservableRangeCollection<POIDatabaseItem>(await Database.GetFavoritesAsync());
            ItemsCount = Favourites.Count;

            foreach (var item in Favourites)
            {
                try
                {
                    //if (Favourites.Where(i=>i.Id == item.Id).FirstOrDefault() is null)
                    //{
                        FavouritesResult.Add(item.Clone<POIDatabaseItem , POI>());
                    //}
                }
                catch (Exception)
                {

                    throw;
                }
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

        public bool IsEmptyList
        {
            get { return ItemsCount == 0; }
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
        }

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
