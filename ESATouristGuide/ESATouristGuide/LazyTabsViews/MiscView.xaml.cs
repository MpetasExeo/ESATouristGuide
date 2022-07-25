using ESATouristGuide.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ESATouristGuide.LazyTabsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MiscView : ContentView
    {
        public MiscView()
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
            }
        }

        private void RadioButtonSystem_CheckedChanged(object sender , CheckedChangedEventArgs e)
        {

            var val = (sender as RadioButton)?.Value as string;
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

        private void RadioButtonGreek_CheckedChanged(object sender , CheckedChangedEventArgs e)
        {
            //if (!loaded)
            //{
            //    return;
            //}

            if (!e.Value)
            {
                return;
            }

            var val = (sender as RadioButton)?.Value as string;
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