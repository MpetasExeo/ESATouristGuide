
using XFTemplateApp.Interfaces;
using XFTemplateApp.Models;

namespace XFTemplateApp.Helpers
{
    public static class UserExperiencePrompts
    {
        public static void NoInternetConnectionPrompt()
        {
            IToastMessage toastMessage = new Toaster();
            toastMessage.MakeToastAsync(StandardToastMessages.No_Internet);
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
