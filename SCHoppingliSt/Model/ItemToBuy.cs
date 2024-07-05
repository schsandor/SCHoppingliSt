using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHoppingliSt.Model
{
    public partial class ItemToBuy : ObservableObject
    {

        [BsonId]
        public ObjectId Id { get; set; }


        public string ItemName { get; set; }

        public List<ItemInStore> StoreList { get; set; }

        public async void ResetCounters(string storename = "")
        {
            if (String.IsNullOrEmpty(storename))
            {

            }

        }
    }
}
