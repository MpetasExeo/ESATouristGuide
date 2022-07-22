using System;
using System.Collections.Generic;
using System.Text;

namespace ESATouristGuide.Interfaces
{
    public interface IPlatformSpecificLocationService
    {
        bool IsLocationServiceEnabled();
        bool OpenDeviceLocationSettingsPage();
    }
}
