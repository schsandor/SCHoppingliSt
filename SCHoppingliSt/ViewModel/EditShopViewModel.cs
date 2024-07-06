using SCHoppingliSt.Resources;
using SCHoppingliSt.Model;

namespace SCHoppingliSt.ViewModel
{
    [QueryProperty(nameof(ShopOverview), nameof(ShopOverview))]
    public partial class EditShopViewModel : BaseViewModel
    {
        [ObservableProperty]
        ShopOverview shopOverview = new();

        [ObservableProperty]
        string name;

        [ObservableProperty]
        string icon;

        //This is the page for editing or creating a shop
        //TODO:
        //should accept a shop for editing or create a new one 
        //we should check before rename and creating a shop for existing shops with that name and ask the user what to do. let's do this cleverly as to avoid triggering this when overwriting itself (we should use the id for this).
        //this should enable also permanent deleting of the shop

        public EditShopViewModel()
        {
            if (ShopOverview != null)
            {
                Name = ShopOverview.ShopName;
                Icon = ShopOverview.Icon;
            }
        }


        [RelayCommand]
        async Task CheckForm()
        {
            if (string.IsNullOrEmpty(Name))
            {
                await ShowToast(AppResources.ShopNameEmptyError);
                return;
            }
            ShopOverview.ShopName = Name;
            if (String.IsNullOrEmpty(Icon))
            {
                Icon = ShopOverview.ShopName.Substring(0, 1).ToUpperInvariant();
            }
            else if (Icon.Length > 1)
            {
                EmojiChecker emojiChecker = new EmojiChecker();
                bool accepted = emojiChecker.CheckEmoji(Icon);
                if (!accepted)
                {
                    await ShowToast(AppResources.IconTooLongError);
                    return;
                }
            }
            ShopOverview.Icon = Icon;
            await CreateOrEditShop(ShopOverview);
        }

        async Task CreateOrEditShop(ShopOverview shop)
        {
            List<ShopOverview> allShops = await GetAllShops();
            if (allShops != null)
            {
                bool overwrite = false;
                if (allShops.Any(c => c.ShopName == shop.ShopName && c.Id != shop.Id))
                {
                    overwrite = await App.Current.MainPage.DisplayAlert(AppResources.ShopNameAlreadyExistsAlertQuestion, AppResources.ShopNameAlreadyExistsAlertMessage, AppResources.Yes, AppResources.No);
                    if (!overwrite) { return; }
                }
            }
            LiteDBService dataService = new();
            var list = await dataService.SetShop(shop, false);
        }

    }
}
