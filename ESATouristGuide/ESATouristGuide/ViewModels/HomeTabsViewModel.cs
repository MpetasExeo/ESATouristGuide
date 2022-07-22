using ESATouristGuide.Services;

using Sharpnado.TaskLoaderView;

using System.Threading.Tasks;

namespace ESATouristGuide.ViewModels
{
    public class HomeTabsViewModel : BaseViewModel
    {
        private int _selectedViewModelIndex = 2;

        public HomeTabsViewModel()
        {
            CollectionViewViewModel = new CollectionViewViewModel();
            CollectionViewViewModelModel = new CollectionViewViewModel();
            GoogleMapsViewModel = new GoogleMapsViewModel();
            FavoritesViewModel = new FavoritesViewModel();
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
                    CollectionViewViewModel.Load();
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
                default:
                    HomePageViewModel.Load();
                    break;
            }
        }

        private async Task InitializationTask()
        {
            await Task.Delay(2000);
        }



        public FavoritesViewModel FavoritesViewModel { get; }
        public HomePageViewModel HomePageViewModel { get; }

        public GoogleMapsViewModel GoogleMapsViewModel { get; set; }

        public CollectionViewViewModel CollectionViewViewModelModel { get; }

        public CollectionViewViewModel CollectionViewViewModel { get; }

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
