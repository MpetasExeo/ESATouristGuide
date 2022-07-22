
using ESATouristGuide.Helpers;
using ESATouristGuide.Models;

using System.Windows.Input;

using Xamarin.Forms;

using Command = MvvmHelpers.Commands.Command;

namespace ESATouristGuide.ViewModels
{
    public class LandingPageViewModel : BaseViewModel
    {
        public LandingPageViewModel()
        {
            StartAppCommand = new Command(StartApplication);
        }

        public ICommand StartAppCommand { get; set; }

        private void StartApplication()
        {
            if (RequiredChecks.HasInternetConnection())
            {
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                UserExperiencePrompts.NoInternetConnectionPrompt();
            }
        }
    }
}
