using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.ViewModels.Common
{
    public sealed class PageViewModelMapping
    {
        private readonly Dictionary<Type, Type> pageViewModelMapping;
        private readonly Dictionary<Type, Type> viewModelPageMapping;

        public PageViewModelMapping()
        {
            pageViewModelMapping = new Dictionary<Type, Type>();
            viewModelPageMapping = new Dictionary<Type, Type>();
        }

        public void AddMapping<TViewModel, TPage>()
            where TViewModel : ViewModelBase
            where TPage : Page
        {
            pageViewModelMapping.Add(typeof(TPage), typeof(TViewModel));
            viewModelPageMapping.Add(typeof(TViewModel), typeof(TPage));
        }

        public Type GetPageType(Type viewModelType)
            => viewModelPageMapping[viewModelType];

        public Type GetViewModelType(Type pageType)
            => pageViewModelMapping[pageType];
    }
}
