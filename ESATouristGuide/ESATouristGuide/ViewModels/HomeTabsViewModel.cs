using ESATouristGuide.Services;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESATouristGuide.ViewModels
{
    public class HomeTabsViewModel : BaseViewModel
    {
        private int _selectedViewModelIndex = 2;

        public HomeTabsViewModel()
        {
            CollectionViewViewModel = new CollectionViewViewModel();
            GoogleMapsViewModel = new GoogleMapsViewModel();
            FavoritesViewModel = new FavoritesViewModel();
            Task.Run(async () => await userLocationService.GetUserLocationAsync(new System.Threading.CancellationToken()));
        }


        public override void Load()
        {
            switch (SelectedViewModelIndex)
            {
                case 0:
                    if (!CollectionViewViewModel.IsLoaded)
                    {
                        CollectionViewViewModel.IsLoaded = true;
                    }
                    break;
                case 1:
                    GoogleMapsViewModel.Load();
                    break;
                case 2:
                    if (!CollectionViewViewModel.IsLoaded)
                    {
                        CollectionViewViewModel.IsLoaded = true;
                    }
                    break;
                case 3:
                    if (!FavoritesViewModel.IsLoaded)
                    {
                        FavoritesViewModel.IsLoaded = true;
                    }
                    break;
                default:
                    HomePageViewModel.Load();
                    break;
            }
        }

        public FavoritesViewModel FavoritesViewModel { get; }
        public HomePageViewModel HomePageViewModel { get; }

        public bool IsTabVisible { get; set; } = true;
        public GoogleMapsViewModel GoogleMapsViewModel { get; set; }
        //public MiscViewModel MiscViewModel { get; }
        //public PlacesArFiltersViewModel PlacesArFiltersViewModel { get; }

        public CollectionViewViewModel CollectionViewViewModel { get; }

        public int SelectedViewModelIndex
        {
            get => _selectedViewModelIndex;

            set
            {
                SetAndRaise(ref _selectedViewModelIndex , value);
                Load();
            }

        }

        public UserLocationService userLocationService { get; set; } = new UserLocationService();
    }
}
