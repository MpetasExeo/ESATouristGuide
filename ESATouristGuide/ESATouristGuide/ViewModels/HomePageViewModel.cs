
using ESATouristGuide.Models;

using System.Threading.Tasks;

namespace ESATouristGuide.ViewModels
{

    public class HomePageViewModel : BaseViewModel
    {
        public HomePageViewModel()
        {
            Task.Run(async () => await PerformRequiredChecks());
            // HomePageViewViewModel= new HomePageViewViewModel();
        }

        bool homePageViewLoaded;
        public bool HomePageViewLoaded
        {
            get => homePageViewLoaded;
            set
            {
                SetAndRaise(ref homePageViewLoaded , value);
            }
        }
        public HomePageViewViewModel HomePageViewViewModel { get; set; }

        async Task PerformRequiredChecks()
        {
            RequiredChecks.DetailsPageRequiredChecks();
            {
                HomePageViewViewModel = new HomePageViewViewModel();
                HomePageViewViewModel.Load();
                HomePageViewLoaded = true;
            }

            await Task.WhenAll();
        }
    }
}
