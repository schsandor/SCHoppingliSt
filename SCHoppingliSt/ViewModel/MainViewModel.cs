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
            //long press on a shop to edit it
        }

        public async Task GetShops()
        {
            var temp = await GetAllShops();
            if (temp != null)
            {
                ShopOverviews.Clear();
                ShopOverviews = temp.ToObservableCollection();
            }
        }

        /// <summary>
        /// Goes to the page where you can edit a given shop or create a new one.
        /// </summary>
        /// <param name="shopOverview">The existing shop to edit or an empty one for a new.</param>
        /// <returns></returns>
        [RelayCommand]
        async Task GoToEditShopPageAsync(ShopOverview shopOverview)
        {
            if (shopOverview != null)
            {
                Trace.WriteLine($"Opening {shopOverview.ShopName} shop's details for editing");
            }
            else
            {
                Trace.WriteLine($"Opening EditShopPage for a creating a new shop");
                shopOverview = new()
                {
                    ShopName = ""
                };
            }
            //Todo
            await Shell.Current.GoToAsync($"{nameof(EditShopPage)}", true,
                new Dictionary<string, object>()
                {
                    ["ShopOverview"] = shopOverview
                });
        }


        [RelayCommand]
        async Task GoToShopPageAsync(ShopOverview shopOverview)
        {
            //if (shopOverview.ShopName is null) return;
            Trace.WriteLine($"Opening {shopOverview.ShopName} shop page");

            var navigationParameter = new Dictionary<string, object> { { "ShopOverview", shopOverview } };
            await Shell.Current.GoToAsync($"{nameof(ShopPage)}", true, navigationParameter);
        }



    }
}
