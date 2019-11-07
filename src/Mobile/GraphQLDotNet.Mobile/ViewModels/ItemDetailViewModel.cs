using System;

using GraphQLDotNet.Mobile.Models;

namespace GraphQLDotNet.Mobile.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item? Item { get; set; }
        public ItemDetailViewModel(Item? item = null)
        {
            Title = item?.Text!;
            Item = item;
        }
    }
}
