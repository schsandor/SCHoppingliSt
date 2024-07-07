namespace SCHoppingliSt.ViewModel
{
    [QueryProperty(nameof(ShopOverview), nameof(ShopOverview))]
    public partial class ShopViewModel : BaseViewModel
    {
        [ObservableProperty]
        ShopOverview shopOverview;

        [ObservableProperty]
        List<ItemToBuy> itemsOnList = new();


        [ObservableProperty]
        List<ItemToBuy> itemsInBasket = new();

        public ShopViewModel()
        {
        }

        partial void OnShopOverviewChanged(ShopOverview value)
        {
            Title = value.ShopName;
        }

        [RelayCommand]
        public async void PutInBasket(ItemToBuy itemToBuy)
        {
            int index = itemToBuy.InShopDataList.FindIndex(c => c.ShopName == ShopOverview.ShopName);
            itemToBuy.InShopDataList[index].OnList = false;
            itemToBuy.InShopDataList[index].InBasket = true;
            itemToBuy.InShopDataList[index].LocationInShop = ShopOverview.LocationCounter;
            itemToBuy.InShopDataList[index].PopularityCounter++;
            ShopOverview.LocationCounter++;
            LiteDBService dataService = new();
            var itemsaved = await dataService.SetItemToBuy(itemToBuy, false);
            var shopsaved = await dataService.SetShop(ShopOverview, false);
        }

        [RelayCommand]
        public async void PutBackOnShelf(ItemToBuy itemToBuy)
        {
            int index = itemToBuy.InShopDataList.FindIndex(c => c.ShopName == ShopOverview.ShopName);
            itemToBuy.InShopDataList[index].OnList = true;
            itemToBuy.InShopDataList[index].InBasket = false;
            itemToBuy.InShopDataList[index].LocationInShop = ShopOverview.LocationCounter;
            itemToBuy.InShopDataList[index].PopularityCounter--;
            ShopOverview.LocationCounter++;
            LiteDBService dataService = new();
            var itemsaved = await dataService.SetItemToBuy(itemToBuy, false);
            var shopsaved = await dataService.SetShop(ShopOverview, false);
        }

        


        //This is the main page of a shop while shopping
        //TODO:
        //Maybe a searchbar to the top that can help filter the results. This should work in realtime, not by waiting for enter
        //a collectionview on top with the items that are on the list, ordered by location in the shop
        //a collectionview below that with all the items in the basket, ordered by location in the shop
        //A button on the bottom to open the page to add items to the list: AddItem
        //find a way to clear the list
    }
}
