using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHoppingliSt.ViewModel
{
    [QueryProperty(nameof(ItemToBuy), nameof(ItemToBuy))]
    public partial class EditItemViewModel : BaseViewModel
    {

        [ObservableProperty]
        ItemToBuy itemToBuy = new();

        [ObservableProperty]
        List<ShopOverview> shopsOfferingThisItem = new();

        //This is the page for editing or creating an item
        //TODO:
        //should accept an item for editing or a create a new one 
        //we should check before rename and creating an item for existing items with that name and ask the user what to do. let's do this cleverly as to avoid triggering this when overwriting itself (we should use the id for this).
        //it should list the shops that the item is currently assigned to
        //this should enable also permanent deleting of an item
    }
}
