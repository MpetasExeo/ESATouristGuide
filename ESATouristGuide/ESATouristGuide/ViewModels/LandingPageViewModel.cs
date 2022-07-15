
using ESATouristGuide.Helpers;
using ESATouristGuide.Models;

using System.Windows.Input;

using Xamarin.Forms;

using XFTemplateApp;

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
        void StartApplication( object obj )
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
