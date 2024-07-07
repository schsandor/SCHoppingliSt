using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHoppingliSt.Model
{
    public partial class ShopOverview : ObservableObject 
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string ShopName { get; set; }

        public string Icon { get; set; }

        private int itemsOnList = 0;

        //amikor hozzáadunk vagy leveszünk egyet, akkor ezt is módosítsuk
        public int ItemsOnList
        {
            get
            { 
                return itemsOnList;
            }

            set
            {
                if (value < 0)
                {
                    itemsOnList = 0;
                }
                else
                {
                    itemsOnList = value;
                }
            }
        }

        //egy számláló, amit nullázunk akkor, amikor nulla elem van a kosárban, és minden pipával eggyel növelünk, ezt használjuk a boltban a pozíciójának meghatározására
        public int LocationCounter { get; set; }

    }
}
