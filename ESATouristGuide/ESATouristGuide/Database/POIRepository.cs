using ESATouristGuide.Models;

using SQLite;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESATouristGuide.Database
{
    public class POIRepository : IPOIRepository
    {
        private static SQLiteAsyncConnection _db;

        async Task Init()
        {
            if (_db != null)
                return;

            _db = new SQLiteAsyncConnection(Constants.DatabasePath);

            await _db.CreateTableAsync<POIDatabaseItem>();
        }

        public async Task<List<POIDatabaseItem>> GetFavoritesAsync()
        {
            await Init();
            return await _db.Table<POIDatabaseItem>().ToListAsync();
        }

        public async Task<POIDatabaseItem> GetItemAsync(int id)
        {

            await Init();
            return await _db.Table<POIDatabaseItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> GetItemIdAsync(POIDatabaseItem item)
        {
            await Init();

            // TODO αλλαγή κριτιρίων ύπαρξης

            var a = await _db.Table<POIDatabaseItem>().Where(i => i.Name == item.Name).FirstOrDefaultAsync();

            if (!(a is null))
            {
                return a.ID;
            }

            return -1;
        }

        public async Task<int> SaveItemAsync(POIDatabaseItem item)
        {
            await Init();
            return await _db.InsertAsync(item);
        }

        public async Task DeleteItemAsync(int id)
        {
            await Init();
            await _db.DeleteAsync<POIDatabaseItem>(id);
        }

    }
}
