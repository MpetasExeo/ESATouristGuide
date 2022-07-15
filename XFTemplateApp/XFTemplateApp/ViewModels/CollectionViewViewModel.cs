using MvvmHelpers;
using MvvmHelpers.Commands;

using Sharpnado.TaskLoaderView;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms.GoogleMaps;

using XFTemplateApp.Interfaces;
using XFTemplateApp.Models;
using XFTemplateApp.Services;

namespace XFTemplateApp.ViewModels
{
    public partial class CollectionViewViewModel : BaseViewModel
    {
        #region Properties
        public IGreekCitiesService GreekCitiesService { get; set; }

        public IUserLocationService UserLocationService { get; set; }

        public IWeatherService WeatherService { get; set; }

        public ICommand GetCitiesCommand { get; set; }

        public Map GoogleMap { get; set; } = new Map();

        public List<Pin> Pins { get; set; } = new List<Pin>();
        private CancellationToken ct = new CancellationToken();
        ObservableRangeCollection<City> cities;

        public ObservableRangeCollection<City> Cities { get => cities; set { SetAndRaise(ref cities, value); } }

        public TaskLoaderNotifier LoaderNotifier { get; set; } = new TaskLoaderNotifier();
        #endregion

        public CollectionViewViewModel()
        {
            Categories = Models.Categories.CategoriesList;
            PropertiesInit();
            IsLoaded = true;
        }

        private async Task InitializationTask()
        {
            List<Task> tasks = new List<Task>();

            Cities = await GreekCitiesService.GetGreekCities();

            PopulateCitiesList();

           await UserLocationService.GetUserLocationAsync(ct);
        }


        public override void Load() { LoaderNotifier.Load(_ => InitializationTask()); }


        public ICommand NavToDetailsCommand { get; set; }

        async Task NavigateToDetails(City city) { await city.NavigateToDetailsAsync(); }

        void PropertiesInit()
        {
            //Categories = Models.Categories.CategoriesList;
            OpenDrawerCommand = new Command(OpenDrawer);
            UserLocationService = new UserLocationService();
            GreekCitiesService = new GreekCitiesService();
            WeatherService = new WeatherService();
            NavToDetailsCommand = new AsyncCommand<City>(NavigateToDetails);
        }

        public List<Category> Categories { get; set; } = new List<Category>();/*= Models.Categories.CategoriesList;*/

        bool isDrawerOpen;

        public bool IsDrawerOpen
        {
            get { return isDrawerOpen; }
            set
            {
                //isDrawerOpen = value;
                SetAndRaise(ref isDrawerOpen, value);
                //RaisePropertyChanged();
            }
        }

        public ICommand OpenDrawerCommand { get; set; }

        bool isLoaded;

        public bool IsLoaded
        {
            get => isLoaded; set
            {
                SetAndRaise(ref isLoaded , value); 
                
                if (value == true)
                {
                    Load();
                } 
            }
        }

        void OpenDrawer(object obj) { IsDrawerOpen = true; }

        private void PopulateCitiesList()
        {
            int i = 0;

            try
            {
                foreach(City city in Cities)
                {
                    city.ImageUrl = new Uri(String.Format("https://picsum.photos/1080/720?random={0}", i));

                    CancellationToken cts = new CancellationToken();

                    if(i < 11)
                    {
                        double lat = city.Lat;
                        double lng = city.Lng;
                        Position pos = new Position(lat, lng);

                        Task.Run(async () => city.Temperatures = await WeatherService.GetCurrentWeatherAsync(pos, cts));

                        i++;
                    }
                    i++;
                }
                ;
            } catch(Exception ex)
            {
                string st = ex.Message;
            }
        }
    }
}
