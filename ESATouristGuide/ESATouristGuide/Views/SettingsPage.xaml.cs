using ESATouristGuide.Helpers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XFTemplateApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            switch (Settings.Theme)
            {
                case 0:
                    RadioButtonSystem.IsChecked = true;
                    break;
                case 1:
                    RadioButtonLight.IsChecked = true;
                    break;
                case 2:
                    RadioButtonDark.IsChecked = true;
                    break;
            }


            switch (Settings.Language)
            {
                case 0:
                    RadioButtonEnglish.IsChecked = true;
                    break;
                case 1:
                    RadioButtonGreek.IsChecked = true;
                    break;
                case 2:
                    RadioButtonDeutsch.IsChecked = true;
                    break;
            }
        }

        bool loaded;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            loaded = true;
        }

        void RadioButton_CheckedChanged( object sender , CheckedChangedEventArgs e )
        {
            if (!loaded || !e.Value)
            {
                return;
            }

            string val = ( sender as RadioButton )?.Value as string;
            if (string.IsNullOrWhiteSpace(val))
            {
                return;
            }

            switch (val)
            {
                case "System":
                    Settings.Theme = 0;
                    break;
                case "Light":
                    Settings.Theme = 1;
                    break;
                case "Dark":
                    Settings.Theme = 2;
                    break;
            }

            TheTheme.SetTheme();
        }

        void LanguageRadioButton_CheckedChanged( object sender , CheckedChangedEventArgs e )
        {
            if (!loaded)
            {
                return;
            }

            if (!e.Value)
            {
                return;
            }

            string val = ( sender as RadioButton )?.Value as string;
            if (string.IsNullOrWhiteSpace(val))
            {
                return;
            }

            switch (val)
            {
                case "English":
                    Settings.Language = 0;
                    break;
                case "Greek":
                    Settings.Language = 1;
                    break;
                case "Deutsch":
                    Settings.Language = 2;
                    break;
            }

            TheLanguage.SetLanguage();
        }
    }
}