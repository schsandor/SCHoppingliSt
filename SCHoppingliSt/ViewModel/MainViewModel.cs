using CommunityToolkit.Maui.Core.Extensions;

namespace SCHoppingliSt.ViewModel
{
    public partial class MainViewModel : BaseViewModel
    {
        [ObservableProperty]
        ObservableCollection<ShopOverview> shopOverviews = new();

        public MainViewModel()
        {
            Task.Run(async () =>
            {
                await GetShops();
            }
            );

            //TODO:
            //A collectionview of the shops from the database.
            //A button at the bottom of the page to add a new shop
            //long press on a shop to edit it
        }

        public async Task GetShops()
        {
            var temp = await GetAllShops();
            if (temp != null) 
            {
                ShopOverviews.Clear();
                //await Task.Delay(500);
                ShopOverviews = temp.ToObservableCollection();
            }
        }



        //[RelayCommand]
        //async Task CreateShop(string shop)
        //{
        //    if (!string.IsNullOrEmpty(shop))
        //    {
        //        await GetShops();
        //        if (!ShopOverviews.Any(c => c.ShopName == shop))
        //        {
        //            ShopOverview newShop = new()
        //            {
        //                ShopName = shop,
        //                ItemsOnlist = 0,
        //                LocationCounter = 0,
        //                Icon = shop.Substring(0, 1).ToUpperInvariant()
        //            };
        //            LiteDBService dataService = new();
        //            var list = await dataService.SetShop(newShop, false);
        //        }
        //        else
        //        {
        //            //Error, shop already exists
        //            await ShowToast($"{shop} shop already exists");
        //        }
        //    }
        //}

        [RelayCommand]
        async Task GoToShopPageAsync(ShopOverview shopOverview)
        {
            if (shopOverview.ShopName is null) return;
            Trace.WriteLine($"Opening {shopOverview.ShopName} shop page");

            //Todo
            var navigationParameter = new Dictionary<string, object>
        {
            {"ShopOverview", shopOverview}
        };
            await Shell.Current.GoToAsync($"{nameof(ShopPage)}", true, navigationParameter );

            //await Shell.Current.GoToAsync($"{nameof(ShopPage)}", true,
            //    new Dictionary<string, object>()
            //    {
            //        ["ShopOverview"] = shopOverview,
            //    });
        }



    }
}
