using ESATouristGuide.Models;

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace ESATouristGuide.ViewModels
{
    public class OnboardingViewModel : BaseViewModel
    {
        private ObservableCollection<OnboardingModel> items;
        private int position;
        private string skipButtonText;

        public OnboardingViewModel()
        {
            SetSkipButtonText("Next");
            InitializeOnBoarding();
            InitializeSkipCommand();
        }

        private void SetSkipButtonText(string skipButtonText)
                => SkipButtonText = skipButtonText;

        private void InitializeOnBoarding()
        {
            Items = new ObservableCollection<OnboardingModel>
            {
                new OnboardingModel
                {
                    Title = "Welcome to Tourist Guide",
                    Content = "Tourist Guide helps you find out about the best places in Place.",
                    ImageUrl = "welcome.svg"
                },
                new OnboardingModel
                {
                    Title = "Discover the best places",
                    Content = "If you would like to find the best places around you, please allow Tourist Guide to access your location while using the app.",
                    ImageUrl = "location.svg"
                },
                new OnboardingModel
                {
                    Title = "Track your progress",
                    Content = "Charts help you visualize your efforts over time.",
                    ImageUrl = "info.svg"
                }
            };
        }

        private void InitializeSkipCommand()
        {
            SkipCommand = new MvvmHelpers.Commands.Command(() =>
            {
                if (LastPositionReached())
                {
                    ExitOnBoarding();
                }
                else
                {
                    MoveToNextPosition();
                }
            });
        }

        private static void ExitOnBoarding()
            => Application.Current.MainPage.Navigation.PopModalAsync();

        private async void MoveToNextPosition()
        {
            var nextPosition = ++Position;
            Position = nextPosition;
            switch (Position)
            {
                case 2:
                    await CheckAndRequestLocationPermission();
                    break;
                default:
                    break;
            }
        }


        public async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
            {

            }

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            return status;
        }


        private bool LastPositionReached()
            => Position == Items.Count - 1;

        public ObservableCollection<OnboardingModel> Items
        {
            get => items;
            set => SetAndRaise(ref items , value);
        }

        public string SkipButtonText
        {
            get => skipButtonText;
            set => SetAndRaise(ref skipButtonText , value);
        }

        public int Position
        {
            get => position;
            set
            {
                if (SetAndRaise(ref position , value))
                {
                    UpdateSkipButtonText();
                }
            }
        }

        private void UpdateSkipButtonText()
        {
            if (LastPositionReached())
            {
                SetSkipButtonText("Got it");
            }
            else
            {
                SetSkipButtonText("Next");
            }
        }

        public ICommand SkipCommand { get; private set; }
    }
}
