using LiteDB;

namespace SCHoppingliSt.Services
{
    public class LiteDBService : IDataService
    {
        private const string ItemCollectionName = "items";
        private const string ShopCollectionName = "shops";


        /// <summary>
        /// Gets the connection string from the secure storage.
        /// </summary>
        /// <returns>The connection string for the db.</returns>
        private async Task<string> GetConnectionString()
        {
            var path = FileSystem.Current.AppDataDirectory;
            var fullPath = Path.Combine(path, "schoppinglist.db");
            if (!File.Exists(fullPath)) 
            { 
                await File.WriteAllTextAsync(fullPath, "");
                //return null;
            }

            var connectionString = $"Filename={fullPath}";
            return connectionString;
        }


        //GetItemToBuy
        public async Task<ItemToBuy> GetItemToBuy(string itemName)
        {
            string connectionString = await GetConnectionString();
            if (string.IsNullOrEmpty(connectionString)) { return null; }
            else
            {
                ItemToBuy itemToBuy = new();
                using var db = new LiteDatabase(connectionString);
                try
                {
                    var query = db.GetCollection<ItemToBuy>(ItemCollectionName);
                    query.EnsureIndex(x => x.ItemName);
                    var queryresult = query.Query().Where(c => c.ItemName == itemName).First();
                    if (queryresult != null) itemToBuy = queryresult;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"{ex.Message}. The search was: {itemName}");
                }
                db.Dispose();
                if (itemToBuy.ItemName != null)
                {
                    return itemToBuy;
                }
                else
                {
                    return null;
                }
            }
        }

        //Get all the items in a given store
        public async Task<List<ItemToBuy>> GetAllTheItemsTheShopSells(string shopName)
        {
            string connectionString = await GetConnectionString();
            List<ItemToBuy> templist = new();
            if (string.IsNullOrEmpty(connectionString)) { return templist; }
            else
            {
                using var db = new LiteDatabase(connectionString);
                var query = db.GetCollection<ItemToBuy>(ItemCollectionName);
                try
                {
                    var queryresult = query.Find(item => item.InShopDataList.Any(inshop => inshop.ShopName == shopName)).ToList();
                }
                catch (Exception e)
                {
                    Trace.WriteLine (e.ToString());
                }
                return templist;
            }
        }

            //SetItemToBuy
            public async Task<bool> SetItemToBuy(ItemToBuy itemToBuy, bool delete)
        {
            string connectionString = await GetConnectionString();
            if (string.IsNullOrEmpty(connectionString)) { return false; }
            else
            {
                ItemToBuy tempItemToBuy = new()
                {
                    ItemName = itemToBuy.ItemName,
                    InShopDataList = itemToBuy.InShopDataList,
                };

                using var db = new LiteDatabase(connectionString);
                var query = db.GetCollection<ItemToBuy>(ItemCollectionName);
                try
                {
                    var queryresult = query.Find(c => c.ItemName == itemToBuy.ItemName).First();
                    if (queryresult.ItemName != null)
                    {
                        if (delete)
                        {
                            query.Delete(queryresult.Id);
                        }
                        else
                        {
                            tempItemToBuy.Id = queryresult.Id;
                            query.Update(tempItemToBuy);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    tempItemToBuy.Id = new ObjectId();
                    var id = query.Insert(tempItemToBuy);
                    Trace.WriteLine($"New id {id} added");
                }
                db.Dispose();
                return true;
            }
        }


        /// <summary>
        /// Gets all the shop overviews from the database.
        /// </summary>
        /// <returns>A list of ShopOverview.</returns>
        public async Task<List<ShopOverview>> GetShops()
        {
            string connectionString = await GetConnectionString();
            if (string.IsNullOrEmpty(connectionString)) { return null; }
            else
            {
                List<ShopOverview> list = new();
                using var db = new LiteDatabase(connectionString);
                try
                {
                    var query = db.GetCollection<ShopOverview>(ShopCollectionName);
                    query.EnsureIndex(x => x.ShopName);
                    var queryresult = query.FindAll();
                    if (queryresult == null)
                    {
                        return null;
                    }
                    else
                    {
                        list = queryresult.ToList();
                    }

                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"{ex.Message}");
                }
                db.Dispose();
                return list;
            }
        }

        /// <summary>
        /// Saves a shop to the database or deletes it. When saving it will overwrite the shop with the same name, so that should be checked beforehand.
        /// </summary>
        /// <param name="shop">The shop item to save or delete.</param>
        /// <param name="delete">If true, itt will delete the shop, and also any references to it from the database.</param>
        /// <returns>A bool.</returns>
        public async Task<bool> SetShop(ShopOverview shop, bool delete = false)
        {
            string connectionString = await GetConnectionString();
            if (string.IsNullOrEmpty(connectionString)) { return false; }
            else
            {
                ShopOverview tempShopOverview = new()
                {
                    ShopName = shop.ShopName,
                    Icon = shop.Icon,
                    ItemsOnList = shop.ItemsOnList,
                    LocationCounter = shop.LocationCounter,
                };

                using var db = new LiteDatabase(connectionString);
                var query = db.GetCollection<ShopOverview>(ShopCollectionName);
                try
                {
                    var queryresult = query.Find(c => c.ShopName == shop.ShopName).First();
                    if (queryresult.ShopName != null)
                    {
                        if (delete)
                        {
                            query.Delete(queryresult.Id);
                            var itemquery = db.GetCollection<ItemToBuy>(ItemCollectionName);
                            var itemlist = itemquery.Find(c => c.InShopDataList.Any(x => x.ShopName == tempShopOverview.ShopName)).ToList();
                            if (itemlist != null && itemlist.Count > 0)
                            {
                                foreach (var item in itemlist)
                                {
                                    int index = item.InShopDataList.FindIndex(y => y.ShopName == tempShopOverview.ShopName);
                                    item.InShopDataList.RemoveAt(index);
                                    await SetItemToBuy(item, false);
                                }
                            }
                        }
                        else
                        {
                            tempShopOverview.Id = queryresult.Id;
                            query.Update(tempShopOverview);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    tempShopOverview.Id = new ObjectId();
                    var id = query.Insert(tempShopOverview);
                    Trace.WriteLine($"New id {id} added");
                }
                db.Dispose();
                return true;
            }
        }
    }
}
