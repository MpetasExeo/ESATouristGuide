
using ESATouristGuide.Database;
using ESATouristGuide.Helpers;
using ESATouristGuide.Interfaces;
using ESATouristGuide.Models;
using ESATouristGuide.Resources;
using ESATouristGuide.Services;

using MvvmHelpers.Commands;

using Sharpnado.TaskLoaderView;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

using Command = MvvmHelpers.Commands.Command;

namespace ESATouristGuide.ViewModels
{
    public class ItemDetailsViewModel : BaseViewModel
    {
        #region Properties,Services & Commands
        private ObservableCollection<CustomImage> _images;
        private int _currentImage;
        private LayoutState _mainState;

        public IPOIRepository Database { get; set; }
        public ICommand AddToFavouritesCommand { get; set; }

        private CancellationToken ct = new CancellationToken();

        public LayoutState MainState { get => _mainState; set => SetAndRaise(ref _mainState , value); }

        public TaskLoaderNotifier LoaderNotifier { get; set; } = new TaskLoaderNotifier();
        Distances distances;

        public Distances Distances { get => distances; set => SetAndRaise(ref distances , value); }

        public ObservableCollection<CustomImage> Images
        {
            get { return _images; }
            set
            {
                SetAndRaise(ref _images , value);
                ;
            }
        }

        public int CurrentImage { get { return _currentImage; } set { SetAndRaise(ref _currentImage , value); } }

        IWeatherService WeatherService { get; set; }

        public IUserLocationService UserLocationService { get; set; }

        public IDistancesService DistancesService { get; set; }

        POI _selectedPOI = new POI();

        public POI SelectedPOI { get => _selectedPOI; set { SetAndRaise(ref _selectedPOI , value); } }

        Temperatures temperatures;

        public Temperatures Temperatures
        {
            get => temperatures;
            set
            {
                temperatures = value;
                SetAndRaise(ref temperatures , value);
            }
        }

        public Location CityPosition { get; set; }
        bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set => SetAndRaise(ref _isFavorite , value);
        }
        #endregion

        public ItemDetailsViewModel(POI poi)
        {
            LoadImages();
            PropertiesInit();
            SelectedPOI = poi;
            CityPosition = new Location(poi.Latitude , poi.Longitude);
        }

        void PropertiesInit()
        {
            //UserLocationService = new UserLocationService();
            Database = new POIRepository();
            WeatherService = new WeatherService();
            DistancesService = new DistancesService();
            AddToFavouritesCommand = new AsyncCommand(AddToFavourites);
        }
        async Task AddToFavourites()
        {
            try
            {
                if (IsFavorite)
                {
                    UserExperiencePrompts.RemovedFromFavorites();
                    await Database.DeleteItemAsync(Poi.ID);
                }
                else
                {
                    UserExperiencePrompts.AddedToFavorites();
                    await Database.SaveItemAsync(Poi);
                }
                IsFavorite = !IsFavorite;
            }
            catch (System.Exception ex)
            {
                var msg = ex.Message;
            }
        }

        POIDatabaseItem _poi;
        public POIDatabaseItem Poi
        {
            get => _poi;
            set => SetAndRaise(ref _poi , value);
        }

        public override void Load() { LoaderNotifier.Load(_ => InitializationTask()); }


        private async Task InitializationTask()
        {
            Poi = SelectedPOI.Clone<POI , POIDatabaseItem>();
            Poi.ID = await Database.GetItemIdAsync(Poi);
            IsFavorite = Poi.ID != -1;

            MainState = LayoutState.Loading;

            IsBusy = true;

            //var userLocation = await UserLocationService.GetUserLocationAsync(ct);

            Temperatures = await WeatherService.GetCurrentWeatherAsync(CityPosition , ct);

            Distances = await DistancesService.GetDistancesFromUserAsync(CityPosition , Settings.Position);

            IsBusy = false;

            MainState = LayoutState.None;
        }

        void LoadImages()
        {
            Images = new ObservableCollection<CustomImage>
            {
                new CustomImage() { ImageUrl = "i1.jpg", Name = "id" },
                new CustomImage() { ImageUrl = "i1.jpg", Name = "id" },
                new CustomImage() { ImageUrl = "i1.jpg", Name = "id" }
            };
        }
    }
}
