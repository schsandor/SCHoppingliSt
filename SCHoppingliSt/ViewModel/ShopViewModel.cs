using CommunityToolkit.Maui.Core.Extensions;
using System.Linq.Dynamic.Core;

namespace SCHoppingliSt.ViewModel
{
    [QueryProperty(nameof(ShopOverview), nameof(ShopOverview))]
    public partial class ShopViewModel : BaseViewModel
    {
        [ObservableProperty]
        ShopOverview shopOverview;

        [ObservableProperty]
        ObservableCollection<ItemToBuy> itemsOnList = new();


        [ObservableProperty]
        ObservableCollection<ItemToBuy> itemsInBasket = new();

        public ShopViewModel()
        {
            //Task.Run(async () =>
            //{
            //    await GetItems(ShopOverview.ShopName);
            //});

        }

        public async Task GetItems(string shopName)
        {
            LiteDBService dataService = new();
            var fullist = await dataService.GetAllTheItemsTheShopSells(shopName);
            List<ItemToBuy> tempItemsOnList = new();
            List<ItemToBuy> tempItemsInBasket = new();
            foreach (var item in fullist)
            {
                int index = item.InShopDataList.FindIndex(shop => shop.ShopName == shopName);
                if (item.InShopDataList[index].OnList)
                {
                    tempItemsOnList.Add(item);
                }
                if (item.InShopDataList[index].InBasket)
                {
                    tempItemsInBasket.Add(item);
                }
            }
            if (tempItemsOnList.Count > 0)
            {
                ItemsOnList.Clear();
                ItemsOnList = OrderItems(tempItemsOnList.ToObservableCollection());
                ShopOverview.ItemsOnList = ItemsOnList.Count();
                var shopsaved = await dataService.SetShop(ShopOverview, false);
            }
            if (tempItemsInBasket.Count > 0)
            {
                ItemsInBasket.Clear();
                ItemsInBasket = OrderItems(tempItemsInBasket.ToObservableCollection());
            }
        }

        partial void OnShopOverviewChanged(ShopOverview value)
        {
            Title = value.ShopName;
        }

        public ObservableCollection<ItemToBuy> OrderItems(ObservableCollection<ItemToBuy> itemsToBuy)
        {
            //var sorted = ItemsOnList.OrderBy(item =>
            //{
            //    var inShopData = item.InShopDataList.FirstOrDefault(inShop => inShop.ShopName == ShopOverview.ShopName);
            //    return inShopData != null ? inShopData.LocationInShop : null;
            //}).ToList();
            var sortedList = itemsToBuy
                .Select(item => new
                {
                    Item = item,
                    LocationInShop = item.InShopDataList.FirstOrDefault(inShop => inShop.ShopName == ShopOverview.ShopName)?.LocationInShop
                })
                .OrderBy(x => x.LocationInShop)
                .Select(x => x.Item)
                .ToList();

            return new ObservableCollection<ItemToBuy>(sortedList);
        }

        [RelayCommand]
        public async Task PutInBasket(ItemToBuy itemToBuy)
        {
            Trace.WriteLine($"Putting {itemToBuy} into the basket");
            int index = itemToBuy.InShopDataList.FindIndex(c => c.ShopName == ShopOverview.ShopName);
            itemToBuy.InShopDataList[index].OnList = false;
            itemToBuy.InShopDataList[index].InBasket = true;
            itemToBuy.InShopDataList[index].LocationInShop = ShopOverview.LocationCounter;
            itemToBuy.InShopDataList[index].PopularityCounter++;
            ShopOverview.LocationCounter++;
            LiteDBService dataService = new();
            var itemsaved = await dataService.SetItemToBuy(itemToBuy, false);
            var shopsaved = await dataService.SetShop(ShopOverview, false);
            index = ItemsOnList.IndexOf(ItemsOnList.Where(x => x.ItemName == itemToBuy.ItemName).FirstOrDefault());
            ItemsOnList.RemoveAt(index);
            ItemsInBasket.Add(itemToBuy);
        }

        [RelayCommand]
        public async Task PutBackOnShelf(ItemToBuy itemToBuy)
        {
            Trace.WriteLine($"Putting {itemToBuy} back on the shelf");
            int index = itemToBuy.InShopDataList.FindIndex(c => c.ShopName == ShopOverview.ShopName);
            itemToBuy.InShopDataList[index].OnList = true;
            itemToBuy.InShopDataList[index].InBasket = false;
            itemToBuy.InShopDataList[index].LocationInShop = ShopOverview.LocationCounter;
            itemToBuy.InShopDataList[index].PopularityCounter--;
            ShopOverview.LocationCounter++;
            LiteDBService dataService = new();
            var itemsaved = await dataService.SetItemToBuy(itemToBuy, false);
            var shopsaved = await dataService.SetShop(ShopOverview, false);
            index = ItemsInBasket.IndexOf(ItemsOnList.Where(x => x.ItemName == itemToBuy.ItemName).FirstOrDefault());
            ItemsInBasket.RemoveAt(index);
            ItemsOnList.Add(itemToBuy);
            //order the list!
        }

        [RelayCommand]
        async Task GoToAddItemToShopPageAsync()
        {
            //if (shopOverview.ShopName is null) return;
            Trace.WriteLine($"Opening the add item page for {ShopOverview.ShopName} shop");

            var navigationParameter = new Dictionary<string, object> { { "ShopName", ShopOverview } };
            await Shell.Current.GoToAsync($"{nameof(ShopPage)}", true, navigationParameter);
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
