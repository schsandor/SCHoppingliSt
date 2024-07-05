using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHoppingliSt.Model
{
    public partial class StoreOverview : ObservableObject 
    {
        
        public string StoreName { get; set; }

        //amikor hozzáadunk vagy leveszünk egyet, akkor ezt is módosítsuk
        public int ItemsOnlist { get; set; }

        //egy számláló, amit nullázunk akkor, amikor nulla elem van a kosárban, és minden pipával eggyel növelünk, ezt használjuk a boltban a pozíciójának meghatározására
        public int LocationCounter { get; set; }

    }
}
