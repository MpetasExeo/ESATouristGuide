using System.Threading.Tasks;

using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

using XFTemplateApp.Interfaces;

namespace XFTemplateApp.Models
{
    public class Toaster : IToastMessage
    {
        public async Task MakeToastAsync( string message )
        {
            await Shell.Current.CurrentPage.DisplayToastAsync(message);
        }

        public async void MakeSnackBarAsync( string message )
        {
            try
            {
                await Shell.Current.CurrentPage.DisplaySnackBarAsync(message , "Ok" , async () => await MakeToastAsync("Ok"));
            }
            catch (System.Exception)
            {
            }
        }
    }
}
