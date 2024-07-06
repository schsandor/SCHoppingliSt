using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHoppingliSt.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {

        /// <summary>
        /// This is the bool for the activity indicator.
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        /// <summary>
        /// The title of the page.
        /// </summary>
        [ObservableProperty]
        string title;

        /// <summary>
        /// The opposite of the busy bool.
        /// </summary>
        public bool IsNotBusy => !IsBusy;

        /// <summary>
        /// Shows a toast message.
        /// </summary>
        /// <param name="text">The text for the toast.</param>
        /// <returns></returns>
        public async Task ShowToast(string text)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;
            var toast = Toast.Make(text, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);

        }

        /// <summary>
        /// Gets all the ShopOverview data from the database.
        /// </summary>
        /// <returns>A list of ShopOverview.</returns>
        public async Task<List<ShopOverview>> GetAllShops()
        {
            LiteDBService dataService = new();
            var list = await dataService.GetShops();
            if (list != null) { Trace.WriteLine($"Returning {list.Count} shops from database"); }
            return list;
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

        /// <summary>
        /// Goes to the page where you can edit a given item or create a new one.
        /// </summary>
        /// <param name="itemToBuy">The existing item to edit or an empty one for a new.</param>
        /// <returns></returns>
        [RelayCommand]
        async Task GoToEditItemPageAsync(ItemToBuy itemToBuy)
        {
            if (itemToBuy.ItemName is null) return;
            Trace.WriteLine($"Opening {itemToBuy.ItemName} item's details for editing");

            //Todo
            //await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true,
            //    new Dictionary<string, object>()
            //    {
            //        ["DictionaryResult"] = newresult
            //    });
        }
    }
}
