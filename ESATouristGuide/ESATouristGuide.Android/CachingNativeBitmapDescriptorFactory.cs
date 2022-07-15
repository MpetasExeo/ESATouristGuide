using System.Collections.Concurrent;

using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Android.Factories;

using AndroidBitmapDescriptor = Android.Gms.Maps.Model.BitmapDescriptor;
using AndroidBitmapDescriptorFactory = Android.Gms.Maps.Model.BitmapDescriptorFactory;

namespace ESATouristGuide.Droid
{
    public sealed class CachingNativeBitmapDescriptorFactory : IBitmapDescriptorFactory
    {
        private readonly ConcurrentDictionary<string , AndroidBitmapDescriptor> _cache
            = new ConcurrentDictionary<string , AndroidBitmapDescriptor>();

        public AndroidBitmapDescriptor ToNative( BitmapDescriptor descriptor )
        {
            DefaultBitmapDescriptorFactory defaultFactory = DefaultBitmapDescriptorFactory.Instance;

            if (!string.IsNullOrEmpty(descriptor.Id))
            {
                int iconId = 0;
                switch (descriptor.Id)
                {
                    case "Πολιτισμός":
                        iconId = Resource.Drawable.place;
                        break;
                    case "Γενική Ανακοίνωση":
                        iconId = Resource.Drawable.place;
                        break;
                    case "Έκτακτη Ειδοποίηση":
                        iconId = Resource.Drawable.place;
                        break;
                    default:
                        iconId = Resource.Drawable.place;
                        break;
                }

                return AndroidBitmapDescriptorFactory.FromResource(iconId);

                //var cacheEntry = _cache.GetOrAdd(descriptor.Id, _ => defaultFactory.ToNative(descriptor));
                //return AndroidBitmapDescriptor.FromResource(iconId);
            }

            return defaultFactory.ToNative(descriptor);
        }
    }
}