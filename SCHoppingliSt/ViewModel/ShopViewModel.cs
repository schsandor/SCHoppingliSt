using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHoppingliSt.ViewModel
{
    [QueryProperty(nameof(ShopOverview), nameof(ShopOverview))]
    public partial class ShopViewModel : BaseViewModel
    {
        [ObservableProperty]
        ShopOverview shopOverview = new();

        [ObservableProperty]
        List<ItemToBuy> itemsOnList = new();



        [ObservableProperty]
        List<ItemToBuy> itemsInBasket = new();

        //This is the main page of a shop while shopping
        //TODO:
        //Maybe a searchbar to the top that can help filter the results. This should work in realtime, not by waiting for enter
        //a collectionview on top with the items that are on the list, ordered by location in the shop
        //a collectionview below that with all the items in the basket, ordered by location in the shop
        //A button on the bottom to open the page to add items to the list: AddItem
        //find a way to clear the list
    }
}
