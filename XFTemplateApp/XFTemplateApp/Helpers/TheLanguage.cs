using System.Globalization;

namespace XFTemplateApp.Helpers
{
    public static class TheLanguage
    {
        public static void SetLanguage()
        {
            switch (Settings.Language)
            {
                //default
                case 0:
                    LocalizationResourceManager.Instance.SetCulture(CultureInfo.GetCultureInfo("en-US"));
                    break;
                //light
                case 1:
                    LocalizationResourceManager.Instance.SetCulture(CultureInfo.GetCultureInfo("el-gr"));
                    break;
                //dark
                case 2:
                    LocalizationResourceManager.Instance.SetCulture(CultureInfo.GetCultureInfo("de-DE"));
                    break;
            }
        }
    }
}
