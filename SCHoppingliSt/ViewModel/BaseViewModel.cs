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
    }
}
