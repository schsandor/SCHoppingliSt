using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHoppingliSt.ViewModel
{
    public partial class MainViewModel : BaseViewModel
    {
        

        public MainViewModel()
        {
            Title = "SCHoppingliSt";

            //This should be the main page with a collectionview of the stores from the database.
        }


        [RelayCommand]
        async Task CreateStore(string name)
        {

        }
    }
}
