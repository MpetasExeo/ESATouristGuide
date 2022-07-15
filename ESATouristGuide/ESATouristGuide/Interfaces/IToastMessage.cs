
using System.Threading.Tasks;

namespace ESATouristGuide.Interfaces
{
    public interface IToastMessage
    {
        Task MakeToastAsync( string message );
        void MakeSnackBarAsync( string message );
    }
}
