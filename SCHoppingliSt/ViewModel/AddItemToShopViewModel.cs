using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHoppingliSt.ViewModel
{
    [QueryProperty(nameof(ShopOverview), nameof(ShopOverview))]
    public partial class AddItemToShopViewModel : BaseViewModel
    {

        [ObservableProperty]
        ShopOverview shopOverview = new();

        [ObservableProperty]
        List<ItemToBuy> itemsTheShopOffers = new();

        [ObservableProperty]
        List<ItemToBuy> itemsOtherShopsOffer = new();

        //This will list the items that are available in this and other shops
        //TODO:
        //XAML page in View
        //maybe a searchbar to the top?
        //a collectionview with all the items the shop offers AND are not yet on the list or in the basket, ordered by popularity
        //a collectionview with all the items that are available from other shops AND are not available in this shop, ordered alphabetically
        //long press on an item to edit it
        //a button at the bottom to open the EditItem page to create a new item


    }
}
