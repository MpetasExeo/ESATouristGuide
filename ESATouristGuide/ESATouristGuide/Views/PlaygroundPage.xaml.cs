
using ESATouristGuide.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ESATouristGuide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaygroundPage : ContentPage
    {
        public PlaygroundPage()
        {
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = this.BindingContext as CollectionViewViewModel;
            if (!vm.IsLoaded)
            {
                vm.IsLoaded = true;
            }
            //vm.Load();
        }

    }
}