using LiteDB;

namespace SCHoppingliSt.Model
{
    public partial class ItemToBuy : ObservableObject
    {

        [BsonId]
        public ObjectId Id { get; set; }

        public string ItemName { get; set; }

        public string Icon { get; set; }

        public List<InShopData> InShopDataList { get; set; }

        public async void ResetPopularityCounter(string shopname = "")
        {
            if (String.IsNullOrEmpty(shopname))
            {
                //reset all shop counters
                foreach (var item in InShopDataList)
                {
                    item.PopularityCounter = 0;
                }
            }
            else
            {
                //reset just the chosen shop
                int index;
                try
                {
                    index = InShopDataList.FindIndex(c => c.ShopName == shopname);
                    InShopDataList[index].PopularityCounter = 0;
                }
                catch
                {
                    Trace.WriteLine($"{shopname} shop not found");
                }
            }
        }
    }
}
