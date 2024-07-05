namespace SCHoppingliSt.Model
{
    public partial class ItemInStore : ObservableObject
    {

        public string StoreName { get; set; }

        //ezt csak egyszerűen növeljük mindig meg eggyel, amikor felraktuk a listára vagy amikor kipipáltuk a listán
        public int PopularityCounter { get; set; }

        double locationInStore;

        //figyelni kéne, hogy az adott boltban éppen milyen sorrendben vesszük meg a dolgokat, és ezt a számot arányosan hozzáadni ezzel.
        //((newNumberOfElements - 1 )/ x ) * oldAverage + ( 1 / newNumberOfElements) * newElement
        public double LocationInStore
        {
            get { return locationInStore; }

            set { locationInStore = ((PopularityCounter - 1) / PopularityCounter) * locationInStore + (1 / PopularityCounter) * value; }
        }



        public bool OnList { get; set; }

        public bool InBasket { get; set; }
    }
}
