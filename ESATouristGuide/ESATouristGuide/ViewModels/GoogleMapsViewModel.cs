
using ESATouristGuide.Interfaces;
using ESATouristGuide.Models;
using ESATouristGuide.Services;

using MvvmHelpers;
using MvvmHelpers.Commands;

using Sharpnado.TaskLoaderView;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

using Command = MvvmHelpers.Commands.Command;
using Map = Xamarin.Forms.GoogleMaps.Map;

namespace ESATouristGuide.ViewModels
{
    public partial class GoogleMapsViewModel : BaseViewModel
    {
        #region Properties
        public IGreekCitiesService GreekCitiesService { get; set; }
        public IContentService ContentService { get; set; }
        public IWeatherService WeatherService { get; set; }
        public ICommand GetCitiesCommand { get; set; }
        public Map GoogleMap { get; set; } = new Map();
        public ObservableRangeCollection<Pin> Pins { get; set; } = new ObservableRangeCollection<Pin>();
        public ICommand NavToDetailsCommand { get; set; }

        private bool mapLoaded;
        public bool MapLoaded
        {
            get => mapLoaded;
            set
            {
                SetAndRaise(ref mapLoaded , value);
                Task.Run(async () => await AfterInitializationTask());
            }
        }

        private bool constructorFinished;
        public bool ConstructorFinished
        {
            get => constructorFinished;
            set
            {
                SetAndRaise(ref constructorFinished , value);
                //Task.Run(() => Load());
            }
        }

        private bool filtersClicked;

        public bool FiltersClicked
        {
            get => filtersClicked;
            set
            {
                if (value)
                {
                    HasSelectedPlace = !value;
                    IsDrawerOpen = value;
                }
                SetAndRaise(ref filtersClicked , value);

            }
        }

        private bool hasSelectedPlace;

        public bool HasSelectedPlace
        {
            get => hasSelectedPlace;
            set
            {
                if (value)
                {
                    FiltersClicked = !value;
                    IsDrawerOpen = value;
                }
                SetAndRaise(ref hasSelectedPlace , value);
            }
        }

        private POISlim selectedPlace;
        public POISlim SelectedPlace
        {
            get => selectedPlace;
            set
            {
                SetAndRaise(ref selectedPlace , value);
            }
        }

        private readonly Temperatures selectedPlaceTemperature;
        //public Temperatures SelectedPlaceTemperature
        //{
        //    get => selectedPlaceTemperature;
        //    set
        //    {
        //        SelectedPlace.Temperatures = value;
        //        RaisePropertyChanged(nameof(SelectedPlace));
        //    }
        //}
        public ObservableRangeCollection<POI> POIS { get; set; }
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
            ServicesAndPropertiesInit();
            UICommandsInit();
            ConstructorFinished = true;
        }

        private async Task NavigateToDetails(POISlim poi)
        {
            await poi.NavigateToDetailsAsync();
        }

        private async Task InitializationTask()
        {
            if (MapLoaded)
            {
                DefineMapStyle();
                return;
            }

            List<Task> tasks = new List<Task>
            {

                // task => Init map 
                MapInitialization()
            };

            //περιμένω να Init map 
            await Task.WhenAll(tasks).ConfigureAwait(true);


            await Task.Delay(2000);

            //ο χάρτης φόρτωσε
            MapLoaded = true;
        }

        private void DefineMapStyle()
        {
            var assembly = typeof(GoogleMapsViewModel).GetTypeInfo().Assembly;

            var currentTheme = Application.Current.RequestedTheme;

            var assemblyStream = System.String.Empty;

            if (currentTheme == OSAppTheme.Light)
            {
                assemblyStream = "ESATouristGuide.Json.travelstyle.json";

            }
            else
            {
                assemblyStream = "ESATouristGuide.Json.darkstyle.json";
            }

            var stream = assembly.GetManifestResourceStream(assemblyStream);

            string styleFile;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
            {
                styleFile = reader.ReadToEnd();
            }

            GoogleMap.MapStyle = MapStyle.FromJson(styleFile);
        }

        private async Task AfterInitializationTask()
        {
            List<Task> tasks = new List<Task>
            {
                GetCitiesAsync()
            };

            UICommandsInit();

            await Task.WhenAll(tasks);
        }

        public override void Load()
        {
            LoaderNotifier.Load(_ => InitializationTask());
        }

        private void UICommandsInit()
        {
            GoogleMap.PinClicked += GoogleMap_PinClicked;
            NavToDetailsCommand = new AsyncCommand<POISlim>(NavigateToDetails);
            OpenFiltersDrawerCommand = new Command(OpenFiltersDrawer);
        }


        public List<Category> Categories { get; set; } = new List<Category>();/*= Models.Categories.CategoriesList;*/

        private bool isDrawerOpen;

        public bool IsDrawerOpen
        {
            get { return isDrawerOpen; }
            set
            {
                if (value)
                {
                    SetAndRaise(ref hasSelectedPlace , !value);
                }
                SetAndRaise(ref isDrawerOpen , value);

            }
        }

        public ICommand OpenFiltersDrawerCommand { get; set; }

        private void OpenFiltersDrawer() { FiltersClicked = true; }

        private void ServicesAndPropertiesInit()
        {
            GreekCitiesService = new GreekCitiesService();
            WeatherService = new WeatherService();
            ContentService = new ContentService();
            Categories = Models.Categories.CategoriesList;
        }

        private async Task MapInitialization()
        {
            await Task.Run(() =>
            {
                GoogleMap = new Xamarin.Forms.GoogleMaps.Map
                {
                    MyLocationEnabled = true ,
                };
                DefineMapStyle();
                GoogleMap.UiSettings.MyLocationButtonEnabled = true;
                CameraPosition cameraPosition = new CameraPosition(new Position(40.57444939059151 , 22.995289142328364) , 13);
                GoogleMap.InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                GoogleMap.UiSettings.MyLocationButtonEnabled = false;
            }).ConfigureAwait(true);
        }

        #region Pin Clicked 

        private void GoogleMap_PinClicked(object sender , PinClickedEventArgs e) => PinSelected(e);

        private void GetSelectedPlaceTemperature(double lat , double lon)
        {
            ThreadPool.QueueUserWorkItem(o => GetTemperatureAsync(lat , lon));
        }

        private async void GetTemperatureAsync(double lat , double lon)
        {
            Location pos = new Location(lat , lon);
            CancellationToken ct = new CancellationToken();

            //SelectedPlaceTemperature = await WeatherService.GetCurrentWeatherAsync(pos , ct);

            IsBusy = false;
            TemperaturesState = LayoutState.None;
        }

        private void PinSelected(PinClickedEventArgs e)
        {
            e.Handled = true;

            try
            {
                if (SelectedPlace == null)
                {
                    FindSelectedPlace(e.Pin.Position.Latitude , e.Pin.Position.Longitude);
                    return;
                }

                if (SelectedPlace.Longitude == e.Pin.Position.Longitude && SelectedPlace.Latitude == e.Pin.Position.Latitude)
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

        private void FindSelectedPlace(double lat , double lon)
        {
            TemperaturesState = LayoutState.Loading;
            IsBusy = true;

            SelectedPlace =
                POIsList.Where(s => s.Latitude == lat).Where(s => s.Longitude == lon).FirstOrDefault();

            if (!(SelectedPlace is null))
            {
                HasSelectedPlace = true;
            }

            GetSelectedPlaceTemperature(lat , lon);

        }
        #endregion


        List<POISlim> _pOIsList;
        public List<POISlim> POIsList
        {
            get => _pOIsList;
            set => SetAndRaise(ref _pOIsList , value);
        }

        /// <summary>
        /// Calls the CitiesService, populates <see cref="Pins">Pins list</see> and creates a <see cref="Pin">Pin</see> for each item in the list
        /// </summary>
        private async Task GetCitiesAsync()
        {
            //POIS = await GreekCitiesService.GetGreekCities().ConfigureAwait(false);

            int[] array = {1,2,3,4,5,6,7,8,9};

            var data = ContentService.GetPagedListItem(0 , array , page: 1);

            var lastPage = data.TotalPages;

            POIsList = data.Data.ToList();

            for (int i = 1; i < lastPage; i++)
            {
                var tempData = ContentService.GetPagedListItem(0 , array , page: i).Data.ToList();

                foreach (var item in tempData)
                {
                    if (item.Latitude != null && item.Longitude != null)
                    {
                        POIsList.Add(item);
                    }
                }
            }

            foreach (var poi in POIsList)
            {
                var lat = poi.Latitude;
                var lng = poi.Longitude;

                Pin pin = new Pin()
                {
                    Position = new Position((double)lat , (double)lng) ,
                    Label = poi.Title ,
                    Type = PinType.Place ,
                    Icon = BitmapDescriptorFactory.FromBundle(poi.CategoryId.ToString())
                };

                Pins.Add(pin);
            }

            ThreadPool.QueueUserWorkItem(o => AddPinsToMap());
        }

        /// <summary>
        /// Adds pins to map using the UI Thread()
        /// </summary>
        private void AddPinsToMap()
        {
            foreach (var pin in Pins)
            {
                Device.BeginInvokeOnMainThread(() =>
                GoogleMap.Pins.Add(pin));
            }
        }
    }
}
