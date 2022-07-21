using ESATouristGuide.Interfaces;
using ESATouristGuide.Models;

namespace ESATouristGuide.Helpers
{
    public static class UserExperiencePrompts
    {
        public static void NoInternetConnectionPrompt()
        {
            IToastMessage toastMessage = new Toaster();
            toastMessage.MakeToastAsync(StandardToastMessages.No_Internet);
        }

        public static void AddedToFavorites()
        {
            IToastMessage toastMessage = new Toaster();
            toastMessage.MakeToastAsync(StandardToastMessages.Added_To_Favourites);
        }

        public static void RemovedFromFavorites()
        {
            IToastMessage toastMessage = new Toaster();
            toastMessage.MakeToastAsync(StandardToastMessages.Removed_From_Favourites);
        }

        public static void NoGPSPrompt()
        {
            IToastMessage toastMessage = new Toaster();
            toastMessage.MakeToastAsync(StandardToastMessages.No_GPS_Enabled);
        }

        public static void ApplicationCannotStartPrompt()
        {
            IToastMessage toastMessage = new Toaster();
            toastMessage.MakeToastAsync(StandardToastMessages.Application_Cannot_Start);
        }

    }
}
