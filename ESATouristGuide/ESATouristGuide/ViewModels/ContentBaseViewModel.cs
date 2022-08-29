using ESATouristGuide.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace ESATouristGuide.ViewModels
{
    public partial class ContentBaseViewModel : BaseViewModel
    {
        public IContentService _contentService { get; }

        public ContentBaseViewModel(IContentService contentService) : base(contentService)
        {
            _contentService = contentService;
        }
    }
}
