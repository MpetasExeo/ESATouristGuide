
using Sharpnado.TaskLoaderView;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.GoogleMaps;

using XFTemplateApp.Interfaces;
using XFTemplateApp.Models;
using XFTemplateApp.Services;

using Command = MvvmHelpers.Commands.Command;

namespace XFTemplateApp.ViewModels
{
    public class ItemDetailsViewModel : BaseViewModel
    {
        #region Properties,Services & Commands
        private ObservableCollection<CustomImage> _images;
        private int _currentImage;
        private LayoutState _mainState;

        public ICommand RefreshCommand => new Command(async () => await GetTemperaturesAsync());

        public ICommand NavToDetailsCommand { get; set; }

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

        public ICommand NavigateToDetailsCommand { get; set; }

        City selectedPlace = new City();

        public City SelectedPlace { get => selectedPlace; set { SetAndRaise(ref selectedPlace , value); } }

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

        public Position CityPosition { get; set; }
        #endregion

        public ItemDetailsViewModel( City city )
        {
            LoadImages();
            PropertiesInit();
            SelectedPlace = city;
            CityPosition = new Position(city.Lat , city.Lng);
            Load();
        }

        void PropertiesInit()
        {
            UserLocationService = new UserLocationService();
            WeatherService = new WeatherService();
            DistancesService = new DistancesService();
        }

        public override void Load() { LoaderNotifier.Load(_ => InitializationTask()); }


        private async Task InitializationTask()
        {
            List<Task> tasks = new List<Task>();
            MainState = LayoutState.Loading;
            IsBusy = true;

            var userLocation = await UserLocationService.GetUserLocationAsync(ct);


            Parallel.Invoke(
                async () =>
                {
                    Temperatures = await WeatherService.GetCurrentWeatherAsync(CityPosition , ct);
                } ,
                async () =>
                {
                    Distances = await DistancesService.GetDistancesFromUserAsync(CityPosition , userLocation);
                }
            );

            Temperatures = await WeatherService.GetCurrentWeatherAsync(CityPosition , ct);

            Distances = await DistancesService.GetDistancesFromUserAsync(CityPosition , userLocation);

            IsBusy = false;
            MainState = LayoutState.None;
        }


        private async Task GetTemperaturesAsync()
        {
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
