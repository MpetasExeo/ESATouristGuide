
using Sharpnado.TaskLoaderView;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

using XFTemplateApp.Helpers;
using XFTemplateApp.Views;

namespace XFTemplateApp.ViewModels
{
    public class HomePageViewViewModel : BaseViewModel
    {
        public ICommand Command1 { get; set; }
        public ICommand Command2 { get; set; }
        public ICommand Command3 { get; set; }
        public ICommand Command4 { get; set; }
        public ICommand Command5 { get; set; }

        public HomePageViewViewModel()
        {
            Load();
        }

        private async Task InitializationTask()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(InitCommands());

            //tasks.Add(Task.Delay(5000));

            await Task.WhenAll(tasks);


        }


        public override void Load()
        {
            LoaderNotifier.Load(_ => InitializationTask());
        }


        public TaskLoaderNotifier LoaderNotifier { get; set; } = new TaskLoaderNotifier();

        public bool HomePageViewLoaded { get; set; }



        private async void Button_Clicked()
        {
            await Shell.Current.Navigation.PushAsync(new GoogleMapsPage() , true);
        }

        private async void Button_Clicked_1()
        {
            await Shell.Current.Navigation.PushAsync(new CollectionViewPage() , true);
        }

        private async void Button_Clicked_2()
        {
            await Shell.Current.Navigation.PushAsync(new PlaygroundPage() , true);
        }

        private async void Button_Clicked_3()
        {
            await Shell.Current.Navigation.PushAsync(new ItemDetailsPage() , true);
        }

        private async void Button_Clicked_4()
        {
            UserExperiencePrompts.NoInternetConnectionPrompt();

            await Task.Delay(3250);

            UserExperiencePrompts.NoGPSPrompt();
        }
        Task InitCommands()
        {
            Command1 = new Command(Button_Clicked);

            Command2 = new Command(Button_Clicked_1);

            Command3 = new Command(Button_Clicked_2);

            Command4 = new Command(Button_Clicked_3);

            Command5 = new Command(Button_Clicked_4);

            return Task.CompletedTask;

        }
    }

}
