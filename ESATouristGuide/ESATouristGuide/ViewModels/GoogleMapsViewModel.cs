
using ESATouristGuide.Interfaces;
using ESATouristGuide.Models;

using MvvmHelpers;
using MvvmHelpers.Commands;

using Sharpnado.TaskLoaderView;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace ESATouristGuide.ViewModels
{
    public partial class GoogleMapsViewModel : BaseViewModel
    {
        #region Properties
        public IGreekCitiesService GreekCitiesService { get; set; }
        public IWeatherService WeatherService { get; set; }
        public ICommand GetCitiesCommand { get; set; }
        public Map GoogleMap { get; set; } = new Map();
        public ObservableRangeCollection<Pin> Pins { get; set; } = new ObservableRangeCollection<Pin>();
        bool hasSelectedPlace;
        public bool HasSelectedPlace
        {
            get => hasSelectedPlace;
            set
            {
                SetAndRaise(ref hasSelectedPlace , value);
            }
        }
        City selectedPlace;
        public City SelectedPlace
        {
            get => selectedPlace;
            set
            {
                SetAndRaise(ref selectedPlace , value);
            }
        }
        Temperatures selectedPlaceTemperature;
        public Temperatures SelectedPlaceTemperature
        {
            get => selectedPlaceTemperature;
            set
            {
                SelectedPlace.Temperatures = value;
                RaisePropertyChanged(nameof(SelectedPlace));
            }
        }
        public ObservableRangeCollection<City> Cities { get; set; }
        private LayoutState _temperaturesState;
        public LayoutState TemperaturesState
        {
            get => _temperaturesState;
            set => SetAndRaise(ref _temperaturesState , value);
        }
        public TaskLoaderNotifier LoaderNotifier { get; set; } = new TaskLoaderNotifier();
        #endregion

        public GoogleMapsViewModel()
        {
            PropertiesAndMapInit();
            UICommandsInit();


            Load();
        }



        public ICommand NavToDetailsCommand { get; set; }
        bool mapLoaded;
        public bool MapLoaded
        {
            get => mapLoaded;
            set
            {
                SetAndRaise(ref mapLoaded , value);
                Task.Run(async () => await AfterInitializationTask());
            }


        }
        async Task NavigateToDetails( City city )
        {
            await city.NavigateToDetailsAsync();
        }

        private async Task InitializationTask()
        {
            MapLoaded = true;
        }

        private async Task AfterInitializationTask()
        {
            List<Task> tasks = new List<Task>
            {
                GetCitiesAsync()
            };

            await Task.WhenAll(tasks);
        }

        public override void Load()
        {
            LoaderNotifier.Load(_ => InitializationTask());
        }

        void UICommandsInit()
        {
            GoogleMap.PinClicked += GoogleMap_PinClicked;
            NavToDetailsCommand = new AsyncCommand<City>(NavigateToDetails);
        }

        void PropertiesAndMapInit()
        {
            GreekCitiesService = new GreekCitiesService();
            WeatherService = new WeatherService();
            CameraPosition cameraPosition = new CameraPosition(new Position(40.57444939059151 , 22.995289142328364) , 13);
            GoogleMap.InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            GoogleMap.UiSettings.MyLocationButtonEnabled = true;
        }

        #region Pin Clicked 

        private void GoogleMap_PinClicked( object sender , PinClickedEventArgs e ) => ThreadPool.QueueUserWorkItem(o => PinSelected(e));

        private void GetSelectedPlaceTemperature( double lat , double lon )
        {
            ThreadPool.QueueUserWorkItem(o => GetTemperatureAsync(lat , lon));
        }

        async void GetTemperatureAsync( double lat , double lon )
        {
            Position pos = new Position(lat , lon);
            CancellationToken ct = new CancellationToken();

            SelectedPlaceTemperature = await WeatherService.GetCurrentWeatherAsync(pos , ct);

            IsBusy = false;
            TemperaturesState = LayoutState.None;
        }

        void PinSelected( PinClickedEventArgs e )
        {
            e.Handled = true;

            try
            {
                if (SelectedPlace == null)
                {
                    FindSelectedPlace(e.Pin.Position.Latitude , e.Pin.Position.Longitude);
                    return;
                }

                if (SelectedPlace.Lng == e.Pin.Position.Longitude && SelectedPlace.Lat == e.Pin.Position.Latitude)
                {
                    HasSelectedPlace = true;
                    return;
                }

                FindSelectedPlace(e.Pin.Position.Latitude , e.Pin.Position.Longitude);

            }
            catch (System.Exception ex)
            {
                var st = ex.Message;
                //throw;
            }
        }

        private void FindSelectedPlace( double lat , double lon )
        {
            TemperaturesState = LayoutState.Loading;
            IsBusy = true;

            SelectedPlace = Cities.Where(s => s.Lat == lat).Where(s => s.Lng == lon).FirstOrDefault();

            if (!( SelectedPlace is null ))
            {
                HasSelectedPlace = true;
            }

            GetSelectedPlaceTemperature(lat , lon);
        }

        #endregion

        /// <summary>
        /// Calls the CitiesService, populates <see cref="Pins">Pins list</see> and creates a <see cref="Pin">Pin</see> for each item in the list
        /// </summary>
        private async Task GetCitiesAsync()
        {
            Cities = await GreekCitiesService.GetGreekCities().ConfigureAwait(false);

            foreach (City city in Cities)
            {
                double lat = city.Lat;
                double lng = city.Lng;

                Pin pin = new Pin()
                {
                    Position = new Position(lat , lng) ,
                    Label = city.CityCity ,
                    Type = PinType.Place ,
                    Icon = BitmapDescriptorFactory.FromBundle("exeo_logo.png")
                };

                Pins.Add(pin);
            }

            ThreadPool.QueueUserWorkItem(o => AddPinsToMap());
        }

        /// <summary>
        /// Adds pins to map using the UI Thread()
        /// </summary>
        void AddPinsToMap()
        {
            foreach (Pin pin in Pins)
            {
                Device.BeginInvokeOnMainThread(() =>
                GoogleMap.Pins.Add(pin));
            }
        }
    }
}
