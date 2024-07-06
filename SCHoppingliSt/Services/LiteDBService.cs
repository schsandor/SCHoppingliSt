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
                    ShopList = itemToBuy.ShopList,
                };

                //TODO:
                //create a separate function to check rename and create situations for possible overwrites
                //change this to check for an existing Id instead of name!
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



        //GetShops
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

        //SetShop
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
                    ItemsOnlist = shop.ItemsOnlist,
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
