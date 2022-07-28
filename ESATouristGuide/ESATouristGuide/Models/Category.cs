
using Xamarin.Forms;

namespace ESATouristGuide.Models
{
    public class Category
    {
        public int Id { get; set; }
        public ImageSource Image { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }
        public bool IsSelected { get; set; } = true;
    }
}
