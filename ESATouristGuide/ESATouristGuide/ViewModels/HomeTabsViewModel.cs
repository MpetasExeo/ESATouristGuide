using ESATouristGuide.Interfaces;
using ESATouristGuide.Services;

using Sharpnado.TaskLoaderView;

using System.Threading.Tasks;

using Xamarin.Essentials;

namespace ESATouristGuide.ViewModels
{
    public class HomeTabsViewModel : BaseViewModel
    {
        private int _selectedViewModelIndex = 0;


        public HomeTabsViewModel()
        {
            CollectionViewViewModel = new CollectionViewViewModel();
            HomeViewViewModel = new HomeViewViewModel();
            GoogleMapsViewModel = new GoogleMapsViewModel();
            FavoritesViewModel = new FavoritesViewModel();
            MiscViewModel = new MiscViewModel();

        }

        public TaskLoaderNotifier LoaderNotifier { get; set; } = new TaskLoaderNotifier();

        public override void Load()
        {
            LoaderNotifier.Load(_ => InitializationTask());
        }

        private void LoadSelectedViewModel()
        {
            switch (SelectedViewModelIndex)
            {
                case 0:
                    HomeViewViewModel.Load();
                    break;
                case 1:
                    GoogleMapsViewModel.Load();
                    break;
                case 2:
                    CollectionViewViewModel.Load();
                    break;
                case 3:
                    FavoritesViewModel.Load();
                    break;
                case 4:
                    MiscViewModel.Load();
                    break;
                default:
                    //HomeViewViewModel.Load();
                    break;
            }
        }

        private async Task InitializationTask()
        {
            await Task.Delay(50);
            //HomeViewViewModel.Load();
        }

        public FavoritesViewModel FavoritesViewModel { get; }
        public HomePageViewModel HomePageViewModel { get; }

        public GoogleMapsViewModel GoogleMapsViewModel { get; set; }

        public HomeViewViewModel HomeViewViewModel { get; }

        public CollectionViewViewModel CollectionViewViewModel { get; }
        public MiscViewModel MiscViewModel { get; }


        public int SelectedViewModelIndex
        {
            get => _selectedViewModelIndex;
            set
            {
                SetAndRaise(ref _selectedViewModelIndex , value);
                LoadSelectedViewModel();
            }

        }

        public UserLocationService userLocationService { get; set; } = new UserLocationService();
    }
}
