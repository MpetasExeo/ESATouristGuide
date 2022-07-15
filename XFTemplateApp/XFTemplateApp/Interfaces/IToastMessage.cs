using System.Threading.Tasks;

namespace XFTemplateApp.Interfaces
{
    public interface IToastMessage
    {
        Task MakeToastAsync( string message );
        void MakeSnackBarAsync( string message );
    }
}
