using ESATouristGuide.Interfaces;
using ESATouristGuide.Models;

using MvvmHelpers.Commands;

using Sharpnado.TaskLoaderView;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ESATouristGuide.ViewModels
{
    public class MiscViewModel : BaseViewModel
    {
        public MiscViewModel()
        {
            PropertiesInit();

        }

        #region Properties
        public ICommand OpenLanguageDrawerCommand { get; set; }
        public ICommand OpenThemeDrawerCommand { get; set; }

        public TaskLoaderNotifier LoaderNotifier { get; set; } = new TaskLoaderNotifier();

        #endregion
        private async Task InitializationTask()
        {
            await Task.Delay(0);
        }

        public override void Load()
        {
            LoaderNotifier.Load(_ => InitializationTask());
        }


        bool _themeClicked;
        public bool ThemeClicked
        {
            get => _themeClicked;
            set
            {
                SetAndRaise(ref _themeClicked , value);
                if (value)
                {
                    LanguageClicked = !value;
                    IsDrawerOpen = value;
                }
            }
        }


        bool _languageClicked;
        public bool LanguageClicked
        {
            get => _languageClicked;
            set
            {
                SetAndRaise(ref _languageClicked , value);
                if (value)
                {
                    ThemeClicked = !value;
                    IsDrawerOpen = value;
                }
            }
        }

        bool _isDrawerOpen;
        public bool IsDrawerOpen
        {
            get => _isDrawerOpen;
            set
            {
                SetAndRaise(ref _isDrawerOpen , value);
            }
        }


        public void OpenLanguageDrawer() { LanguageClicked = true; }
        public void OpenThemeDrawer() { ThemeClicked = true; }
        void PropertiesInit()
        {
            OpenThemeDrawerCommand = new Command(OpenThemeDrawer);
            OpenLanguageDrawerCommand = new Command(OpenLanguageDrawer);
        }
        




    }
}
