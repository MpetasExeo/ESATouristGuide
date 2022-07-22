namespace ESATouristGuide.Interfaces
{
    public interface IPlatformSpecificLocationService
    {
        bool IsLocationServiceEnabled();
        bool OpenDeviceLocationSettingsPage();
    }
}
