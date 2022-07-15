
using ESATouristGuide.Models;

using System.Collections.Generic;
using System.Windows.Input;

using Xamarin.Forms;

namespace ESATouristGuide.ViewModels
{
    public class PlaygroundPageViewModel : BaseViewModel
    {
        public PlaygroundPageViewModel()
        {
            Categories = Models.Categories.CategoriesList;
            OpenDrawerCommand = new Command(OpenDrawer);
            SecondaryCategoriesSelectedCommand = new Command(SecondaryCategoriesEnable);
            BasicCategoriesSelectedCommand = new Command(BasicCategoriesEnable);
            SelectedIndexes = new List<int>();
            BasicCategoriesChangedCommand = new Command(BasicCategoriesChanged);
        }


        public List<Category> Categories { get; set; } /*= Models.Categories.CategoriesList;*/

        bool isDrawerOpen;

        public bool IsDrawerOpen
        {
            get { return isDrawerOpen; }
            set
            {
                //isDrawerOpen = value;
                SetAndRaise(ref isDrawerOpen , value);
                //RaisePropertyChanged();
            }
        }

        bool secondaryCategoriesVisible;

        public bool SecondaryCategoriesVisible
        {
            get { return secondaryCategoriesVisible; }
            set
            {
                //isDrawerOpen = value;
                SetAndRaise(ref secondaryCategoriesVisible , value);
                //RaisePropertyChanged();
            }
        }


        bool basicCategoriesVisible = true;

        public bool BasicCategoriesVisible
        {
            get { return basicCategoriesVisible; }
            set
            {
                //isDrawerOpen = value;
                SetAndRaise(ref basicCategoriesVisible , value);
                //RaisePropertyChanged();
            }
        }

        bool basicCategoriesEnabled = true;

        public bool BasicCategoriesEnabled
        {
            get { return basicCategoriesEnabled; }
            set
            {
                //isDrawerOpen = value;
                SetAndRaise(ref basicCategoriesEnabled , value);
                //RaisePropertyChanged();
            }
        }

        bool secondaryOptionsEnabled;

        public bool SecondaryOptionsEnabled
        {
            get { return SelectedIndexes.Count > 0; }
            set { SetAndRaise(ref secondaryOptionsEnabled , value); }
        }

        public ICommand SecondaryCategoriesSelectedCommand { get; set; }

        public ICommand BasicCategoriesSelectedCommand { get; set; }

        public ICommand OpenDrawerCommand { get; set; }

        public ICommand BasicCategoriesChangedCommand { get; set; }


        void OpenDrawer( object obj ) { IsDrawerOpen = true; }

        IReadOnlyList<object> selectedItems;


        public IReadOnlyList<object> SelectedItems
        {
            get => selectedItems;
            set => SetAndRaise(ref selectedItems , value);
        }

        IList<int> selectedIndexes;

        public IList<int> SelectedIndexes
        {
            get => selectedIndexes;
            set
            {
                SetAndRaise(ref selectedIndexes , value);
                RaisePropertyChanged(nameof(SecondaryOptionsEnabled));
            }
        }


        void SecondaryCategoriesEnable( object obj )
        {
            var a = obj;
            SecondaryCategoriesVisible = true;
            BasicCategoriesVisible = false;
        }

        void BasicCategoriesEnable( object obj )
        {
            var a = obj;
            BasicCategoriesVisible = true;
            SecondaryCategoriesVisible = false;
        }

        void BasicCategoriesChanged( object obj ) { RaisePropertyChanged(nameof(SecondaryOptionsEnabled)); }
    }
}
